@echo off

set SERVER=-S localhost
set DB_NAME=SiteBase
set USER=-U web
set PASS=-P Password1
set NO_PAUSE=
set INSERT_POSTAL_CODES=

:setargs
if "%1"=="" goto doneargs
if "%1"=="/nopause" SET NO_PAUSE=nopause
if "%1"=="/initpostalcodes" SET INSERT_POSTAL_CODES=true
SHIFT
goto setargs

:doneargs

sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i truncate-aspnet-membership.sql
sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i drop-all-tables.sql
sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i common-drop-all-tables.sql
sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i common-data-model.sql
sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i common-seed-data.sql
sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i test-data.sql
if defined INSERT_POSTAL_CODES sqlcmd %SERVER% -d %DB_NAME% %USER% %PASS% -i postal-codes.sql

echo Done

if not "%NO_PAUSE%"=="nopause" pause