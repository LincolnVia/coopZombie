@echo off
setlocal EnableExtensions DisableDelayedExpansion

rem Reproduce the setup and build stages documented in PORTING.md.
rem Run from anywhere; all paths are resolved relative to this file.
pushd "%~dp0" || exit /b 1

set "ILSPY_VERSION=9.1.0.7988"
set "DOTNET_VERSION=8.0.301"
set "FNALIBS_URL=https://nightly.link/FNA-XNA/fnalibs-dailies/workflows/ci.yml/main/fnalibs.zip"
set "GAME_URL=https://archive.org/download/game_20260814/game"
set "GAME_POS=OriginalDump\game"
set "DOWNLOADS=%CD%\tools\downloads"
set "FNALIBS_ZIP=%DOWNLOADS%\fnalibs.zip"
set "PORT_PROJECT=%CD%\Port\TheCoOpZombieGame.csproj"
set "GAME_EXE=%CD%\Extracted\584E07D1\TheCoOpZombieGame.exe"
set "GAME_CONTENT=%CD%\Extracted\584E07D1\The_CoOp_Zombie_Game"


echo.
echo === Co-Op Zombie port setup and build ===

where git.exe >nul 2>nul
if errorlevel 1 (
    echo ERROR: Git is required. Install Git for Windows and run this file again.
    goto :fail
)

where curl.exe >nul 2>nul
if errorlevel 1 (
    echo ERROR: curl.exe is required. It is included with current Windows 10 and 11.
    goto :fail
)

where tar.exe >nul 2>nul
if errorlevel 1 (
    echo ERROR: tar.exe is required. It is included with current Windows 10 and 11.
    goto :fail
)

if not exist "%DOWNLOADS%" mkdir "%DOWNLOADS%"

call :select_dotnet
if errorlevel 1 goto :fail

echo.
curl.exe --fail --location --retry 3 --output "%GAME_POS%" "%GAME_URL%"




echo [1/9] Downloading repositories declared in .gitmodules...
if not exist ".gitmodules" (
    echo ERROR: %CD%\.gitmodules was not found.
    goto :fail
)
for /f "tokens=1,*" %%A in ('git config -f .gitmodules --get-regexp "^submodule\..*\.path$"') do (
    call :ensure_module "%%A" "%%B"
    if errorlevel 1 goto :fail
)
call :ensure_repo "https://github.com/hedge-dev/XenosRecomp.git" "tools\CSO_Processor\XenosRecomp"
if errorlevel 1 goto :fail

echo.
echo [2/9] Installing ILSpy command-line tool %ILSPY_VERSION%...
if not exist "tools\ilspy\ilspycmd.exe" (
    rem NuGet.Config intentionally clears public feeds for the game build, so
    rem give the standalone ILSpy tool its official feed explicitly.
    dotnet tool install ilspycmd --tool-path "tools\ilspy" --version "%ILSPY_VERSION%" --add-source "https://api.nuget.org/v3/index.json"
    if errorlevel 1 goto :fail
) else (
    echo ILSpy is already present.
)

echo.
echo [3/9] Downloading FNA native libraries...
if exist "tools\fnalibs\x64\SDL3.dll" if exist "tools\fnalibs\x64\FNA3D.dll" if exist "tools\fnalibs\x64\FAudio.dll" if exist "tools\fnalibs\x64\libtheorafile.dll" goto :fnalibs_ready

if exist "%FNALIBS_ZIP%" (
    tar -tf "%FNALIBS_ZIP%" >nul 2>nul
    if errorlevel 1 del /q "%FNALIBS_ZIP%"
)
if not exist "%FNALIBS_ZIP%" (
    echo Downloading the latest official fnalibs build through nightly.link...
    curl.exe --fail --location --retry 3 --output "%FNALIBS_ZIP%" "%FNALIBS_URL%"
    if errorlevel 1 goto :fail
)
if not exist "tools\fnalibs" mkdir "tools\fnalibs"
tar -xf "%FNALIBS_ZIP%" -C "tools\fnalibs"
if errorlevel 1 goto :fail

:fnalibs_ready
for %%F in (SDL3.dll FNA3D.dll FAudio.dll libtheorafile.dll) do (
    if not exist "tools\fnalibs\x64\%%F" (
        echo ERROR: tools\fnalibs\x64\%%F is missing after extraction.
        goto :fail
    )
)
echo FNA native libraries are ready.

echo.
echo [4/9] Locating the Windows SDK HLSL compiler...
call :find_fxc
if not defined FXC (
    where winget.exe >nul 2>nul
    if errorlevel 1 (
        echo ERROR: fxc.exe was not found and WinGet is unavailable.
        echo Install the Windows 11 SDK 10.0.26100, then run this file again.
        goto :fail
    )
    echo Installing Windows SDK 10.0.26100. This may request elevation...
    winget install --exact --id Microsoft.WindowsSDK.10.0.26100 --source winget --accept-package-agreements --accept-source-agreements
    if errorlevel 1 goto :fail
    call :find_fxc
)
if not defined FXC (
    echo ERROR: The Windows SDK was installed, but its x64 fxc.exe was not found.
    goto :fail
)
echo Using "%FXC%"

echo.
echo [5/9] Extracting the STFS package...
if not exist "OriginalDump\game" (
    echo ERROR: OriginalDump\game is missing. Supply your legally obtained dump first.
    goto :fail
)
if not exist "%GAME_EXE%" (
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "tools\Extract-STFS\Extract-STFS.ps1" -Path "OriginalDump\game" -ListOnly
    if errorlevel 1 goto :fail
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "tools\Extract-STFS\Extract-STFS.ps1" -Path "OriginalDump\game" -OutputDir "Extracted"
    if errorlevel 1 goto :fail
) else (
    echo Extracted game executable is already present; skipping package extraction.
)

