@echo off

set SERVER=-S localhost
set DB_NAME=SiteBase
set USER=-E
set NO_PAUSE=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/nopause" SET NO_PAUSE=nopause
if "%1"=="/user" (
	SET USER=-U %2
	SHIFT
)
SHIFT
goto setargs

:doneargs

sqlcmd %SERVER% %USER% -Q "CREATE DATABASE %DB_NAME%"
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\aspnet_regsql.exe %USER% -A m %SERVER% -d %DB_NAME%
sqlcmd %SERVER% -d %DB_NAME% %USER% -i create-db-user.sql

echo Done

if not "%NO_PAUSE%"=="nopause" pause