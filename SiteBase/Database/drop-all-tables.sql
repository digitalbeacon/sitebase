IF DB_NAME() IN ('master', 'msdb', 'model', 'distribution')
BEGIN
RAISERROR('Not for use on system databases', 16, 1)
GOTO Done
END

SET NOCOUNT ON

DECLARE @DropStatement nvarchar(4000)
DECLARE @SequenceNumber int
DECLARE @LastError int
DECLARE @TablesDropped int
DECLARE @Schema nvarchar(100)

SET @Schema = 'web'

DECLARE DropStatements CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
--views
SELECT
1 AS SequenceNumber,
N'DROP VIEW ' +
QUOTENAME(TABLE_SCHEMA) +
N'.' +
QUOTENAME(TABLE_NAME) AS DropStatement
FROM
INFORMATION_SCHEMA.TABLES
WHERE
TABLE_SCHEMA = @Schema AND
TABLE_TYPE = N'VIEW'
UNION ALL
--functions
SELECT
2 AS SequenceNumber,
N'DROP FUNCTION ' +
QUOTENAME(ROUTINE_SCHEMA) +
N'.' +
QUOTENAME(ROUTINE_NAME) AS DropStatement
FROM
INFORMATION_SCHEMA.ROUTINES
WHERE
ROUTINE_SCHEMA = @Schema AND
ROUTINE_TYPE = N'FUNCTION'
UNION ALL
--procedures
SELECT
2 AS SequenceNumber,
N'DROP PROCEDURE ' +
QUOTENAME(ROUTINE_SCHEMA) +
N'.' +
QUOTENAME(ROUTINE_NAME) AS DropStatement
FROM
INFORMATION_SCHEMA.ROUTINES
WHERE
ROUTINE_SCHEMA = @Schema AND
ROUTINE_TYPE = N'PROCEDURE'
UNION ALL
--foreign keys
SELECT
4 AS SequenceNumber,
N'ALTER TABLE ' +
QUOTENAME(TABLE_SCHEMA) +
N'.' +
QUOTENAME(TABLE_NAME) +
N' DROP CONSTRAINT ' +
CONSTRAINT_NAME AS DropStatement
FROM
INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE
TABLE_SCHEMA = @Schema AND
CONSTRAINT_TYPE = N'FOREIGN KEY'
UNION ALL
--tables
SELECT
5 AS SequenceNumber,
N'DROP TABLE ' +
QUOTENAME(TABLE_SCHEMA) +
N'.' +
QUOTENAME(TABLE_NAME) AS DropStatement
FROM
INFORMATION_SCHEMA.TABLES
WHERE
TABLE_SCHEMA = @Schema AND
TABLE_TYPE = N'BASE TABLE'
ORDER BY SequenceNumber

OPEN DropStatements
WHILE 1 = 1
BEGIN
FETCH NEXT FROM DropStatements INTO @SequenceNumber, @DropStatement
IF @@FETCH_STATUS = -1 BREAK
BEGIN
RAISERROR('%s', 0, 1, @DropStatement) WITH NOWAIT
PRINT @DropStatement
EXECUTE sp_ExecuteSQL @DropStatement
SET @LastError = @@ERROR
IF @LastError > 0
BEGIN
RAISERROR('Script terminated due to unexpected error', 16, 1)
GOTO Done
END
END
END
CLOSE DropStatements
DEALLOCATE DropStatements

Done:

GO