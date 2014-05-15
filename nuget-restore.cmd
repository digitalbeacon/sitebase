@echo off

SET _NoPause=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/nopause" SET _NoPause=nopause
SHIFT
goto setargs

:doneargs

for %%i in (*.sln) do (
	3rdParty\Nuget\Bin\NuGet.exe restore %%i
)

if not "%_NoPause%"=="nopause" pause