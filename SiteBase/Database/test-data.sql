-- test navigation

SET IDENTITY_INSERT [sitebase].[NavigationItem] ON

INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(1,0,NULL,1,NULL,1,'Home',NULL,'/',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(2,0,NULL,2,NULL,1,'Sign In',NULL,'/identity/signIn',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(3,0,NULL,2,NULL,1,'Sign Out',NULL,'/identity/signOut',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(4,0,NULL,1,NULL,2,'Register',NULL,'/identity/register',NULL)

INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(5,0,NULL,1,NULL,99,'Account',NULL,'/account',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(6,0,NULL,1,5,1,'Update Profile',NULL,'/account/updateProfile',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(7,0,NULL,1,5,2,'Change Password',NULL,'/account/changePassword',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(8,0,NULL,1,5,3,'Change Security Question',NULL,'/account/changeSecurityQuestion',NULL)

INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(9,0,NULL,1,NULL,98,'Admin',NULL,'/admin',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(10,0,NULL,1,9,1,'Users',NULL,'/users',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(11,0,NULL,1,9,1,'Modules',NULL,'/modules',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(12,0,NULL,1,9,1,'Navigation',NULL,'/navigationItems',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(13,0,NULL,1,9,1,'Email Queue',NULL,'/queuedEmails',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(14,0,NULL,1,9,1,'Role Groups',NULL,'/roleGroups',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(15,0,NULL,1,9,1,'Roles',NULL,'/roles',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(16,0,NULL,1,9,1,'Permissions',NULL,'/permissions',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(17,0,NULL,1,9,1,'Audit Log',NULL,'/auditLog',NULL)
INSERT INTO [sitebase].[NavigationItem] ([Id],[ModificationCounter],[AssociationId],[NavigationId],[ParentId],[DisplayOrder],[Text],[ModuleId],[Url],[ImageUrl])VALUES(18,0,NULL,1,9,1,'Localization',NULL,'/localization',NULL)

SET IDENTITY_INSERT [sitebase].[NavigationItem] OFF
	
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/',4,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/postalCodes/json',4,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/identity/signIn',4,2,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/identity/signOut',4,3,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/identity/register',4,2,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/account',4,3,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/account/updateProfile',4,3,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/account/changePassword',4,3,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/account/changeSecurityQuestion',4,3,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/admin',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/roleGroups',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/roles',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/permissions',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/users',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/modules',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/navigationItems',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/auditLog',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/localization',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/queuedEmails',3,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/queuedEmails/processQueue',4,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/content/flexible/default',4,1,1)
INSERT [sitebase].[Permission] ([Key1],[Key3],[EntityTypeId],[EntityId],[Mask])
	VALUES('SitePath','/content/basic',4,1,1)

-- test user

INSERT INTO [sitebase].[Person]([Created], [FirstName], [LastName])
	VALUES(getdate(), 'Test', 'User')

INSERT INTO [sitebase].[User]([Username], [DisplayName], [Email], [PersonId])
	SELECT 'test.user', 'Test User', 'test.user@mydomain.net', MAX([Id]) FROM [sitebase].[Person]

INSERT INTO [sitebase].[UserAssociation]([UserId], [AssociationId])
	SELECT MAX([Id]), 1 FROM [sitebase].[User]	

INSERT INTO [sitebase].[UserRole]([UserId], [RoleId], [AssociationId])
	SELECT MAX([Id]), 3, 1 FROM [sitebase].[User]

/*
	
-- test module

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder],[AllowMultiple]) VALUES (999, 'Test Module', '00.00.01', 999, 1)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

INSERT INTO [sitebase].[Module] ([AssociationId],[ModuleDefinitionId],[Name],[Url],[DefaultInstance])
	VALUES(1,999,'Test 123','~/test123',1)

INSERT INTO [sitebase].[Module] ([AssociationId],[ModuleDefinitionId],[Name],[Url])
	VALUES(1,999,'Test XYZ','~/testxyz')

INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 1, 1, 0, 'Test.ShortText', 'ShortText*', '00.00.01', null, 'short text default')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 2, 2, 0, 'Test.LongText', 'LongText*', '00.00.01', null, 'long text default')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 3, 3, 0, 'Test.Number', 'Number*', '00.00.01', null, '999.999')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 4, 4, 0, 'Test.Date', 'Date*', '00.00.01', null, null)
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 5, 5, 0, 'Test.Boolean', 'Boolean*', '00.00.01', null, 'True')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 6, 6, 0, 'Test.MessageTemplate', 'MessageTemplate*', '00.00.01', 'message template subject default', 'message template content default')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 7, 7, 0, 'Test.Integer', 'Integer*', '00.00.01', null, '999')
INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(999, 8, 8, 0, 'Test.Currency', 'Currency*', '00.00.01', null, '999.99')

*/	