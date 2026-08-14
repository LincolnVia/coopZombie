using System.Text;
using Microsoft.Xna.Framework.Content;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: XnbEffectExtractor <input.xnb> <output.cso>");
    return 1;
}

using FileStream input = File.OpenRead(args[0]);
using BinaryReader reader = new(input, Encoding.UTF8, leaveOpen: true);

if (Encoding.ASCII.GetString(reader.ReadBytes(3)) != "XNB")
{
    throw new InvalidDataException("Input is not an XNB file.");
}

char platform = (char)reader.ReadByte();
byte version = reader.ReadByte();
byte flags = reader.ReadByte();
int xnbLength = reader.ReadInt32();

if (version is not (4 or 5))
{
    throw new InvalidDataException($"Unsupported XNB version {version}.");
}

Stream payload;
if ((flags & 0x80) != 0)
{
    int decompressedSize = reader.ReadInt32();
    int compressedSize = xnbLength - 14;
    byte[] compressedBytes = reader.ReadBytes(compressedSize);
    if (compressedBytes.Length != compressedSize)
    {
        throw new EndOfStreamException("The compressed XNB data is truncated.");
    }

    MemoryStream compressed = new(compressedBytes, writable: false);
    MemoryStream decompressed = new(new byte[decompressedSize], 0, decompressedSize, writable: true, publiclyVisible: true);
    LzxDecoder decoder = new(16);
    long compressedPosition = 0;

    while (compressedPosition < compressedSize)
    {
        int high = compressed.ReadByte();
        int low = compressed.ReadByte();
        if (high < 0 || low < 0)
        {
            throw new EndOfStreamException("The XNB block header is truncated.");
        }

        int blockSize = (high << 8) | low;
        int frameSize = 0x8000;
        if (high == 0xFF)
        {
            high = low;
            low = compressed.ReadByte();
            int blockHigh = compressed.ReadByte();
            int blockLow = compressed.ReadByte();
            if (low < 0 || blockHigh < 0 || blockLow < 0)
            {
                throw new EndOfStreamException("The extended XNB block header is truncated.");
            }

            frameSize = (high << 8) | low;
            blockSize = (blockHigh << 8) | blockLow;
            compressedPosition += 5;
        }
        else
        {
            compressedPosition += 2;
        }

        if (blockSize == 0 || frameSize == 0)
        {
            break;
        }

        decoder.Decompress(compressed, blockSize, decompressed, frameSize);
        compressedPosition += blockSize;
        compressed.Position = compressedPosition;
    }

    if (decompressed.Position != decompressedSize)
    {
        throw new InvalidDataException($"Expected {decompressedSize} decompressed bytes, got {decompressed.Position}.");
    }

    decompressed.Position = 0;
    payload = decompressed;
}
else
{
    payload = input;
}

using BinaryReader content = new(payload, Encoding.UTF8, leaveOpen: true);
int readerCount = Read7BitEncodedInt(content);
if (readerCount != 1)
{
    throw new InvalidDataException($"Expected one content reader, found {readerCount}.");
}

string readerName = ReadString(content);
_ = content.ReadInt32(); // Reader version.
if (!readerName.Contains("EffectReader", StringComparison.Ordinal))
{
    throw new InvalidDataException($"The XNB contains {readerName}, not an EffectReader.");
}

int sharedResourceCount = Read7BitEncodedInt(content);
if (sharedResourceCount != 0)
{
    throw new InvalidDataException($"Unexpected shared resources: {sharedResourceCount}.");
}

int primaryReaderIndex = Read7BitEncodedInt(content);
if (primaryReaderIndex != 1)
{
    throw new InvalidDataException($"Unexpected primary reader index: {primaryReaderIndex}.");
}

int effectLength = content.ReadInt32();
byte[] effect = content.ReadBytes(effectLength);
if (effect.Length != effectLength)
{
    throw new EndOfStreamException("The effect blob is truncated.");
}

File.WriteAllBytes(args[1], effect);
Console.WriteLine($"Platform: {platform}; XNB version: {version}; reader: {readerName}");
Console.WriteLine($"Extracted {effect.Length:N0} bytes to {Path.GetFullPath(args[1])}");
return 0;

static int Read7BitEncodedInt(BinaryReader reader)
{
    int result = 0;
    int shift = 0;
    while (shift < 35)
    {
        byte value = reader.ReadByte();
        result |= (value & 0x7F) << shift;
        if ((value & 0x80) == 0)
        {
            return result;
        }

        shift += 7;
    }

    throw new FormatException("Invalid 7-bit encoded integer.");
}

static string ReadString(BinaryReader reader)
{
    int length = Read7BitEncodedInt(reader);
    return Encoding.UTF8.GetString(reader.ReadBytes(length));
}
