SET Configuration=Release
SET BuildTarget=Rebuild
SET NoPause=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/release" SET Configuration=Release
if "%1"=="/clean" SET BuildTarget=Clean
if "%1"=="/nopause" SET NoPause=nopause
SHIFT
goto setargs

:doneargs

3rdParty\Nuget\Bin\NuGet.exe restore DigitalBeacon.sln

setlocal enabledelayedexpansion
set VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
for /f "usebackq tokens=*" %%i in (`"!VSWHERE!" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
    set "MSBUILD_EXE=%%i"
    goto :found_msbuild
)

:found_msbuild
if defined MSBUILD_EXE (
    echo MSBuild found at: !MSBUILD_EXE!
) else (
    echo MSBuild not found.
    goto exitscript
)

"!MSBUILD_EXE!" DigitalBeacon.sln /m /t:%BuildTarget% /p:Configuration=%Configuration%

:exitscript

if not "%NoPause%"=="nopause" pause