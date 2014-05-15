IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE [type] = 'S' and [name] = 'web')
	CREATE LOGIN [web] WITH PASSWORD = 'Password1';
GO

CREATE USER [web] FOR LOGIN [web];
GO

CREATE SCHEMA [web] AUTHORIZATION [web]
GO

ALTER USER [web] WITH DEFAULT_SCHEMA=[web]
GO

EXEC sys.sp_addrolemember @rolename = N'db_datareader', @membername = N'web';
EXEC sys.sp_addrolemember @rolename = N'db_datawriter', @membername = N'web';
EXEC sys.sp_addrolemember @rolename = N'db_ddladmin', @membername = N'web';
EXEC sys.sp_addrolemember @rolename = N'aspnet_Membership_FullAccess', @membername = N'web';
GO
