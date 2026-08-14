using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Structs;

public class StructsClass
{
	public struct ParticleVertex
	{
		public const int SizeInBytes = 36;

		public Short2 Corner;

		public Vector3 Position;

		public Vector3 Velocity;

		public Color Random;

		public float Time;

		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Short2, VertexElementUsage.Position, 0), new VertexElement(4, VertexElementFormat.Vector3, VertexElementUsage.Position, 1), new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0), new VertexElement(28, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0));
	}

	public struct VertexPositionColorNormalTexture
	{
		private Color vColor;

		private Vector3 vPosition;

		private Vector3 vNormal;

		private Vector3 vTangent;

		private Vector2 vTexture;

		private Byte4 blendIndices;

		private Vector4 blendWeights;

		public static int SizeInBytes => 68;

		public float X
		{
			get
			{
				return vPosition.X;
			}
			set
			{
				vPosition.X = value;
			}
		}

		public float Y
		{
			get
			{
				return vPosition.Y;
			}
			set
			{
				vPosition.Y = value;
			}
		}

		public float Z
		{
			get
			{
				return vPosition.Z;
			}
			set
			{
				vPosition.Z = value;
			}
		}

		public float nX
		{
			get
			{
				return vNormal.X;
			}
			set
			{
				vNormal.X = value;
			}
		}

		public float nY
		{
			get
			{
				return vNormal.Y;
			}
			set
			{
				vNormal.Y = value;
			}
		}

		public float nZ
		{
			get
			{
				return vNormal.Z;
			}
			set
			{
				vNormal.Z = value;
			}
		}

		public float tX
		{
			get
			{
				return vTangent.X;
			}
			set
			{
				vTangent.X = value;
			}
		}

		public float tY
		{
			get
			{
				return vTangent.Y;
			}
			set
			{
				vTangent.Y = value;
			}
		}

		public float tZ
		{
			get
			{
				return vTangent.Z;
			}
			set
			{
				vTangent.Z = value;
			}
		}

		public byte R
		{
			get
			{
				return vColor.R;
			}
			set
			{
				vColor.R = value;
			}
		}

		public byte G
		{
			get
			{
				return vColor.G;
			}
			set
			{
				vColor.G = value;
			}
		}

		public byte B
		{
			get
			{
				return vColor.B;
			}
			set
			{
				vColor.B = value;
			}
		}

		public byte A
		{
			get
			{
				return vColor.A;
			}
			set
			{
				vColor.A = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				return vPosition;
			}
			set
			{
				vPosition = value;
			}
		}

		public Color Color
		{
			get
			{
				return vColor;
			}
			set
			{
				vColor = value;
			}
		}

		public Vector3 Normal
		{
			get
			{
				return vNormal;
			}
			set
			{
				vNormal = value;
			}
		}

		public Vector3 Tangent
		{
			get
			{
				return vTangent;
			}
			set
			{
				vTangent = value;
			}
		}

		public Vector2 Texture
		{
			get
			{
				return vTexture;
			}
			set
			{
				vTexture = value;
			}
		}

		public float TextureX
		{
			get
			{
				return vTexture.X;
			}
			set
			{
				vTexture.X = value;
			}
		}

		public float TextureY
		{
			get
			{
				return vTexture.Y;
			}
			set
			{
				vTexture.Y = value;
			}
		}

		public VertexPositionColorNormalTexture(Vector3 pos, Color color, Vector3 normal, Vector3 tangent, Vector2 texture)
		{
			vPosition = pos;
			vColor = color;
			vNormal = normal;
			vTangent = tangent;
			vTexture = texture;
			blendIndices = new Byte4(0f, 0f, 0f, 0f);
			blendWeights = new Vector4(1f, 0f, 0f, 0f);
		}

		public VertexPositionColorNormalTexture(Vector3 pos, Color color, Vector3 normal, Vector3 tangent, Vector2 texture, byte blendIndex0, byte blendIndex1, byte blendIndex2, byte blendIndex3, float blendWeight0, float blendWeight1, float blendWeight2, float blendWeight3)
		{
			vPosition = pos;
			vColor = color;
			vNormal = normal;
			vTangent = tangent;
			vTexture = texture;
			blendIndices = new Byte4((int)blendIndex0, (int)blendIndex1, (int)blendIndex2, (int)blendIndex3);
			blendWeights = new Vector4(blendWeight0, blendWeight1, blendWeight2, blendWeight3);
		}

		public void Set_Values(float x, float y, float z, float nx, float ny, float nz, float tanx, float tany, float tanz, float tx, float ty, byte R, byte G, byte B, byte A)
		{
			vPosition.X = x;
			vPosition.Y = y;
			vPosition.Z = z;
			vNormal.X = nx;
			vNormal.Y = ny;
			vNormal.Z = nz;
			vTangent.X = tanx;
			vTangent.Y = tany;
			vTangent.Z = tanz;
			vTexture.X = tx;
			vTexture.Y = ty;
			vColor.R = R;
			vColor.G = G;
			vColor.B = B;
			vColor.A = A;
		}

		public void Set_Values(float x, float y, float z, float nx, float ny, float nz, float tanx, float tany, float tanz, float tx, float ty, float R, float G, float B, float A, byte blendIndex0, byte blendIndex1, byte blendIndex2, byte blendIndex3, float blendWeight0, float blendWeight1, float blendWeight2, float blendWeight3)
		{
			vPosition.X = x;
			vPosition.Y = y;
			vPosition.Z = z;
			vNormal.X = nx;
			vNormal.Y = ny;
			vNormal.Z = nz;
			vTangent.X = tanx;
			vTangent.Y = tany;
			vTangent.Z = tanz;
			vTexture.X = tx;
			vTexture.Y = ty;
			vColor.R = (byte)(R * 255f);
			vColor.G = (byte)(G * 255f);
			vColor.B = (byte)(B * 255f);
			vColor.A = (byte)(A * 255f);
			blendIndices = new Byte4((int)blendIndex0, (int)blendIndex1, (int)blendIndex2, (int)blendIndex3);
			blendWeights.X = blendWeight0;
			blendWeights.Y = blendWeight1;
			blendWeights.Z = blendWeight2;
			blendWeights.W = blendWeight3;
		}

		public void Set_Values(ref Vector3 Pos, ref Vector3 Norm, float tx, float ty, ref Color c1)
		{
			vPosition.X = Pos.X;
			vPosition.Y = Pos.Y;
			vPosition.Z = Pos.Z;
			vNormal.X = Norm.X;
			vNormal.Y = Norm.Y;
			vNormal.Z = Norm.Z;
			vTexture.X = tx;
			vTexture.Y = ty;
			vColor.R = c1.R;
			vColor.G = c1.G;
			vColor.B = c1.B;
			vColor.A = c1.A;
		}

		public void Set_Values(ref Vector2 Tex, float R, float G, float B, float A)
		{
			vTexture.X = Tex.X;
			vTexture.Y = Tex.Y;
			vColor.R = (byte)(R * 255f);
			vColor.G = (byte)(G * 255f);
			vColor.B = (byte)(B * 255f);
			vColor.A = (byte)(A * 255f);
		}

		public void Set_Values(float size, byte position)
		{
			switch (position)
			{
			case 0:
				vTexture.X = 0f;
				vTexture.Y = 1f;
				vPosition.X = 0f - size;
				vPosition.Y = 0f - size;
				break;
			case 1:
				vTexture.X = 1f;
				vTexture.Y = 1f;
				vPosition.X = size;
				vPosition.Y = 0f - size;
				break;
			case 2:
				vTexture.X = 1f;
				vTexture.Y = 0f;
				vPosition.X = size;
				vPosition.Y = size;
				break;
			case 3:
				vTexture.X = 0f;
				vTexture.Y = 0f;
				vPosition.X = 0f - size;
				vPosition.Y = size;
				break;
			}
			vPosition.Z = 0f;
			vNormal.X = 0f;
			vNormal.Y = 0f;
			vNormal.Z = 1f;
			vColor.R = byte.MaxValue;
			vColor.G = byte.MaxValue;
			vColor.B = byte.MaxValue;
			vColor.A = byte.MaxValue;
		}

		public void Set_Values(float x, float y, float z)
		{
			vPosition.X = x;
			vPosition.Y = y;
			vPosition.Z = z;
		}

		public void Set_Alpha(byte alpha)
		{
			vColor.A = alpha;
		}
	}

	public class vtex
	{
		public float[] v = new float[3];
	}

	public struct vtex_byte
	{
		public byte vx;

		public byte vy;

		public byte vz;
	}

	public class vnorm
	{
		public float[] n = new float[3];
	}

	public class texcoord
	{
		public float[] t = new float[2];
	}

	public struct textureInfo
	{
		public bool[] isAlpha;

		public string[] texNames;

		public short[] texID;

		public short numTex;

		public short numAllocTex;

		public Texture2D[] texData;
	}

	public struct poly
	{
		public int[] v;

		public int[] n;

		public int[] t;

		public vtex tangent;

		public vtex bitangent;
	}

	public struct face
	{
		public byte height;

		public byte width;

		public byte faceID;

		public int texture;

		public vtex[] v1;

		public vnorm[] n1;

		public vtex tangent;

		public vtex bitangent;

		public float u;

		public float v;

		public float texOffsetX;

		public float texOffsetY;
	}

	public struct regen_holder
	{
		public byte[] height;

		public byte[] width;
	}

	public struct particle
	{
		public byte side;

		public sbyte status;

		public byte destroyOrientation;
	}

	public struct particle_list_byte
	{
		public bool bbDirty;

		public long numP;

		public long numUsed;

		public float a1;

		public float a2;

		public float a3;

		public vtex_byte[] v1;

		public vtex pos1;

		public vtex pos2;

		public vtex b1;

		public vtex b2;

		public vtex b3;

		public vtex b4;
	}

	public struct physics
	{
		public float mass;

		public float momentInertiaAxisX;

		public float momentInertiaAxisY;

		public float momentInertiaAxisZ;

		public float totalVelocity;

		public vtex position;

		public vtex velocity;

		public vtex angularVelocity;

		public vtex acceleration;

		public vtex angularAcceleration;

		public vtex angle;

		public float fx;

		public float fy;

		public float fz;

		public float rx;

		public float ry;

		public float rz;

		public double initialTime;
	}

	public struct physics_new
	{
		public float mass;

		public float momentInertiaAxisX;

		public float momentInertiaAxisY;

		public float momentInertiaAxisZ;

		public float x;

		public float y;

		public float z;

		public float velocity;

		public float velocityX;

		public float velocityY;

		public float velocityZ;

		public float accelerationX;

		public float accelerationY;

		public float accelerationZ;

		public float angularVelocityX;

		public float angularVelocityY;

		public float angularVelocityZ;

		public float angularAccerlationX;

		public float angularAccerlationY;

		public float angularAccerlationZ;

		public float forceX;

		public float forceY;

		public float forceZ;

		public float torqueX;

		public float torqueY;

		public float torqueZ;

		public double initialTime;
	}

	public struct particle_list
	{
		public bool bbDirty;

		public long numP;

		public long numUsed;

		public vtex[] v1;

		public vtex pos1;

		public vtex pos2;

		public vtex b1;

		public vtex b2;
	}

	public class gameobject
	{
		public bool destructable;

		public byte faces;

		public byte type;

		public short id;

		public short instanceID;

		public short boxCount;

		public short curBoxes;

		public face[] f1;

		public particle[] pt1;

		public ushort objRefID;

		public uint dimX;

		public uint dimY;

		public uint dimZ;

		public short texID;

		public short[] texIDs = new short[6];

		public long ptX;

		public long pcount;

		public int[] boxList;

		public long fcount;

		public float tScaleX;

		public float tScaleY;

		public float pScale;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float bbX1;

		public float bbY1;

		public float bbZ1;

		public float bbX2;

		public float bbY2;

		public float bbZ2;

		public string texture;

		public float x;

		public float y;

		public float z;

		public float xMoved;

		public float yMoved;

		public float zMoved;

		public float originX;

		public float originY;

		public float originZ;

		public float[] color;

		public bool doPhysics;

		public bool moving;

		public bool isRotated;

		public physics phys1 = default(physics);

		public particle_list_byte pList;

		public Matrix mv;

		public Matrix mvT;

		public int vboPtr;

		public int ibPtr;
	}

	public struct model
	{
		public bool usesAlpha;

		public bool inLevelVBO;

		public byte bufferType;

		public byte numObjects;

		public byte numObjectRotations;

		public byte blendFunction;

		public byte usesRigging;

		public byte numTextures;

		public byte riggingStatus;

		public byte[] blendIndex0;

		public byte[] blendIndex1;

		public byte[] blendIndex2;

		public byte[] blendIndex3;

		public short[] dimX;

		public short[] dimY;

		public short[] dimZ;

		public string name;

		public string texture;

		public string textureNormal;

		public string texSpecular;

		public string[] textureListNames;

		public int ncount;

		public int vcount;

		public int pcount;

		public int vbStart;

		public int ibStart;

		public int triangleCount;

		public int vertexCount;

		public int indexBufferSize;

		public int vertexBufferStart;

		public int tcount;

		public int texID;

		public int texNormalID;

		public int texSpecularID;

		public int[] textureListStart;

		public int[] textureListEnd;

		public int[] textureList;

		public int[] textureListPrimitiveCnt;

		public int[] textureListIndexCnt;

		public short instanceCount;

		public float texMultX;

		public float texMultY;

		public float texXadj;

		public float texYadj;

		public float texMovX;

		public float texMovY;

		public float[] x;

		public float[] y;

		public float[] z;

		public float[] rotX;

		public float[] rotY;

		public float[] rotZ;

		public float[] defaultColor;

		public float[] emissive;

		public byte[] vertexBytes;

		public byte[] normalBytes;

		public byte[] textureBytes;

		public byte[] vIndexBytes;

		public byte[] nIndexBytes;

		public byte[] tIndexBytes;

		public byte[] tangentBytes;

		public byte[] bwBytes0;

		public byte[] bwBytes1;

		public byte[] bwBytes2;

		public byte[] bwBytes3;

		public poly[] p1;

		public vtex[] v1;

		public vnorm[] n1;

		public texcoord[] t1;

		public IndexBuffer mInstanceIndex;
	}

	public class weapon
	{
		public bool autoReload;

		public bool unLimitedAmmo;

		public bool IsHeld;

		public bool IronSightsWileReloading;

		public bool IronSightsWhileChambering;

		public bool ScopeWileReloading;

		public bool ScopeWhileChambering;

		public bool roundChambered;

		public bool ChamberAfterShot;

		public bool hasLaser;

		public bool hasIronSights;

		public byte numAttachmentPoints;

		public byte maxAmmo;

		public byte numSkins;

		public byte fireMode;

		public byte numCrossHairs;

		public byte scopeID;

		public byte foreGripID;

		public byte barrelID;

		public byte energyDeviceID;

		public byte numBarrels;

		public byte heatMax;

		public byte coolMin;

		public byte mountScope;

		public byte mountForeGrip;

		public byte mountBarrel;

		public byte mountEnergyDevice;

		public byte[] crossHairs;

		public sbyte AnimationReload;

		public sbyte AnimationChamber;

		public sbyte AnimationHolding;

		public sbyte AnimationThrow;

		public sbyte AnimationSpecial1;

		public sbyte AnimationWalk;

		public sbyte AnimationIronSights;

		public sbyte AnimationRun;

		public ushort AnimationFire;

		public ushort AnimationChangeWeapon;

		public sbyte jointID;

		public sbyte jointRID;

		public sbyte jointEID;

		public sbyte gripType;

		public sbyte ammoIndex;

		public short magazineCapacity;

		public short currentRounds;

		public short hudIcon;

		public ushort modelID;

		public ushort[] skins;

		public ushort[] skinIcons;

		public string modName;

		public string weaponName;

		public string snd_fire;

		public string snd_reload;

		public string snd_chamber;

		public string snd_fire_empty;

		public float particleDistance;

		public float secsPerBullet = 1f;

		public float centerOfGravityAdjustmentX;

		public float CenterOfGravityAdjustmentY;

		public float heatGeneration;

		public float heatDissipation;

		public float movementFactor;

		public float turningFactor;

		public float[] recoilSide = new float[3];

		public float[] recoilUp = new float[2];

		public float[] recoilBack = new float[2];

		public float fireRate;

		public float fireRateAdjLowPerc;

		public float fireRateReduction;

		public float fireRateRecharge;

		public float attachPointScopeX;

		public float attachPointBarrelX;

		public float attachPointForeGripX;

		public float attachPointEnergyDeviceX;

		public float attachPointScopeY;

		public float attachPointBarrelY;

		public float attachPointForeGripY;

		public float attachPointEnergyDeviceY;

		public float attachPointScopeZ;

		public float attachPointBarrelZ;

		public float attachPointForeGripZ;

		public float attachPointEnergyDeviceZ;

		public float pfx1;

		public float pfx2;

		public float pfx3;

		public float spread;

		public float firstPersonViewX;

		public float firstPersonViewY;

		public float firstPersonViewZ;

		public float ironSightsViewX;

		public float ironSightsViewY;

		public float ironSightsViewZ;

		public float scopeViewX;

		public float scopeViewY;

		public float scopeViewZ;

		public double firingStart;

		public int tracerCnt;

		public int roundsPerTracer = 2;

		public vtex[,] offset;

		public physics weaponLoc = default(physics);

		public particle_list box = default(particle_list);

		public Weapon_Attachment_Point[] attachmentPoints;
	}

	public struct Weapon_Attachment_Point
	{
		public byte category;

		public uint mount;

		public float x;

		public float y;

		public float z;
	}

	public struct Weapon_Stub_Attachment_Point
	{
		public byte status;

		public ushort attachmentID;
	}

	public struct Weapon_Attachment
	{
		public byte category;

		public byte itemID;

		public uint mount;

		public ushort modID;

		public string modelName;
	}

	public struct weapon_stub
	{
		public bool roundChambered;

		public bool fired;

		public bool singleShot;

		public bool active;

		public bool triggerPulled;

		public bool shooting;

		public bool fullyAutomatic;

		public bool needToChamber;

		public bool needToReload;

		public bool reloading;

		public byte numAttachments;

		public byte crossHair;

		public byte curClip;

		public byte weaponID;

		public byte scopeLow;

		public byte scopeHigh;

		public byte scopeID;

		public byte foreGripID;

		public byte barrelID;

		public byte energyDeviceID;

		public byte skinID;

		public byte ammoIndex;

		public sbyte AnimationReload;

		public sbyte AnimationChamber;

		public sbyte AnimationHolding;

		public sbyte AnimationThrow;

		public sbyte AnimationSpecial1;

		public sbyte AnimationWalk;

		public sbyte AnimationIronSights;

		public sbyte AnimationRun;

		public ushort AnimationFire;

		public ushort AnimationChangeWeapon;

		public sbyte gripType;

		public ushort magazineCapacity;

		public ushort currentRounds;

		public float shootingAccuracy;

		public float scopeViewX;

		public float scopeViewY;

		public float scopeViewZ;

		public float muzzleFlashTimer;

		public float ammoTimer;

		public float fireRate;

		public float accuracyAdjustment;

		public float fireRateAdjustment;

		public float muzzleVelocityAdjustment;

		public float muzzleVelocity;

		public float ammoAccelerationZ;

		public float secsPerBullet;

		public float posX;

		public float posY;

		public float posZ;

		public float curHeat;

		public float[] recoilSide;

		public float[] recoilUp;

		public float[] recoilBack;

		public double firingStart;

		public int shotCount;

		public int tracerCnt;

		public int roundsPerTracer;

		public Coordinate_3D[,] offset;

		public Weapon_Stub_Attachment_Point[] attachments;
	}

	public struct weapon_scope
	{
		public string modelName;

		public byte rangeLow;

		public byte rangeHigh;

		public byte unlockLevel;

		public ushort mount;

		public ushort modID;

		public float adjustmentAccuracy;

		public float adjustmentMovement;

		public float adjustmentTurning;

		public float scopeViewAdjX;

		public float scopeViewAdjY;

		public float scopeViewAdjZ;
	}

	public struct weapon_foregrip
	{
		public byte unlockLevel;

		public ushort mount;

		public ushort modID;

		public float adjustmentAccuracy;

		public float adjustmentMovement;

		public float adjustmentTurning;

		public string modelName;
	}

	public struct weapon_barrel
	{
		public byte unlockLevel;

		public ushort mount;

		public ushort modID;

		public float adjustmentAccuracy;

		public float adjustmentMovement;

		public float adjustmentTurning;

		public float adjustmentMuzzleVelocity;

		public string modelName;
	}

	public struct weapon_energydevice
	{
		public byte unlockLevel;

		public ushort mount;

		public ushort modID;

		public float energy;

		public float adjustmentMovement;

		public float adjustmentTurning;

		public string modelName;
	}

	public struct player_preference
	{
		public byte numWeapons;

		public weapon_preference[] weapons;
	}

	public struct weapon_preference
	{
		public byte weaponID;

		public byte scopeID;

		public byte foreGripID;

		public byte barrelID;

		public byte energyDeviceID;

		public byte skinID;

		public byte tauntID;
	}

	public struct boxList
	{
		public short[] oList;

		public short cnt;

		public short numObjects;
	}

	public class joint
	{
		public bool redoNorms;

		public bool matrixReady;

		public int subIDCount;

		public int curSub;

		public int pSkip1;

		public int pSkip2;

		public short parentID;

		public short parentCount;

		public short pListStart;

		public short numVertices;

		public short numIndexes;

		public short numPrimitives;

		public short numParticles;

		public short[] idList;

		public short[] parentList;

		public float len;

		public float lenSquared;

		public float maxPinH;

		public float pinAngleD;

		public float ringYD;

		public float damageMultiplier;

		public float dirX;

		public float dirY;

		public float dirZ;

		public float radSqr;

		public float radius;

		public float[] angles;

		public float maxAngle;

		public float minAngle;

		public float minPivot;

		public float maxPivot;

		public float maxPivot2;

		public float minPivot2;

		public float curTimeToNextStep;

		public float rotX;

		public float rotZ;

		public float rotY;

		public float targetAngle;

		public float targetPivot2;

		public float targetPivot;

		public float angleSpeed;

		public float pivotSpeed;

		public float pivot2Speed;

		public float angleMoved;

		public float pivotMoved;

		public float pivot2Moved;

		public short adjustStartRing;

		public short adjustEndRing;

		public int rings;

		public int ringPtCnt;

		public float pinOffset;

		public float[] ringPins;

		public float[] particles;

		public vtex[] ringPts;

		public vnorm[] ringNorms;

		public vtex[] tangent;

		public vtex[] bitangent;

		public short status;

		public bool damageJoint;

		public short modID;

		public int texID;

		public float x;

		public float y;

		public float z;

		public float angleX;

		public float angleY;

		public float angleZ;

		public Matrix mvBase;

		public Matrix mvAnimation;

		public Matrix[] mv = new Matrix[2];

		public particle_list apList = default(particle_list);
	}

	public class JointCollection
	{
		public bool usingParticles;

		public byte type;

		public short numJoints;

		public int numJointPoints;

		public int numVertices;

		public int numPrimitives;

		public short[] jStat;

		public float crouchAdjX;

		public float crouchAdjY;

		public float crouchAdjZ;

		public joint[] jt1;

		public connector ct1 = default(connector);

		public VertexBuffer colBuffer;

		public IndexBuffer colIBuffer;

		public Matrix[] InvBindPose;
	}

	public struct joint_basic
	{
		public bool hasModel;

		public byte status;

		public byte type;

		public byte objectType;

		public short modID;

		public int objID;

		public float angleX;

		public float angleY;

		public float angleZ;

		public float maxAngle;

		public float minAngle;

		public float minPivot;

		public float maxPivot;

		public float maxPivot2;

		public float minPivot2;

		public float angle;

		public float pivot;

		public float pivot2;

		public float targetAngle;

		public float targetPivot2;

		public float targetPivot;

		public float angleSpeed;

		public float pivotSpeed;

		public float pivot2Speed;

		public float angleMoved;

		public float pivotMoved;

		public float pivot2Moved;

		public float x;

		public float y;

		public float z;

		public float time;

		public float targetX;

		public float targetY;

		public float targetZ;

		public float xSpeed;

		public float ySpeed;

		public float zSpeed;

		public Matrix[] mv;
	}

	public struct connector
	{
		public long modID;

		public int texID;

		public float angleX;

		public float angleY;

		public float angleZ;

		public float x;

		public float y;

		public float z;

		public Matrix mv;
	}

	public struct program
	{
		public bool loop;

		public bool reverse;

		public bool inReverse;

		public bool resetOnMinorStart;

		public bool staysActiveAtEnd;

		public byte callBackType;

		public byte callBack;

		public byte group;

		public ushort numRotStepsZ;

		public ushort numRotStepsX;

		public ushort numRotStepsY;

		public short numJoints;

		public short numSteps;

		public short curStep;

		public sbyte status;

		public short[] jt;

		public float[] zRot;

		public float[] xRot;

		public float[] yRot;

		public float[] zRotTime;

		public float[] xRotTime;

		public float[] yRotTime;

		public float[] rotXSpeed;

		public float[] rotZSpeed;

		public float[] rotYSpeed;

		public float[] x;

		public float[] y;

		public float[] z;

		public float[] time;

		public float[] xSpeed;

		public float[] ySpeed;

		public float[] zSpeed;

		public float[] x2;

		public float[] y2;

		public float[] z2;

		public float[] time2;

		public float[] xSpeed2;

		public float[] ySpeed2;

		public float[] zSpeed2;
	}

	public struct program_instance
	{
		public bool reverse;

		public bool inReverse;

		public bool staysActiveAtEnd;

		public byte callBackType;

		public short curStep;

		public ushort callBack;

		public ushort timingJoint;

		public sbyte status;

		public float stepTime;
	}

	public struct animation
	{
		public bool loop;

		public bool staysActiveAtEnd;

		public bool networked;

		public byte callBackType;

		public byte callBack;

		public byte group;

		public byte cancelledCallBackType;

		public byte cancelledCallBack;

		public ushort numAnimationSequences;

		public ushort numActions;

		public float directionAndSpeed;

		public float length;

		public animation_sequence[] animation_sequences;

		public animation_action[] actions;
	}

	public struct animation_sequence
	{
		public ushort jointID;

		public ushort numAnimationFrames;

		public animation_rotation_frame[] animation_frames;
	}

	public struct animation_rotation_frame
	{
		public float time;

		public float xRot;

		public float yRot;

		public float zRot;

		public Matrix mv;
	}

	public struct animation_action
	{
		public byte type;

		public byte actionID;

		public sbyte direction;

		public ushort var1;

		public ushort var2;

		public float time;
	}

	public struct animation_instance
	{
		public bool[] actionComplete;

		public bool loop;

		public bool staysActiveAtEnd;

		public byte callBackType;

		public byte callBack;

		public byte group;

		public byte status;

		public byte cancelledCallBackType;

		public byte cancelledCallBack;

		public ushort var1;

		public float directionAndSpeed;

		public float curTime;

		public float scaleFactor;

		public ushort[] curFrames;
	}

	public class program_collection
	{
		public int numPrograms;

		public int currentProgram;

		public int numAnimations;

		public program[] pg1;

		public animation[] animation1;
	}

	public class player
	{
		public bool invincible;

		public bool commanderTargeted;

		public bool makeParticle;

		public bool active;

		public bool falling;

		public bool dead;

		public bool taunting;

		public bool needToReload;

		public bool needToChamber;

		public bool shooting;

		public bool usingTracers;

		public byte renderWeapon;

		public byte curVehicleIndex;

		public byte shotOnce;

		public byte weaponModifier;

		public byte playerLastWeapon;

		public byte numAmmoClips;

		public byte aiID;

		public byte damageType;

		public byte onmap;

		public byte race;

		public byte curBulletHit;

		public byte numAllocatedJoints;

		public byte numJoints;

		public byte humanoidBackJoint;

		public byte headJoint;

		public byte torqueJoint;

		public byte thirdPesonJoint;

		public byte eyeJoint;

		public byte shoulderJointL;

		public byte shoulderJointR;

		public byte weaponJoint;

		public sbyte type;

		public sbyte transporterDirection;

		public sbyte numAvailableWeapons = 8;

		public sbyte numWeapons;

		public sbyte primaryWeaponMountWeapon;

		public sbyte wpnIndex;

		public sbyte jointWasShot;

		public sbyte[] weaponList = new sbyte[8];

		public short playerIsMoving;

		public short id;

		public short lastParticleCount;

		public short inRecoil;

		public short programCollection;

		public short jointPackage;

		public short programJump;

		public short programTurnLeft;

		public short programTurnRight;

		public short programStationaryLegsBody;

		public short programWalk;

		public short programWalkBackwards;

		public short programSidestep;

		public short programRun;

		public short programStationaryArms;

		public short programDeath;

		public short programDeathBlownAway;

		public short[] particles = new short[10];

		public short[] playerModel;

		public ushort curVehicle;

		public ushort team;

		public ushort programSwitchWeapons;

		public ushort[] vehicles = new ushort[3];

		public ushort[] textureID;

		public ulong teamMask;

		public int roundPts;

		public int objectivePoints;

		public int voiceCueID;

		public int respawnParticle;

		public int textureID2;

		public int textureNormalID;

		public int textureSpecularID;

		public int transportParticle;

		public float playerSeparationDistanceSqr;

		public float playerMeleeDistance;

		public float playerBoudingRadius;

		public float playerBoudingRadiusSqr;

		public float playerBoudingRadiusTimes2Sqr;

		public float speakingTimer;

		public float transporter;

		public float xRotation;

		public float yRotation;

		public float zRotation;

		public float velocityTerminal;

		public float velocityTerminalThreshold;

		public float maxDamage;

		public float damage;

		public float projectileResistance;

		public float velX;

		public float velY;

		public float velZ;

		public float impactX;

		public float impactY;

		public float impactZ;

		public float deathFlyBackPercentage;

		public float invincibleTimer;

		public float shootingAccuracy;

		public float shotImpulse;

		public float shotTorque;

		public float deathTimer;

		public float deathTime;

		public float damagePercentage;

		public float damagePercentageCapped;

		public float renderScale;

		public float animationStopTimer;

		public float[] posX = new float[2];

		public float[] posY = new float[2];

		public float[] posZ = new float[2];

		public float[] timeBeforeRespawn = new float[2];

		public float[] laserDist = new float[2];

		public float[,] jColT;

		public float[,] laserPos = new float[2, 3];

		public float[,] laserDir = new float[2, 3];

		public string username;

		public string abreviateName;

		public string password;

		public physics charP = default(physics);

		public particle_list charMain = default(particle_list);

		public weapon weapon1 = new weapon();

		public weapon_stub[] weapon2 = new weapon_stub[10];

		public vtex[] particlePrev;

		public vtex[,] jVect1T;

		public vtex[,] jVect2T;

		public vtex[,] jVect3T;

		public connector ct1 = default(connector);

		public joint[] jt1;

		public Matrix[] mv = new Matrix[2];

		public Ammo_Clips[] ammoClips;

		public program_instance[] pg1;

		public animation_instance[] animations;

		public player()
		{
			for (byte b = 0; b < 10; b++)
			{
				weapon2[b] = default(weapon_stub);
			}
			Initialize_Weapon(ref weapon1);
			Initialize_ParticleList(ref charMain);
			Initialize_Physics(ref charP);
		}
	}

	public class Multiplayer_Data
	{
		public bool delayedPointsSend;

		public bool dataThisRound;

		public ushort specialData;

		public float currentPosX;

		public float currentPosY;

		public float currentPosZ;

		public float springX;

		public float springY;

		public float springZ;

		public float delayedPointsTime;

		public float xRotation;

		public float zRotation;

		public float timeFromLastUpdate;

		public float velX;

		public float velY;

		public float velZ;

		public float rotVelX;

		public float rotVelZ;

		public long lastUpdate;

		public Matrix mv;
	}

	public class Multiplayer_Data_AI
	{
		public bool dataThisRound;

		public ushort specialData;

		public int lastTarget;

		public float currentPosX;

		public float currentPosY;

		public float currentPosZ;

		public float springX;

		public float springY;

		public float springZ;

		public float lastRotX;

		public float lastRotZ;

		public float timeFromLastUpdate;

		public float velX;

		public float velY;

		public float velZ;

		public long lastUpdate;

		public Matrix mv;
	}

	public class Ballistics
	{
		public byte ammoType;

		public byte ammoIndex;

		public byte weaponID;

		public byte barrelID;

		public sbyte tracer;

		public sbyte lightID;

		public short playerID;

		public short soundID;

		public short soundID2;

		public float timer;

		public float particleTimer;

		public float rotation;

		public float[] startX;

		public float[] startY;

		public float[] startZ;

		public float[] endX;

		public float[] endY;

		public float[] endZ;

		public physics phys1;

		public Matrix[] mv;

		public Quaternion rot;
	}

	public class Ammunition
	{
		public bool single;

		public byte type;

		public byte shotCount;

		public byte splash;

		public byte numModels;

		public byte numBreakApartModels;

		public byte particleID;

		public byte particleID2;

		public byte particleID3;

		public string sound;

		public string sound2;

		public short explosionID;

		public short[] modelList;

		public short[] breakApartModelList;

		public float mass;

		public float timer;

		public float releaseTimer;

		public float particleTimer;

		public float muzzleVelocity;

		public float spreadAngle;

		public float accelerationZ;

		public float deceleration;

		public float splashFalloff;

		public float colorIntensity;

		public float length;

		public float radius;

		public float deathFlyBackPercentage;

		public float[] damage = new float[5];

		public float[] lightColor = new float[4];

		public float[] color = new float[4];

		public float[] colorE = new float[4];
	}

	public struct Ammo_Clips
	{
		public byte ammoIndex;

		public byte size;

		public ushort surplus;

		public ushort count;

		public ushort numClips;

		public ushort startingNumClips;

		public ushort maxCanCarry;
	}

	public class particle_effect
	{
		public byte soundID;

		public sbyte type;

		public ushort cullRadius;

		public ushort modID;

		public short refID2;

		public short refID3;

		public int texID;

		public int refID;

		public int lightID = -1;

		public float lifeTime;

		public float rotation;

		public float initialLife;

		public float fadeOutTimer;

		public float[] color;

		public float[] colorChange;

		public float size;

		public float sizeChange;

		public physics phys1 = default(physics);
	}

	public class solid_particle_effect
	{
		public byte soundID;

		public sbyte type;

		public ushort cullRadius;

		public short refID2;

		public short refID3;

		public int texID;

		public int modID;

		public int lightID = -1;

		public float lifeTime;

		public float initialLife;

		public float fadeOutTimer;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float[] color;

		public float[] colorChange;

		public float size;

		public float sizeChange;

		public physics phys1 = default(physics);

		public Quaternion particleRot;
	}

	public struct sort_list
	{
		public short index;

		public int next;

		public int prev;

		public float value;
	}

	public class aiEntity
	{
		public bool canFire;

		public bool authorizedToRespawn;

		public bool locallyControlled;

		public bool needsRoute;

		public bool updateAIRoute;

		public bool active;

		public bool stationary;

		public bool targetVisible;

		public bool targetInRange;

		public bool independentWeaponArm;

		public bool checkForEnemy;

		public bool leadsTarget;

		public bool resetSpeed;

		public byte bossID;

		public byte colDir;

		public byte onmap;

		public byte damageType;

		public byte team;

		public byte enemyTeam;

		public byte state;

		public byte aiType;

		public byte raceType;

		public byte race;

		public byte stepOver;

		public byte patrolling;

		public byte numChildrenAI;

		public byte targetMode;

		public sbyte weaponJoint;

		public sbyte weaponJointR;

		public sbyte weaponJointE;

		public sbyte velocityVariationDirection;

		public sbyte weapon;

		public int textureID;

		public int textureNormalID;

		public int textureSpecularID;

		public short playerID;

		public short targetID;

		public short status;

		public short jointPackage;

		public short programPackage;

		public short controllingPlayer;

		public ushort hostID;

		public ushort[] childrenAI;

		public float x;

		public float y;

		public float z;

		public float xRotation;

		public float zRotation;

		public float fov;

		public float cosFov;

		public float goalDistance;

		public float weaponElevationDir;

		public float speakingTime;

		public float randomFactor;

		public float targetX;

		public float targetY;

		public float targetZ;

		public float lastTargetX;

		public float lastTargetY;

		public float lastTargetZ;

		public float targetCanBeSeenDistance;

		public float maxTargetAngle;

		public float optimalTargetDistanceSqr;

		public float optimalTargetDistance;

		public float lostTargetTimer;

		public float maxTargetMoveDistanceSqr;

		public float goalX;

		public float goalY;

		public float goalZ;

		public float goal2Z;

		public float positionGoalX;

		public float positionGoalY;

		public float positionGoalZ;

		public float firingTime = 1f;

		public float fireTimeRemaining;

		public float timeBetweenFiring;

		public float firingTimeAdj;

		public float firingTimeAdjusted = 1f;

		public float timeBetweenFiringAdjusted;

		public float speed;

		public float speedHover;

		public float speedRotationX;

		public float speedRotationZ;

		public float velocityTerminal;

		public float velocityTerminalThreshold;

		public float velocityVariation;

		public float maxDamage;

		public float damage;

		public float deathTimer;

		public float maxDistanceToHearShotsSqr;

		public float maxDistanceToSeeShotTeammateSqr;

		public Matrix hostMatrix;

		public Route aiRoute;
	}

	public class Pickups
	{
		public bool changed;

		public bool positionChanged;

		public bool enabled;

		public bool startsEnabled;

		public bool startsOnMap;

		public bool onmap;

		public bool willRespawn;

		public bool bool1;

		public bool startWillRespawn;

		public byte type;

		public byte numModels;

		public byte numAllocatedModels;

		public byte numSounds;

		public byte numAllocatedSounds;

		public byte numFloatVars;

		public byte numAllcoatedFloats;

		public long[] modelList;

		public ushort id2;

		public short id;

		public short refID;

		public short actionID;

		public float respawnTime;

		public float timeBeforeRespawn;

		public float soundX;

		public float soundY;

		public float soundZ;

		public float offsetX;

		public float offsetY;

		public float offsetZ;

		public float movementZ;

		public float renderOffsetZ;

		public float movementRotationX;

		public float movementRotationY;

		public float movementRotationZ;

		public float curRotX;

		public float curRotY;

		public float curRotZ;

		public float[] fVar;

		public float[] emissive;

		public string[] sounds;

		public vtex position = new vtex();

		public vtex rotation = new vtex();

		public vtex b1 = new vtex();

		public vtex b2 = new vtex();

		public Matrix mv = default(Matrix);

		public Matrix mv2 = default(Matrix);
	}

	public class SwitchControl
	{
		public bool enabled;

		public bool resetOnMinorStart;

		public bool fixShowSwitch;

		public byte callBackType;

		public byte type;

		public byte numModels;

		public byte state;

		public byte group;

		public byte numSounds;

		public byte numAllocatedSounds;

		public byte numFloatVars;

		public byte numAllcoatedFloats;

		public long[] modelList;

		public short id;

		public short refID;

		public short actionID;

		public float[] fVar;

		public string[] sounds;

		public vtex position = new vtex();

		public vtex rotation = new vtex();

		public vtex b1 = new vtex();

		public vtex b2 = new vtex();

		public Matrix mv = default(Matrix);
	}

	public struct RenderInstance
	{
		public bool useVbo;

		public bool usesCollisionModel;

		public byte type;

		public byte numModels;

		public byte numCollisionModels;

		public byte[] active;

		public byte[] needsBB;

		public byte[] bbType;

		public short numItems;

		public short numObjects;

		public short numAllocatedObjects;

		public short texID;

		public short[] modelList;

		public short[] objList;

		public ushort[] zoneID;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float[] x;

		public float[] y;

		public float[] z;

		public float[] sx;

		public float[] sy;

		public float[] sz;

		public float[] tx;

		public float[] ty;

		public string[] collisionModel;

		public Matrix mv;

		public Vector4 color;
	}

	public struct Object_Collection
	{
		public bool moved;

		public bool active;

		public byte type;

		public ushort numObj;

		public ushort numAllocatedObj;

		public ushort[] objList;

		public float[] objOffsetX;

		public float[] objOffsetY;

		public float[] objOffsetZ;

		public float[] renderOffsetX;

		public float[] renderOffsetY;

		public float[] renderOffsetZ;

		public float x;

		public float y;

		public float z;

		public float movedX;

		public float movedY;

		public float movedZ;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float velX;

		public float velY;

		public float velZ;

		public float frameTime;
	}

	public struct Onscreen_Text
	{
		public float bottomLeftX;

		public float bottomLeftY;

		public float spacingX;

		public float spacingY;

		public float lifeTime;

		public float[] timeRemaining;

		public byte fontID;

		public byte numTextItems;

		public byte scrollDirection;

		public byte curItems;

		public string[] textItems;

		public Color fColor;
	}

	public struct Commander_Item
	{
		public bool active;

		public bool expires;

		public byte type;

		public byte order;

		public byte id;

		public short value;

		public float x;

		public float y;

		public float z;

		public float rotation;

		public float timeToLive;

		public float points;
	}

	public class Basic_Position
	{
		public float x;

		public float y;

		public float z;
	}

	public struct Player_Races
	{
		public byte numTypes;

		public byte numAllocatedTypes;

		public byte numBloodModels;

		public byte numDeathAnimations1;

		public byte numDeathAnimations2;

		public byte numBulletImpactAnimations;

		public byte[] particleEffect;

		public byte[] damageType;

		public byte[] vehicleID;

		public byte[] soundIndex;

		public byte[] jointPackage;

		public byte[] humanoidBackJoint;

		public byte[] torqueJoint;

		public byte[] headJoint;

		public byte[] thirdPesonJoint;

		public byte[] eyeJoint;

		public byte[] shoulderJointL;

		public byte[] shoulderJointR;

		public byte[] weaponJoint;

		public byte[] firstPersonViewJoint1;

		public byte[] deathParticle;

		public byte[] programCollection;

		public byte[] programStationaryLegsBody;

		public byte[] programSwitchWeapons;

		public byte[] programStationaryArms;

		public byte[] programWalk;

		public byte[] programJump;

		public byte[] programTurnLeft;

		public byte[] programTurnRight;

		public byte[] programWalkBackwards;

		public byte[] programSidestep;

		public byte[] programRun;

		public byte[,] programDeath;

		public byte[,] programDeathBlownAway;

		public byte[,] programBulletHit;

		public ushort[] bloodModelIDs;

		public ushort[] miniMapIconID;

		public float bloodSize;

		public float bloodSizeChange;

		public float velocityTerminal;

		public float velocityTerminalThreshold;

		public float[] playerHeight;

		public float[] renderScale;

		public float[] bloodColor;

		public float[] weaponDischarge;

		public float[] whiskers;

		public float[] centerPoint;

		public float[] iconHeight;

		public float[] spawnHeight;

		public float[] playerSeparationDistance;

		public float[] meleeDistance;

		public float[] boundingRadius;

		public float[] mainSoundTimerLength;

		public float[] hurtTimerLength;

		public float[] projectileResistance;

		public float[] gamerTagHeight;

		public float[,] bBox;

		public string name;

		public string teleportInSound;

		public string teleportOutSound;

		public string[] soundDeath;

		public string[] soundHurt;

		public string[] soundMain;

		public string[] jointPackageName;

		public string[] bloodModels;

		public string[] miniMapIcon;
	}

	public struct AI_Boss
	{
		public byte aiID;

		public byte numWeapons;

		public byte curWeapon;

		public byte numPositions;

		public byte curPosition;

		public byte numAllocatedWeapons;

		public byte numAllocatedPositions;

		public byte[] weaponIDs;

		public short textureID;

		public float[] weaponTimers;

		public float[] positionTimers;

		public float[] positionX;

		public float[] positionY;

		public float[] positionZ;

		public float[] accuracy;

		public float weaponTime;

		public float positionTime;
	}

	public struct Route
	{
		public bool routeError;

		public byte curPt;

		public float startX;

		public float startY;

		public float startZ;

		public float endX;

		public float endY;

		public float endZ;

		public ushort numPts;

		public Vector3[] NavMeshRoute;
	}

	public struct Vehicle
	{
		public bool[] driveWheel;

		public byte type;

		public byte numWheels;

		public byte numMounts;

		public byte numModels;

		public byte numWeaponMounts;

		public byte numWeapons;

		public byte maxHeat;

		public byte numAlternateTextures;

		public ushort[] weaponMounts;

		public ushort[] playerIDs;

		public ushort[] alternateTextureIDs;

		public short numColPoints;

		public short[] textureID;

		public short[] vehicleModel;

		public float maxDamage;

		public float throttleSpeed;

		public float wheelTouchingTimer;

		public float wheelTouchingFactor;

		public float xBalanceTimer;

		public float yBalanceTimer;

		public float zBalanceTimer;

		public float balanceFactor;

		public float newVelX;

		public float newVelY;

		public float newVelZ;

		public float vehicleTimer1;

		public float particleTimer;

		public float splashDamageFalloff;

		public float explosionDamage;

		public float explosionImpactForce;

		public float accelerationFactor;

		public float controllerSpring;

		public float controllerDampening;

		public float damageThresholdForExplosion;

		public float damageParticleX;

		public float damageParticleY;

		public float damageParticleZ;

		public float curHeat;

		public float heatGeneration;

		public float heatDissipation;

		public float overHeatingDamage;

		public float[] wheelColPoints;

		public float[] wheelColPointsRadiusAxisZ;

		public float[] wheelRot;

		public float[] maxWheelRot;

		public float[] wheelRotationMultiplier;

		public float[] wheelAttachPoint;

		public float data1;

		public float data2;

		public float data3;

		public float data4;

		public float data5;

		public float data6;

		public float data7;

		public float data8;

		public float data9;

		public float data10;

		public float data11;

		public float data12;

		public float data13;

		public float data14;

		public float data15;

		public float data16;

		public ushort maxOccupants;

		public ushort currentOccupants;

		public float[] colPoints;

		public float Vx;

		public float Vy;

		public float Vz;

		public float velocity;

		public string[] modelName;

		public float startX;

		public float startY;

		public float startZ;

		public float startRotX;

		public float startRotY;

		public float startRotZ;

		public Matrix[] wheelsMatrix;

		public Matrix[] mv;

		public Momentum momentum;

		public physics_new ph1;

		public Model_List mainModel;

		public Mounting_Point_Fixed[] mounts;

		public weapon_stub[] weapons;
	}

	public struct Momentum
	{
		public short numPoints;

		public short countForces;

		public short[] buffimpactForceId;

		public float[] collisionPoints;

		public float[] buffForceValue;

		public float forceX;

		public float forceY;

		public float forceZ;
	}

	public struct Model_List
	{
		public byte numModels;

		public byte numAllocatedModels;

		public byte[] alphaBlend;

		public short id;

		public float[] x;

		public float[] y;

		public float[] z;

		public byte[] bufferID;

		public short[] modelID;

		public string[] modelName;

		public short[] texID;

		public short[] texID2;
	}

	public struct Avatar_Data
	{
		public bool[] looping;

		public bool[] disabled;

		public byte npcAnimationID;

		public byte numAnimations;

		public byte animationLeftArm;

		public byte animationRightArm;

		public byte animationFullBody;

		public byte animationArms;

		public byte animationLegs;

		public byte animationHead;

		public byte animationActive;

		public byte[] animations;

		public byte[] group;

		public byte[] status;

		public sbyte[] animationDirectionControl;

		public float x;

		public float y;

		public float z;

		public float rx;

		public float ry;

		public float rz;

		public float avatarTimeToChange;

		public float npcAvatarTime;

		public float[] animationSpeedControl;

		public float[] animationStopInterval;

		public Avatar_Animation_Change_Request[,] animationChangeRequest;

		public Avatar_Bones_Matrix[] avatarBones;

		public Matrix[] mv;

		public AvatarDescription avatarDescription;

		public AvatarRenderer avatarRenderer;

		public AvatarExpression avatarExpression;

		public AvatarAnimation[] avatarAnimations;
	}

	public struct Avatar_Animation_Change_Request
	{
		public bool needToChangeAnimation;

		public bool changeLoop;

		public bool cancelOtherGroupAnimations;

		public byte changeCategory;

		public byte changeAction;

		public byte changeAnimationID;

		public byte priority;
	}

	public struct Avatar_Bones_Matrix
	{
		public Matrix[] avatarBones;
	}

	public struct Graphs
	{
		public ushort numPoints;

		public ushort numAllocatedPts;

		public byte type;

		public float[] floatVars;

		public int[] intVars;

		public int intMinX;

		public int intMaxX;

		public int intMinY;

		public int intMaxY;

		public int intMinZ;

		public int intMaxZ;

		public float floatMinX;

		public float floatMaxX;

		public float floatMinY;

		public float floatMaxY;

		public float floatMinZ;

		public float floatMaxZ;
	}

	public struct Zone
	{
		public byte type;

		public ushort zoneID;

		public Zone_List zoneList;
	}

	public struct Zone_Check
	{
		public byte type;

		public byte numBoxChecks;

		public byte numAllocatedBoxChecks;

		public byte numSphereChecks;

		public byte numAllocatedSphereChecks;

		public float[] Box;

		public float[] Sphere;

		public ushort objID;

		public ushort objIndex;

		public ushort zoneCheckID;
	}

	public class Zone_List
	{
		public ushort[] oList;

		public ushort[] gidList;

		public ushort numObjects;

		public ushort numAllocatedObjects;

		public Matrix[] matrixList;
	}

	public struct MiniMapItem
	{
		public byte type;

		public byte status;

		public byte startingStatus;

		public byte colorR;

		public byte colorG;

		public byte colorB;

		public byte highlightColorR;

		public byte highlightColorG;

		public byte highlightColorB;

		public byte normalColorR;

		public byte normalColorG;

		public byte normalColorB;

		public ushort texID;

		public ushort gid;

		public float x1;

		public float y1;

		public string texture;
	}

	public struct GameInfo
	{
		public bool commanderMode;

		public byte gameMode;

		public byte gameType;

		public byte numTeams;

		public byte numRounds;

		public byte difficulty;

		public byte floatSize;

		public byte intSize;

		public byte ushortDataSize;

		public byte roundsWon;

		public ushort numPlayers;

		public ushort numAllocatedPlayers;

		public ushort level;

		public float roundTime;

		public float timeRemaining;

		public GameInfoPlayer[] players;
	}

	public struct GameInfoPlayer
	{
		public byte xboxID;

		public float timePlayed;

		public float[] scoresF;

		public int[] scoresI;

		public ushort id;

		public ushort numDeaths;

		public ushort numKills;

		public ushort shotsFired;

		public ushort shotsHit;

		public ushort teamKills;

		public ushort selfKills;

		public ushort[] dataUS;
	}

	public struct CollisionModel
	{
		public byte collisionScheme;

		public string fileName;

		public ushort id;

		public ushort polygonCount;

		public float minX;

		public float minY;

		public float dx;

		public float dy;

		public int numBoxes;

		public int curDiv;

		public Vector3[] v;

		public Vector3[] n;

		public Collision_Model_Box[] cb;
	}

	public struct Target
	{
		public bool active;

		public bool startsActive;

		public bool enabled;

		public bool startsEnabled;

		public bool visible;

		public bool startsVisible;

		public byte callBack;

		public byte callBackType;

		public byte hitAction;

		public byte type;

		public float pointsF;

		public float boxX1;

		public float boxY1;

		public float boxZ1;

		public float boxX2;

		public float boxY2;

		public float boxZ2;

		public float curTime;

		public float startTime;

		public float timeBeforeReset;

		public ushort pointsI;

		public ushort modelID;

		public ushort collisionModelID;

		public ushort programID;

		public string modelName;

		public physics_new ph1;

		public Matrix[] mv;
	}

	public struct Damage_Target
	{
		public bool active;

		public bool startsActive;

		public bool enabled;

		public bool startsEnabled;

		public bool showOnMiniMap;

		public byte callBack;

		public byte callBackType;

		public byte hitAction;

		public byte type;

		public byte team;

		public byte startingTeam;

		public float pointsF;

		public float curDamage;

		public float maxDamage;

		public float repairMultiplier;

		public float curTime;

		public float startTime;

		public float timeBeforeReset;

		public float x;

		public float y;

		public float z;

		public float colorR;

		public float colorG;

		public float colorB;

		public ushort pointsI;

		public ushort programID;

		public ushort miniMapItem;

		public ulong teamMask;

		public string miniMapTexture;
	}

	public struct Object_Position
	{
		public float x1;

		public float y1;

		public float z1;

		public float x2;

		public float y2;

		public float z2;

		public float length;
	}

	public struct Weapon_Modifier
	{
		public byte mask;

		public float amount;

		public float time;
	}

	public struct Weapon_Mount_Player
	{
		public byte type;

		public byte weaponID;

		public byte vehicleID;

		public float posX;

		public float posY;

		public float posZ;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float maxRotX;

		public float maxRotY;

		public float maxRotZ;

		public float minRotX;

		public float minRotY;

		public float minRotZ;

		public float oriRotX;

		public float oriRotY;

		public float oriRotZ;

		public float turretRotFactorX;

		public float turretRotFactorY;

		public float turretRotFactorZ;

		public float playerRotFactorX;

		public float playerRotFactorY;

		public float playerRotFactorZ;

		public float playerPosX;

		public float playerPosY;

		public float playerPosZ;

		public float turretSpeedFactorX;

		public float turretSpeedFactorY;

		public float turretSpeedFactorZ;

		public Matrix mvo;

		public Matrix[] mv;
	}

	public struct Mounting_Point_Fixed
	{
		public byte type;

		public byte objectID;

		public byte objectAttached;

		public byte jointID;

		public byte itemPlacmentMatrixID;

		public Matrix mvStart;

		public Matrix[] mvCurrent;
	}

	public struct UI_Window
	{
		public bool needsUpdating;

		public bool ignoreStickInputs;

		public byte type;

		public byte status;

		public byte state;

		public byte actions;

		public byte curTab;

		public byte startComponent;

		public byte windowCloseFlags;

		public byte returnValue;

		public byte curButton;

		public byte curTextButton;

		public byte curCheckBox;

		public byte curTextArea;

		public byte curStaticGraphic;

		public byte curSlider;

		public byte curLabel;

		public byte curGroup;

		public byte numLabels;

		public byte numButtons;

		public byte numTextButtons;

		public byte numCheckBoxes;

		public byte numSliders;

		public byte numGroups;

		public byte numTextAreas;

		public byte buttonFlags;

		public byte numStaticGraphics;

		public float x;

		public float y;

		public float modScaleX;

		public float modScaleY;

		public float modScaleZ;

		public float autoHideTimer;

		public ushort modID;

		public ushort modTexID;

		public ushort parentID;

		public string soundOpen;

		public string soundClose;

		public string modTexture;

		public UI_Window_Component_Button[] buttons;

		public UI_Window_Component_Text_Button[] textButtons;

		public UI_Window_Component_TextLabel[] labels;

		public UI_Window_Component_Checkbox[] checkBoxes;

		public UI_Window_Component_Slider[] sliders;

		public UI_Window_Component_Group[] groups;

		public UI_Window_Component_Text_Area[] textAreas;

		public UI_Window_Component_Static_Graphic[] staticGraphics;
	}

	public struct UI_Window_Component_Checkbox
	{
		public bool inGroup;

		public byte status;

		public byte value;

		public byte clickGroup;

		public byte clickAction;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort textureID1;

		public ushort textureID2;

		public ushort iconID;

		public string texture1;

		public string texture2;

		public string icon;

		public string soundFocus;

		public string soundClick;

		public float x;

		public float y;

		public float iconX;

		public float iconY;
	}

	public struct UI_Window_Component_Group
	{
		public byte status;

		public byte type;

		public byte numItems;

		public ushort[] items;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort iconID;

		public string icon;

		public string soundFocus;

		public string soundClick;

		public float x;

		public float y;

		public float iconX;

		public float iconY;
	}

	public struct UI_Window_Component_Slider
	{
		public bool inGroup;

		public byte status;

		public byte clickGroup;

		public byte clickAction;

		public float value;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort textureID1;

		public ushort textureID2;

		public ushort iconID;

		public ushort height;

		public string texture1;

		public string texture2;

		public string icon;

		public string soundFocus;

		public string soundChange;

		public float x;

		public float y;

		public float iconX;

		public float iconY;

		public float minX;

		public float maxX;

		public float movementSpeed;
	}

	public struct UI_Window_Component_TextLabel
	{
		public byte status;

		public byte fontID;

		public byte r;

		public byte g;

		public byte b;

		public byte a;

		public byte hr;

		public byte hg;

		public byte hb;

		public byte ha;

		public byte centering;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort iconID;

		public string labelText;

		public string icon;

		public float x;

		public float y;

		public float iconX;

		public float iconY;
	}

	public struct UI_Window_Component_Static_Graphic
	{
		public byte status;

		public ushort id;

		public ushort graphicID;

		public string graphic;

		public float x;

		public float y;
	}

	public struct UI_Window_Component_Text_Area
	{
		public byte status;

		public byte fontID;

		public byte r;

		public byte g;

		public byte b;

		public byte a;

		public byte centering;

		public ushort id;

		public ushort numLines;

		public string[] lines;

		public float x;

		public float y;

		public float fontHeight;
	}

	public struct UI_Window_Component_Button
	{
		public bool inGroup;

		public byte status;

		public byte clickGroup;

		public byte clickAction;

		public byte type;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort iconID;

		public ushort button1ID;

		public ushort button2ID;

		public string soundFocus;

		public string soundClick;

		public string icon;

		public string button1;

		public string button2;

		public float x;

		public float y;

		public float iconX;

		public float iconY;
	}

	public struct UI_Window_Component_Text_Button
	{
		public bool inGroup;

		public byte status;

		public byte fontID;

		public byte r;

		public byte g;

		public byte b;

		public byte a;

		public byte hr;

		public byte hg;

		public byte hb;

		public byte ha;

		public byte clickGroup;

		public byte clickAction;

		public byte type;

		public byte centering;

		public ushort id;

		public ushort componentDown;

		public ushort componentUp;

		public ushort componentLeft;

		public ushort componentRight;

		public ushort iconID;

		public string buttonText;

		public string soundFocus;

		public string soundClick;

		public string icon;

		public float x;

		public float y;

		public float iconX;

		public float iconY;
	}

	public struct UI_Window_List
	{
		public byte numWindows;

		public byte[] windows;
	}

	public struct Game_Object
	{
		public bool startsActive;

		public bool usesPhysics;

		public bool doPhysics;

		public bool isTarget;

		public byte type;

		public byte numCollisionModels;

		public byte numParticleModels;

		public byte numModels;

		public byte damageType;

		public byte state;

		public sbyte explosionID;

		public int points;

		public ushort ID;

		public ushort gid;

		public ushort numPts;

		public ushort targetID;

		public ushort modelListID;

		public ushort collisionModelListID;

		public ushort destroyedParticleID;

		public ushort objectDroppedOnDestruction;

		public ushort[] modID;

		public ushort[] colModels;

		public ushort[] colModelZones;

		public ushort[] particleModels;

		public float x;

		public float y;

		public float z;

		public float rotX;

		public float rotY;

		public float rotZ;

		public float boundingRadius;

		public float colorR;

		public float colorG;

		public float colorB;

		public float colorA;

		public float scaleX;

		public float scaleY;

		public float scaleZ;

		public float curDamage;

		public float maxDamage;

		public float particleTimer;

		public float damageBarHeight;

		public float distanceFromExplosion;

		public float[] collisionPoints;

		public physics_new phy;

		public Quaternion qn1;

		public Matrix mv1;

		public Matrix mvStart;

		public string snd_Destroyed;

		public string snd_Repaired;
	}

	public struct Collision_Model_Box
	{
		public byte type;

		public byte status;

		public ushort id;

		public ushort[] ids;

		public ushort numIDs;

		public float x;

		public float y;

		public float z;

		public float x2;

		public float y2;

		public float z2;
	}

	public struct Coordinate_3D
	{
		public float x;

		public float y;

		public float z;
	}

	public struct Game_Reward
	{
		public byte status;

		public float rewardTimer;

		public float rewardLength;

		public string activationSound;
	}

	public struct Game_Perk
	{
		public byte type;

		public byte category;

		public float multiplier;

		public ushort minRankToUse;

		public string perkName;
	}

	public struct Ballistic_Strike
	{
		public byte status;

		public byte ammoIndex;

		public byte variance;

		public ushort playerID;

		public float x;

		public float y;

		public float z;

		public float vx;

		public float vy;

		public float vz;

		public float radius;

		public float duration;

		public float timeBetweenAmmo;

		public float curTime;

		public float curFiringTime;
	}

	public struct Explosion
	{
		public float blastRadius;

		public float duration;

		public float cameraShakeX;

		public float cameraShakeY;

		public float cameraShakeZ;

		public float cameraShakeVariance;

		public float splashFalloff;

		public float impactForce;

		public ushort particleID;

		public string sound;

		public float[] damage;
	}

	public struct Explosion_Occurance
	{
		public byte status;

		public ushort playerID;

		public ushort explosionID;

		public float x;

		public float y;

		public float z;

		public float curTime;
	}

	public struct Hit_Indicator
	{
		public ushort originX;

		public ushort originY;

		public ushort radius;

		public float starTime;
	}

	public struct Hit_Indicator_Instance
	{
		public float rotation;

		public float curTime;
	}

	public struct Particle_Emitter
	{
		public byte particleID;

		public float length;

		public float particlesPerSecond;
	}

	public struct Particle_Emitter_Instance
	{
		public byte status;

		public byte particleID;

		public float curTime;

		public float length;

		public float particlesPerSecond;

		public float x;

		public float y;

		public float z;

		public float nx;

		public float ny;

		public float nz;

		public float vx;

		public float vy;

		public float vz;

		public ushort count;
	}

	public struct Particle_Type
	{
		public ParticleVertex[] MS_Particles;

		public DynamicVertexBuffer newparticleVertexBuffer;

		public IndexBuffer newparticleIndexBuffer;

		public byte effectType;

		public byte numParticles;

		public sbyte particleEmitter;

		public int firstActiveParticle;

		public int firstNewParticle;

		public int firstFreeParticle;

		public int firstRetiredParticle;

		public int drawCounter;

		public float currentTime;

		public float curRotation;

		public float rotation;

		public float particleDuration;

		public float durationRandomFactor;

		public float velocityScaleFactor;

		public float startSizeMin;

		public float startSizeMax;

		public float endSizeMin;

		public float endSizeMax;

		public float frameCount;

		public float invFrameCount;

		public float rotateSpeedMin;

		public float rotateSpeedMax;

		public float positionVariance;

		public float positionVarianceHalf;

		public float velocityVariance;

		public float velocity;

		public float angleOffSetMin;

		public float range;

		public ushort maxParticles;

		public ushort textureID;

		public string texture;
	}

	public struct AI_Path
	{
		public byte direction;

		public int startBox;

		public int endBox;

		public int numPaths;

		public int nextPath;

		public int[] paths;

		public float startX;

		public float startY;

		public float startZ;

		public float endX;

		public float endY;

		public float endZ;

		public float length;

		public float vx;

		public float vy;

		public float vz;

		public float upx;

		public float upy;

		public float upz;
	}

	public struct AI_Route_Box
	{
		public byte numPaths;

		public ushort[] pathList;
	}

	public struct Muzzle_Flash
	{
		public byte textureIndex;

		public float x;

		public float y;

		public float z;

		public float timeRemaining;

		public float fadeoutTime;
	}

	public struct Network_Player
	{
		public bool playerLoaded;

		public bool haveAllRemotePlayerDataForStart;

		public bool haveRemotePlayerArrayPosition;

		public bool haveRemotePlayerTeam;

		public bool haveRemotePlayerPosition;

		public bool haveRemotePlayerStatus;

		public short playerArrayPosition;

		public GamerProfile profile;

		public Texture2D gamerPicture;
	}

	public struct Spawn_Point
	{
		public bool active;

		public bool startsActive;

		public ushort teamMask;

		public float x;

		public float y;

		public float z;

		public float rotation;
	}

	public static readonly VertexElement[] VertexElements = new VertexElement[7]
	{
		new VertexElement(0, VertexElementFormat.Color, VertexElementUsage.Color, 0),
		new VertexElement(4, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
		new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
		new VertexElement(28, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
		new VertexElement(40, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
		new VertexElement(48, VertexElementFormat.Byte4, VertexElementUsage.BlendIndices, 0),
		new VertexElement(52, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0)
	};

	public static long roundf(float x)
	{
		return (long)((x > 0f) ? (x + 0.5f) : (x - 0.5f));
	}

	public static long roundd(double x)
	{
		return (long)((x > 0.0) ? (x + 0.5) : (x - 0.5));
	}

	public static float roundff(float x)
	{
		return (long)((x > 0f) ? (x + 0.5f) : (x - 0.5f));
	}

	public static double rounddd(double x)
	{
		return (long)((x > 0.0) ? (x + 0.5) : (x - 0.5));
	}

	public static byte roundfb(float x)
	{
		return (byte)((x > 0f) ? (x + 0.5f) : (x - 0.5f));
	}

	public static void Initialize_Ballistics(ref Ballistics a1)
	{
		a1.startX = new float[2];
		a1.startY = new float[2];
		a1.startZ = new float[2];
		a1.endX = new float[2];
		a1.endY = new float[2];
		a1.endZ = new float[2];
		a1.mv = new Matrix[2];
		a1.phys1 = default(physics);
		Initialize_Physics(ref a1.phys1);
	}

	public static void Initialize_Face(ref face face1)
	{
		face1.v1 = new vtex[4];
		face1.n1 = new vnorm[4];
		face1.tangent = new vtex();
		face1.bitangent = new vtex();
	}

	public static void Initialize_GameObject(ref gameobject g1)
	{
		g1.color = new float[4];
		Initialize_Physics(ref g1.phys1);
		g1.pList = default(particle_list_byte);
		Initialize_ParticleList_Byte(ref g1.pList);
	}

	public static void Initialize_Model(ref model m1)
	{
		m1.defaultColor = new float[4];
		m1.vcount = 0;
		m1.ncount = 0;
		m1.tcount = 0;
		m1.pcount = 0;
		m1.instanceCount = 0;
		m1.numObjects = 0;
		m1.triangleCount = 0;
		m1.vertexCount = 0;
		m1.inLevelVBO = false;
	}

	public static void Initialize_Collision_Model(ref CollisionModel m1)
	{
		m1.polygonCount = 0;
		m1.collisionScheme = 1;
		m1.numBoxes = 0;
	}

	public static void Initialize_ParticleEffect(particle_effect p1)
	{
		p1.color = new float[4];
		p1.colorChange = new float[4];
		Initialize_Physics(ref p1.phys1);
	}

	public static void Initialize_Solid_ParticleEffect(solid_particle_effect p1)
	{
		p1.color = new float[4];
		p1.colorChange = new float[4];
		Initialize_Physics(ref p1.phys1);
	}

	public static void Initialize_ParticleList(ref particle_list p1)
	{
		p1.b1 = new vtex();
		p1.b2 = new vtex();
		p1.pos1 = new vtex();
		p1.pos2 = new vtex();
	}

	public static void Initialize_ParticleList_Byte(ref particle_list_byte p1)
	{
		p1.b1 = new vtex();
		p1.b2 = new vtex();
		p1.b3 = new vtex();
		p1.b4 = new vtex();
		p1.pos1 = new vtex();
		p1.pos2 = new vtex();
	}

	public static void Initialize_Physics(ref physics p1)
	{
		p1.acceleration = new vtex();
		p1.angle = new vtex();
		p1.position = new vtex();
		p1.velocity = new vtex();
		p1.angularVelocity = new vtex();
		p1.angularAcceleration = new vtex();
		p1.velocity.v[0] = 0f;
		p1.velocity.v[1] = 0f;
		p1.velocity.v[2] = 0f;
		p1.angularVelocity.v[0] = 0f;
		p1.angularVelocity.v[1] = 0f;
		p1.angularVelocity.v[2] = 0f;
		p1.acceleration.v[0] = 0f;
		p1.acceleration.v[1] = 0f;
		p1.acceleration.v[2] = 0f;
		p1.angularAcceleration.v[0] = 0f;
		p1.angularAcceleration.v[1] = 0f;
		p1.angularAcceleration.v[2] = 0f;
	}

	public static void Initialize_Poly(ref poly p1)
	{
		p1.v = new int[3];
		p1.n = new int[3];
		p1.t = new int[3];
		p1.tangent = new vtex();
		p1.bitangent = new vtex();
	}

	public static void Initialize_Weapon(ref weapon weapon1)
	{
		Initialize_ParticleList(ref weapon1.box);
		weapon1.numBarrels = 1;
		weapon1.offset = new vtex[1, 10];
		for (int i = 0; i < 10; i++)
		{
			weapon1.offset[0, i] = new vtex();
		}
		Initialize_Physics(ref weapon1.weaponLoc);
	}

	public static void Initialize_Physics_New(ref physics_new phy)
	{
		phy.mass = 1f;
		phy.momentInertiaAxisX = 1f;
		phy.momentInertiaAxisY = 1f;
		phy.momentInertiaAxisZ = 1f;
		Reset_Physics_New(ref phy);
	}

	public static void Reset_Physics_New(ref physics_new phy)
	{
		phy.accelerationX = 0f;
		phy.accelerationY = 0f;
		phy.accelerationZ = 0f;
		phy.angularAccerlationX = 0f;
		phy.angularAccerlationY = 0f;
		phy.angularAccerlationZ = 0f;
		phy.velocityX = 0f;
		phy.velocityY = 0f;
		phy.velocityZ = 0f;
		phy.angularVelocityX = 0f;
		phy.angularVelocityY = 0f;
		phy.angularVelocityZ = 0f;
		phy.forceX = 0f;
		phy.forceY = 0f;
		phy.forceZ = 0f;
		phy.torqueX = 0f;
		phy.torqueY = 0f;
		phy.torqueZ = 0f;
		phy.initialTime = 0.0;
		phy.velocity = 0f;
	}

	public static void Initialize_Weapon_Stub(ref weapon_stub stub)
	{
		stub.weaponID = 0;
		stub.fired = false;
		stub.fullyAutomatic = true;
		stub.shooting = false;
		stub.triggerPulled = false;
		stub.roundChambered = true;
		stub.singleShot = false;
		stub.scopeID = 0;
		stub.scopeLow = 0;
		stub.scopeHigh = 0;
		stub.gripType = 0;
		stub.fireRate = 0f;
		stub.secsPerBullet = 0.0001f;
		stub.firingStart = 0.0;
		stub.tracerCnt = 0;
		stub.roundsPerTracer = 10;
		stub.crossHair = 0;
		stub.fireRateAdjustment = 1f;
		stub.accuracyAdjustment = 1f;
		stub.muzzleVelocityAdjustment = 1f;
		stub.shootingAccuracy = 1f;
		stub.magazineCapacity = 10;
		stub.currentRounds = 0;
		stub.curClip = byte.MaxValue;
		stub.ammoIndex = 0;
		stub.shotCount = 1;
		stub.ammoAccelerationZ = 0f;
		stub.muzzleVelocity = 1f;
		stub.recoilUp = new float[2];
		stub.recoilSide = new float[3];
		stub.recoilBack = new float[2];
		stub.active = false;
		stub.numAttachments = 0;
		stub.offset = new Coordinate_3D[1, 10];
		for (byte b = 0; b < 10; b++)
		{
			stub.offset[0, b] = default(Coordinate_3D);
		}
	}
}
