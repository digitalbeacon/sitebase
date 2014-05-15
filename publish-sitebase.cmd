SET Configuration=Release
SET BuildTarget=ResolveReferences;_CopyWebApplication
SET NoPause=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/release" SET Configuration=Release
if "%1"=="/debug" SET Configuration=Debug
if "%1"=="/clean" SET BuildTarget=Clean
if "%1"=="/compress" SET BuildTarget=CompressWebAssets
if "%1"=="/nopause" SET NoPause=nopause
SHIFT
goto setargs

:doneargs

rmdir /s /q SiteBase\Publish\Site

%SystemRoot%\Microsoft.NET\Framework\v3.5\msbuild SiteBase\Site\DigitalBeacon.SiteBase.csproj /t:%BuildTarget% "/p:Configuration=%Configuration%;WebProjectOutputDir=%cd%\SiteBase\Publish\Site\;OutDir=%cd%\SiteBase\Publish\Site\Bin\;BuildingProject=true"

del /q SiteBase\Publish\Site\web.config

mkdir SiteBase\Publish\Site\Temp

move SiteBase\Publish\Site\Bin\DigitalBeacon*.dll SiteBase\Publish\Site\Temp

del /q SiteBase\Publish\Site\Bin\*.*

move SiteBase\Publish\Site\Temp\*.* SiteBase\Publish\Site\Bin

rmdir /s /q SiteBase\Publish\Site\Temp

if not "%NoPause%"=="nopause" pause