echo.
echo [6/9] Reconstructing the managed project with ILSpy...
if not exist "Decompiled" (
    "tools\ilspy\ilspycmd.exe" -p -o "Decompiled" --nested-directories "%GAME_EXE%"
    if errorlevel 1 goto :fail
) else (
    echo Decompiled directory is already present; leaving it unchanged.
)

echo.
echo [7/9] Extracting and compiling the compatibility effect...
dotnet restore "tools\XnbEffectExtractor\XnbEffectExtractor.csproj" --configfile "NuGet.Config"
if errorlevel 1 goto :fail
dotnet build "tools\XnbEffectExtractor\XnbEffectExtractor.csproj" --no-restore
if errorlevel 1 goto :fail
"tools\XnbEffectExtractor\bin\Debug\net8.0\XnbEffectExtractor.exe" "%GAME_CONTENT%\Effect_Main.xnb" "Port\ContentSource\Effect_Main.cso"
if errorlevel 1 goto :fail
"%FXC%" /nologo /T fx_2_0 /Fo "Port\ContentSource\Effect_Main.fxb" "Port\ContentSource\Effect_Main.compat.fx"
if errorlevel 1 goto :fail

echo.
echo [8/9] Restoring the port...
dotnet restore "%PORT_PROJECT%" --configfile "NuGet.Config"
if errorlevel 1 goto :fail

echo.
echo [9/9] Building the port...
dotnet build "%PORT_PROJECT%" --no-restore
if errorlevel 1 goto :fail

echo.
echo Build completed successfully.
echo Output: %CD%\Port\bin\Debug\net8.0\TheCoOpZombieGame.exe
popd
exit /b 0

:select_dotnet
set "USE_LOCAL_DOTNET="
if exist ".dotnet-cli\dotnet.exe" set "USE_LOCAL_DOTNET=1"
if not defined USE_LOCAL_DOTNET (
    where dotnet.exe >nul 2>nul
    if not errorlevel 1 (
        dotnet --list-sdks 2>nul | findstr /b /c:"%DOTNET_VERSION% " >nul
        if not errorlevel 1 exit /b 0
    )
    echo .NET SDK %DOTNET_VERSION% is not installed; downloading a repository-local copy...
    curl.exe --fail --location --retry 3 --output "%DOWNLOADS%\dotnet-install.ps1" "https://dot.net/v1/dotnet-install.ps1"
    if errorlevel 1 exit /b 1
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%DOWNLOADS%\dotnet-install.ps1" -Version "%DOTNET_VERSION%" -InstallDir "%CD%\.dotnet-cli" -NoPath
    if errorlevel 1 exit /b 1
    set "USE_LOCAL_DOTNET=1"
)
if defined USE_LOCAL_DOTNET (
    set "DOTNET_ROOT=%CD%\.dotnet-cli"
    set "PATH=%CD%\.dotnet-cli;%PATH%"
)
dotnet --version
exit /b %ERRORLEVEL%

:ensure_module
setlocal
set "MODULE_KEY=%~1"
set "MODULE_PATH=%~2"
set "URL_KEY=%MODULE_KEY:.path=.url%"
set "MODULE_URL="
for /f "delims=" %%U in ('git config -f .gitmodules --get "%URL_KEY%"') do set "MODULE_URL=%%U"
if not defined MODULE_URL (
    echo ERROR: No URL is configured for %MODULE_PATH% in .gitmodules.
    endlocal & exit /b 1
)
if exist "%MODULE_PATH%\.git" (
    echo Reusing %MODULE_PATH%
    git -C "%MODULE_PATH%" submodule update --init --recursive
    endlocal & exit /b %ERRORLEVEL%
)
if exist "%MODULE_PATH%" (
    dir /b "%MODULE_PATH%" 2>nul | findstr . >nul
    if not errorlevel 1 (
        echo ERROR: %MODULE_PATH% exists, is non-empty, and is not a Git checkout.
        endlocal & exit /b 1
    )
)
echo Cloning %MODULE_URL% to %MODULE_PATH%...
git clone --recursive "%MODULE_URL%" "%MODULE_PATH%"
endlocal & exit /b %ERRORLEVEL%

:ensure_repo
setlocal
set "REPO_URL=%~1"
set "REPO_PATH=%~2"
if exist "%REPO_PATH%\.git" (
    echo Reusing %REPO_PATH%
    git -C "%REPO_PATH%" submodule update --init --recursive
    endlocal & exit /b %ERRORLEVEL%
)
if exist "%REPO_PATH%" (
    dir /b "%REPO_PATH%" 2>nul | findstr . >nul
    if not errorlevel 1 (
        echo ERROR: %REPO_PATH% exists, is non-empty, and is not a Git checkout.
        endlocal & exit /b 1
    )
)
for %%D in ("%REPO_PATH%") do if not exist "%%~dpD" mkdir "%%~dpD"
echo Cloning %REPO_URL% to %REPO_PATH%...
git clone --recursive "%REPO_URL%" "%REPO_PATH%"
endlocal & exit /b %ERRORLEVEL%

:find_fxc
set "FXC="
for /d %%V in ("%ProgramFiles(x86)%\Windows Kits\10\bin\*") do if exist "%%~fV\x64\fxc.exe" set "FXC=%%~fV\x64\fxc.exe"
exit /b 0

:fail
set "FAIL_CODE=%ERRORLEVEL%"
if "%FAIL_CODE%"=="0" set "FAIL_CODE=1"
echo.
echo Setup/build failed with exit code %FAIL_CODE%.
popd
exit /b %FAIL_CODE%
