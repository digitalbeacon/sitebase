@echo off

SET Configuration=Release
SET BuildTarget=_CopyWebApplication
SET NoPause=
SET Version=

for %%a in (.) do set Project=%%~na

SETLOCAL EnableDelayedExpansion
FOR /F "tokens=2" %%i IN (CommonAssemblyInfo.cs) DO (
	SET token=%%i
	SET token=!token:")]=!
	IF "!token:~0,19!"=="AssemblyFileVersion" SET Version=-!token:~21!
)

:setargs
if "%1"=="" goto doneargs
if "%1"=="/release" SET Configuration=Release
if "%1"=="/debug" SET Configuration=Debug
if "%1"=="/clean" SET BuildTarget=Clean
if "%1"=="/compress" SET BuildTarget=CompressWebAssets
if "%1"=="/nopause" SET NoPause=nopause
if "%1"=="/version" (
	SET Version=-%2
	SHIFT
)
if "%1"=="/project" (
	SET Project=%2
	SHIFT
)
SHIFT
goto setargs

:doneargs

rmdir /s /q Publish

for %%i in (SiteBase\Site\*.csproj) do (
	%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\msbuild %%i /t:%BuildTarget% /p:Configuration=%Configuration%;WebProjectOutputDir=%cd%\Publish\Site\;OutDir=%cd%\Publish\Site\Bin\
)

copy /y SiteBase\Site\Bin\*.dll Publish\Site\Bin
copy /y SiteBase\Site\Bin\*.pdb Publish\Site\Bin

xcopy /Y /E /I SiteBase\Site\Resources\Base\telerik Publish\Site\Resources\Base\telerik

mkdir Publish\Config\Config
xcopy /Y SiteBase\Site\web.config Publish\Config
xcopy SiteBase\Config\*.config Publish\Config\Config
if exist Publish\Config\Config\web.config del Publish\Config\Config\web.config

if exist "%ProgramFiles%\7-Zip\7z.exe" set zipexe=%ProgramFiles%\7-Zip\7z.exe
if not defined zipexe if exist "%PROGRAMW6432%\7-Zip\7z.exe" set zipexe=%PROGRAMW6432%\7-Zip\7z.exe

if defined zipexe (
	cd Publish\Site
	"%zipexe%" a -r -xr^^!.git* ..\%Project%%Version%.zip *
	cd ..\Config
	"%zipexe%" a -r -xr^^!.git* ..\Config%Version%.zip *
	cd ..\..
)

xcopy Publish\Config\* Publish\Site /E
rmdir /s /q Publish\Config

cd Publish\Site
del /S /Q .git*
cd ..\..

if not defined zipexe (
	echo.
	echo Could not find 7-zip.
	echo.
)

echo.
echo Deployment files are located in the Publish folder.
echo.

if not "%NoPause%"=="nopause" pause