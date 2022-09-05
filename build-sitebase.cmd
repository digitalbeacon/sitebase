SET Configuration=Release
SET BuildTarget=Rebuild
SET NoPause=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/release" SET Configuration=Release
if "%1"=="/debug" SET Configuration=Debug
if "%1"=="/clean" SET BuildTarget=Clean
if "%1"=="/nopause" SET NoPause=nopause
SHIFT
goto setargs

:doneargs

3rdParty\Nuget\Bin\NuGet.exe restore DigitalBeacon.SiteBase.sln

SET MsBuildPath=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild
if "%PROCESSOR_ARCHITECTURE%"=="AMD64" SET MsBuildPath=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\msbuild
echo Using MSBuild path: %MsBuildPath%

%MsBuildPath% DigitalBeacon.SiteBase.sln /m /t:%BuildTarget% /p:Configuration=%Configuration%

if not "%NoPause%"=="nopause" pause