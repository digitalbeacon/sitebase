-- variables

declare @OrganizationKey varchar(100) = 'MySite'
declare @OrganizationName varchar(100) = 'My Site'
declare @OrganizationEmail varchar(100) = 'info@mydomain.net'
declare @SiteUrl varchar(100) = 'http://localhost/sitebase'
declare @MobileAppName varchar(100) = ''

-- sql update

INSERT INTO [SqlUpdate]([Version],[PatchNumber],[Module]) VALUES('3.1.1', 999, 'SiteBase')

-- lookup data

SET IDENTITY_INSERT [sitebase].[ComparisonOperator] ON
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (1, 'Equal')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (2, 'Not Equal')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (3, 'Null')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (4, 'Not Null')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (5, 'Less Than')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (6, 'Less Than Or Equal')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (7, 'Greater Than')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (8, 'Greater Than Or Equal')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (9, 'Contains')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (10, 'Starts With')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (11, 'Ends With')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (12, 'In')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (101, 'Sort Ascending')
INSERT INTO [sitebase].[ComparisonOperator]([Id],[Name]) VALUES (102, 'Sort Descending')
SET IDENTITY_INSERT [sitebase].[ComparisonOperator] OFF

SET IDENTITY_INSERT [sitebase].[Language] ON
INSERT INTO [sitebase].[Language]([Id],[Code],[Name]) VALUES (1, 'en-US', 'English')
SET IDENTITY_INSERT [sitebase].[Language] OFF

SET IDENTITY_INSERT [sitebase].[Country] ON
INSERT INTO [sitebase].[Country]([Id],[Code],[Name]) VALUES (1, 'US', 'United States')
SET IDENTITY_INSERT [sitebase].[Country] OFF

SET IDENTITY_INSERT [sitebase].[State] ON
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 1, 1, 'AL','Alabama')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 2, 1, 'AK','Alaska')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 3, 1, 'AZ','Arizona')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 4, 1, 'AR','Arkansas')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 5, 1, 'CA','California')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 6, 1, 'CO','Colorado')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 7, 1, 'CT','Connecticut')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 8, 1, 'DE','Delaware')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES ( 9, 1, 'DC','District of Columbia')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (10, 1, 'FL','Florida')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (11, 1, 'GA','Georgia')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (12, 1, 'HI','Hawaii')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (13, 1, 'ID','Idaho')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (14, 1, 'IL','Illinois')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (15, 1, 'IN','Indiana')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (16, 1, 'IA','Iowa')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (17, 1, 'KS','Kansas')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (18, 1, 'KY','Kentucky')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (19, 1, 'LA','Louisianna')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (20, 1, 'ME','Maine')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (21, 1, 'MD','Maryland')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (22, 1, 'MA','Massachusetts')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (23, 1, 'MI','Michigan')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (24, 1, 'MN','Minnesota')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (25, 1, 'MS','Mississippi')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (26, 1, 'MO','Missouri')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (27, 1, 'MT','Montana')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (28, 1, 'NE','Nebraska')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (29, 1, 'NV','Nevada')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (30, 1, 'NH','New Hampshire')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (31, 1, 'NJ','New Jersey')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (32, 1, 'NM','New Mexico')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (33, 1, 'NY','New York')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (34, 1, 'NC','North Carolina')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (35, 1, 'ND','North Dakoda')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (36, 1, 'OH','Ohio')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (37, 1, 'OK','Oklahoma')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (38, 1, 'OR','Oregon')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (39, 1, 'PA','Pennsylvania')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (40, 1, 'RI','Rhode Island')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (41, 1, 'SC','South Carolina')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (42, 1, 'SD','South Dakoda')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (43, 1, 'TN','Tennessee')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (44, 1, 'TX','Texas')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (45, 1, 'UT','Utah')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (46, 1, 'VT','Vermont')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (47, 1, 'VA','Virginia')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (48, 1, 'WA','Washington')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (49, 1, 'WV','West Virginia')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (50, 1, 'WI','Wisconsin')
INSERT INTO [sitebase].[State]([Id],[CountryId],[Code],[Name]) VALUES (51, 1, 'WY','Wyoming')
SET IDENTITY_INSERT [sitebase].[State] OFF

SET IDENTITY_INSERT [sitebase].[Gender] ON
INSERT INTO [sitebase].[Gender]([Id],[Code],[Name]) VALUES (1, 'M', 'Male')
INSERT INTO [sitebase].[Gender]([Id],[Code],[Name]) VALUES (2, 'F', 'Female')
SET IDENTITY_INSERT [sitebase].[Gender] OFF

SET IDENTITY_INSERT [sitebase].[Relationship] ON
INSERT INTO [sitebase].[Relationship]([Id],[DisplayOrder],[Name]) VALUES (1, 999, 'Other')
INSERT INTO [sitebase].[Relationship]([Id],[DisplayOrder],[Name]) VALUES (2, 1, 'Spouse')
INSERT INTO [sitebase].[Relationship]([Id],[DisplayOrder],[Name]) VALUES (3, 2, 'Parent')
INSERT INTO [sitebase].[Relationship]([Id],[DisplayOrder],[Name]) VALUES (4, 3, 'Child')
SET IDENTITY_INSERT [sitebase].[Relationship] OFF

SET IDENTITY_INSERT [sitebase].[PhoneType] ON
INSERT INTO [sitebase].[PhoneType]([Id],[Name]) VALUES (1, 'Home')
INSERT INTO [sitebase].[PhoneType]([Id],[Name]) VALUES (2, 'Work')
INSERT INTO [sitebase].[PhoneType]([Id],[Name]) VALUES (3, 'Mobile')
SET IDENTITY_INSERT [sitebase].[PhoneType] OFF

SET IDENTITY_INSERT [sitebase].[SecurityQuestion] ON
INSERT INTO [sitebase].[SecurityQuestion]([Id],[DisplayOrder],[Text]) VALUES (1, 1, 'What is your favorite movie?')
INSERT INTO [sitebase].[SecurityQuestion]([Id],[DisplayOrder],[Text]) VALUES (2, 2, 'What was the name of your first pet?')
INSERT INTO [sitebase].[SecurityQuestion]([Id],[DisplayOrder],[Text]) VALUES (3, 3, 'What is your mother''s maiden name?')
INSERT INTO [sitebase].[SecurityQuestion]([Id],[DisplayOrder],[Text]) VALUES (4, 4, 'What is your father''s middle name?')
SET IDENTITY_INSERT [sitebase].[SecurityQuestion] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingType] ON
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (0, 'Custom')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (1, 'Short Text')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (2, 'Long Text')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (3, 'Number')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (4, 'Date')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (5, 'Boolean')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (6, 'Message Template')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (7, 'Integer')
INSERT INTO [sitebase].[ModuleSettingType]([Id],[Name]) VALUES (8, 'Currency')
SET IDENTITY_INSERT [sitebase].[ModuleSettingType] OFF

SET IDENTITY_INSERT [sitebase].[AuditAction] ON
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (1, 'Log In')
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (2, 'Log Out')
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (3, 'Log In Failed')
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (4, 'Create Entity')
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (5, 'Update Entity')
INSERT INTO [sitebase].[AuditAction]([Id],[Name]) VALUES (6, 'Delete Entity')
SET IDENTITY_INSERT [sitebase].[AuditAction] OFF

-- association

INSERT INTO [sitebase].[Address]([Email]) VALUES(@OrganizationEmail)

SET IDENTITY_INSERT [sitebase].[Association] ON
INSERT INTO [sitebase].[Association]([Id],[Key],[Name],[AddressId])
	SELECT 1, @OrganizationKey, @OrganizationName, MAX([Id]) FROM [sitebase].[Address]
SET IDENTITY_INSERT [sitebase].[Association] OFF

-- roles

SET IDENTITY_INSERT [sitebase].[RoleGroup] ON
INSERT INTO [sitebase].[RoleGroup]([Id],[Name]) VALUES(1, 'Everyone')
INSERT INTO [sitebase].[RoleGroup]([Id],[Name]) VALUES(2, 'Unauthenticated')
INSERT INTO [sitebase].[RoleGroup]([Id],[Name]) VALUES(3, 'Authenticated')
INSERT INTO [sitebase].[RoleGroup]([Id],[DisplayOrder],[Name]) VALUES(4, 0, 'Hidden')
INSERT INTO [sitebase].[RoleGroup]([Id],[Name]) VALUES(5, 'Admin')
SET IDENTITY_INSERT [sitebase].[RoleGroup] OFF

SET IDENTITY_INSERT [sitebase].[Role] ON
INSERT INTO [sitebase].[Role]([Id],[Name]) VALUES(1, 'Administrator')
INSERT INTO [sitebase].[Role]([Id],[Name]) VALUES(2, 'Site Manager')
INSERT INTO [sitebase].[Role]([Id],[Name]) VALUES(3, 'User')
INSERT INTO [sitebase].[Role]([Id],[Name]) VALUES(4, 'Guest')
SET IDENTITY_INSERT [sitebase].[Role] OFF

-- users

INSERT INTO [sitebase].[Person]([Created], [FirstName], [LastName])
	VALUES(getdate(), 'Admin', 'User')
	
INSERT INTO [sitebase].[User]([Username], [DisplayName], [Email], [PersonId])
	SELECT 'admin', 'Admin User', 'admin@mydomain.net', MAX([Id]) FROM [sitebase].[Person]

INSERT INTO [sitebase].[UserAssociation]([UserId], [AssociationId])
	SELECT MAX([Id]), 1 FROM [sitebase].[User]	

INSERT INTO [sitebase].[UserRole]([UserId], [RoleId], [AssociationId])
	SELECT MAX([Id]), 1, 1 FROM [sitebase].[User]	

-- substitutions

SET IDENTITY_INSERT [sitebase].[SubstitutionDefinition] ON
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (1, 'SITE_URL')
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (2, 'DYNAMIC_CONTENT')
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (3, 'USER_DISPLAY_NAME')
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (4, 'USERNAME')
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (5, 'PASSWORD')
INSERT INTO [sitebase].[SubstitutionDefinition]([Id],[Name]) VALUES (6, 'CONFIRM_REGISTRATION_URL')
SET IDENTITY_INSERT [sitebase].[SubstitutionDefinition] OFF

-- permissions

SET IDENTITY_INSERT [sitebase].[EntityType] ON
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (1, 'Person', 'DigitalBeacon.SiteBase.Model.PersonEntity')
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (2, 'User', 'DigitalBeacon.SiteBase.Model.UserEntity')
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (3, 'Role', 'DigitalBeacon.SiteBase.Model.RoleEntity')
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (4, 'Role Group', 'DigitalBeacon.SiteBase.Model.RoleGroupEntity')
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (5, 'Predicate Group', 'DigitalBeacon.SiteBase.Model.PredicateGroupEntity')
INSERT INTO [sitebase].[EntityType]([Id],[Name],[Type]) VALUES (6, 'Contact', 'DigitalBeacon.SiteBase.Model.ContactEntity')
SET IDENTITY_INSERT [sitebase].[EntityType] OFF

-- global

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (0, 'Site', '1.0.0', 1)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(1, 0, 1, 1, 1, 'Global.SiteUrl', 'Site Url', '1.0.0', @SiteUrl)
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(2, 0, 1, 2, 1, 'Global.AdminUsername', 'Admin Username', '1.0.0', 'admin')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(3, 0, 6, 3, 1, 'Global.Content.Error', 'Error Page Content', '1.0.0', 'Oops!', 'An error has occurred while processing your request.')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(4, 0, 6, 4, 1, 'Global.Email.Exception', 'Exception Email', '1.0.0', 'Exception Notification', '<div style="font-family: Consolas, Courier New; font-size: 10pt">$$DYNAMIC_CONTENT$$</div>')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue],[MinValue],[MaxValue])
	VALUES(5, 0, 7, 5, 1, 'Global.List.PageSize', 'Default List Page Size', '1.0.0', '10', 1, 100)
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

INSERT INTO [sitebase].[ModuleSettingDefinition]([ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(0, 1, 100, 1, 'MobileAppName', 'Mobile Application Name', '4.0.0', @MobileAppName)

INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 4, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'DYNAMIC_CONTENT'

-- identity

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (7, 'Identity', '1.0.0', 2)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(51, 7, 5, 1, 1, 'Identity.ShowMiddleName', 'Show Middle Name', '1.0.0', 'False')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(52, 7, 5, 2, 1, 'Identity.RequireAddress', 'Require Address', '1.0.0', 'False')
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

-- login

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (1, 'Login', '1.0.0', 3)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue],[Localizable])
	VALUES(101, 1, 6, 1, 1, 'Login.Email.UsernameRequested', 'Username Request Email', '1.0.0', 'Username Request', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>Your username is <strong>$$USERNAME$$</strong>.</p></div>', 1)
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 101, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 101, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USERNAME'

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue],[Localizable])
	VALUES(102, 1, 6, 2, 1, 'Login.Email.PasswordChanged', 'Password Changed Email', '1.0.0', 'Account Notification', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>Your account password was recently changed. If you did not change your password, please reply to this email and let us know so we can review your account.</p></div>', 1)
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 102, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'

-- registration

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (2, 'Registration', '1.0.0', 4)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON

INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(111, 2, 5, 1, 1, 'Registration.Enabled', 'Registration Enabled', '1.0.0', 'True')

INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue],[Localizable])
	VALUES(113, 2, 6, 3, 1, 'Registration.Email.Submitted', 'Submission Email', '1.0.0', 'Confirm Registration', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>Thank you for your registration. Please click the link below to confirm your email address and complete the registration process.</p><p><a href="$$CONFIRM_REGISTRATION_URL$$">$$CONFIRM_REGISTRATION_URL$$</a></p></div>', 1)
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 113, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 113, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'CONFIRM_REGISTRATION_URL'

--INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
--	VALUES(114, 2, 6, 4, 1, 'Registration.Confirmation.Complete', 'Completion Confirmation', '1.0.0', 'Registration Complete', '<p>Your registration is now complete, and your account has been activated.</p>')

INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue],[Localizable])
	VALUES(115, 2, 6, 5, 1, 'Registration.Email.Complete', 'Completion Email', '1.0.0', 'Welcome!', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>Your registration is now complete, and your account has been activated.</p></div>', 1)
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 115, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'

INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue],[Localizable])
	VALUES(116, 2, 6, 6, 1, 'Registration.Email.UserCreatedByAdmin', 'Created by Admin Notification', '1.0.0', 'Welcome!', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>An account has been created for you.</p><p>Your username is <strong>$$USERNAME$$</strong> and your temporary password is <strong>$$PASSWORD$$</strong>. You will be required to change your password after you log in for the first time.</p></div>',1)
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 116, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 116, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USERNAME'
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 116, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'PASSWORD'
	
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(117, 2, 1, 101, 1, 'Registration.Url.ConfirmBaseUrl', 'Confirm Registration Base Url', '1.0.0', '/identity/confirmRegistration')

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

-- basic content

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder],[AllowMultiple]) VALUES (3, 'Basic Content', '1.0.0', 5, 1)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[Module] ON
INSERT INTO [sitebase].[Module] ([Id],[ModificationCounter],[AssociationId],[ModuleDefinitionId],[Name],[Url],[DefaultInstance])
	VALUES(1,0,1,3,'Default','~',1)
SET IDENTITY_INSERT [sitebase].[Module] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(121, 3, 6, 1, 0, 'Content.Main', 'Title and Content', '1.0.0', 'Basic Content Title', '<p>This is the default text for the basic content module.</p>')
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

-- flexible content

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder],[AllowMultiple]) VALUES (4, 'Flexible Content', '1.0.0', 6, 0)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[ContentGroupType] ON
INSERT INTO [sitebase].[ContentGroupType]([Id],[Name],[CssClass],[DateFormat]) VALUES (1, 'Default', '', 'M/d/yyyy h:mm tt')
SET IDENTITY_INSERT [sitebase].[ContentGroupType] OFF

SET IDENTITY_INSERT [sitebase].[ContentGroup] ON
INSERT INTO [sitebase].[ContentGroup] ([Id],[ModificationCounter],[AssociationId],[Name],[Title],[ContentGroupTypeId],[DisplayOrder])
	VALUES(1, 0, 1, 'Default', 'Default Flexible Content Title', 1, 1)
SET IDENTITY_INSERT [sitebase].[ContentGroup] OFF

INSERT INTO [sitebase].[ContentEntry] ([LastModificationDate],[ContentGroupId],[ContentDate],[DisplayOrder],[Title],[Body])
	VALUES(getdate(),1,getdate(),1,'Flexible Content','<p><img src="~/resources/images/stock.gif" class="shadow" style="float:right;margin-left:5px" />Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Vestibulum fringilla, arcu a consectetuer condimentum, odio velit dapibus risus, sit amet sollicitudin eros augue id sapien. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam a mi id nisl molestie molestie. Curabitur in est ac purus aliquam lacinia. Vestibulum imperdiet dolor non purus consequat euismod. Praesent faucibus, erat vitae sollicitudin accumsan, dolor orci rhoncus augue, ut placerat erat risus sed felis. Ut sit amet velit. Aliquam commodo risus ac arcu. Proin congue est ac ipsum. Donec dictum pede malesuada odio. In malesuada leo ut justo. Fusce neque tellus, condimentum non, congue quis, dignissim eget, augue. Donec pulvinar tortor ut lectus. Cras metus purus, commodo volutpat, sollicitudin auctor, aliquet nec, metus.</p><p>Proin non sem. Quisque consectetuer. Curabitur commodo risus vel ante. Sed placerat eros mollis sem. Suspendisse potenti. Aenean aliquet massa sed magna. Proin orci. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Cras scelerisque magna in diam. Curabitur congue massa. Donec at libero nec nunc consectetuer laoreet. Donec commodo malesuada quam. Quisque blandit ante vitae diam. Morbi quis ante. Proin sollicitudin mi molestie diam. Proin elit nunc, cursus blandit, luctus in, pellentesque nec, nisl. Etiam rhoncus, sapien vel imperdiet hendrerit, tellus risus bibendum elit, ut interdum ante urna in neque.</p><p>Praesent libero odio, malesuada cursus, varius eu, euismod eget, nunc. Donec sit amet mauris. Praesent id metus non lorem convallis volutpat. Pellentesque nisi. In sed augue. Mauris posuere. Nulla facilisi. Mauris libero pede, dignissim id, porttitor tincidunt, vestibulum vel, erat. Donec nisi diam, fermentum nec, semper non, ullamcorper eu, ante. Suspendisse vitae eros eu diam euismod nonummy. Maecenas ultrices purus et mi. Curabitur eget orci at magna sodales interdum. Etiam libero. Donec rhoncus rutrum eros. Quisque blandit sagittis leo. Etiam vel purus. Suspendisse arcu mauris, fermentum id, rutrum nec, lacinia eu, felis.</p><p>Nam tellus lectus, dignissim id, pharetra ac, rutrum non, metus. Nunc tincidunt, urna eu bibendum blandit, orci magna porta arcu, ut vehicula ipsum nulla ac lectus. Aenean malesuada felis quis orci. Morbi ornare viverra elit. Quisque elit lacus, ullamcorper commodo, lacinia vel, vulputate eu, purus. Nunc vitae sapien accumsan orci dictum adipiscing. Nunc eget dui. Vivamus venenatis commodo ante. Cras sit amet massa. Mauris iaculis felis sit amet eros. Integer tempor, quam nec aliquam accumsan, augue quam viverra eros, sit amet vestibulum nisi est quis ante.</p><p>Aliquam eleifend, ipsum et ornare vehicula, ligula elit vestibulum eros, sit amet condimentum tellus nisl in ante. In lorem risus, rutrum ut, sollicitudin in, iaculis quis, lorem. Praesent sodales tincidunt arcu. Quisque euismod ligula porta metus. Integer vehicula risus a mi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed iaculis. Proin pulvinar nibh id velit. Duis elementum. Nam posuere tristique urna.</p>')

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[CustomEditor])
	VALUES(131, 4, 0, 1, 1, 'FlexibleContent.ContentGroups', 'Content Groups', '1.0.0', '/contentGroups/index')
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

-- files

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (5, 'Files', '1.0.0', -1)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[FolderType] ON
INSERT INTO [sitebase].[FolderType]([Id],[Name]) VALUES (1, 'File')
SET IDENTITY_INSERT [sitebase].[FolderType] OFF

-- messaging

SET IDENTITY_INSERT [sitebase].[ModuleDefinition] ON
INSERT INTO [sitebase].[ModuleDefinition]([Id],[Name],[VersionNumber],[DisplayOrder]) VALUES (6, 'Messaging', '1.0.0', -1)
SET IDENTITY_INSERT [sitebase].[ModuleDefinition] OFF

SET IDENTITY_INSERT [sitebase].[FolderType] ON
INSERT INTO [sitebase].[FolderType]([Id],[Name]) VALUES (2, 'Message')
SET IDENTITY_INSERT [sitebase].[FolderType] OFF

SET IDENTITY_INSERT [sitebase].[Folder] ON
INSERT INTO [sitebase].[Folder]([Id],[TypeId],[AssociationId],[DisplayOrder],[Name]) VALUES(1, 2, 1, 1, 'Inbox')
INSERT INTO [sitebase].[Folder]([Id],[TypeId],[AssociationId],[DisplayOrder],[Name]) VALUES(2, 2, 1, 2, 'Drafts')
INSERT INTO [sitebase].[Folder]([Id],[TypeId],[AssociationId],[DisplayOrder],[Name]) VALUES(3, 2, 1, 3, 'Archived Items')
INSERT INTO [sitebase].[Folder]([Id],[TypeId],[AssociationId],[DisplayOrder],[Name]) VALUES(4, 2, 1, 4, 'Sent Items')
INSERT INTO [sitebase].[Folder]([Id],[TypeId],[AssociationId],[DisplayOrder],[Name]) VALUES(5, 2, 1, 5, 'Trash')
SET IDENTITY_INSERT [sitebase].[Folder] OFF

INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 1, 3, 2, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 2, 3, 2, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 3, 3, 2, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 4, 3, 2, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 5, 3, 2, 1)
																											 
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 1, 3, 3, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 2, 3, 3, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 3, 3, 3, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 4, 3, 3, 1)
INSERT INTO [sitebase].[Permission]([Key1],[Key2],[EntityTypeId],[EntityId],[Mask]) VALUES('FolderEntity', 5, 3, 3, 1)

SET IDENTITY_INSERT [sitebase].[MessageImportance] ON
INSERT INTO [sitebase].[MessageImportance]([Id],[Name]) VALUES(1, 'Low')
INSERT INTO [sitebase].[MessageImportance]([Id],[Name]) VALUES(2, 'Normal')
INSERT INTO [sitebase].[MessageImportance]([Id],[Name]) VALUES(3, 'High')
SET IDENTITY_INSERT [sitebase].[MessageImportance] OFF

SET IDENTITY_INSERT [sitebase].[NotificationPreference] ON
INSERT INTO [sitebase].[NotificationPreference]([Id],[Name]) VALUES(0, 'First New Message')
INSERT INTO [sitebase].[NotificationPreference]([Id],[Name]) VALUES(1, 'Every Message')
INSERT INTO [sitebase].[NotificationPreference]([Id],[Name]) VALUES(2, 'Once Daily')
INSERT INTO [sitebase].[NotificationPreference]([Id],[Name]) VALUES(3, 'Never')
SET IDENTITY_INSERT [sitebase].[NotificationPreference] OFF

SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] ON
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(151, 6, 1, 1, 1, 'Messaging.AttachmentTypes', 'Allowed Attachment Types', '1.0.0', 'pdf,jpg,gif,txt,zip,doc,docx,xls,xlsx')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultValue])
	VALUES(152, 6, 3, 2, 1, 'Messaging.MaxAttachmentSize', 'Max Attachment Size in KB', '1.0.0', '8192')
INSERT INTO [sitebase].[ModuleSettingDefinition]([Id],[ModuleDefinitionId],[ModuleSettingTypeId],[DisplayOrder],[Global],[Key],[Name],[IntroducedInVersion],[DefaultSubject],[DefaultValue])
	VALUES(153, 6, 6, 3, 1, 'Messaging.NotificationMessage', 'Notificaton Message', '1.0.0', 'Message Notification', '<div style="font-family: Segoe UI, Tahoma; font-size: 10pt"><p>$$USER_DISPLAY_NAME$$,</p><p>You have one or more new messages in your Inbox. Please login to your account at $$SITE_URL$$ to access your messages.</p></div>')
SET IDENTITY_INSERT [sitebase].[ModuleSettingDefinition] OFF

INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 153, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'USER_DISPLAY_NAME'
INSERT INTO [sitebase].[ModuleSettingSubstitution]([ModuleSettingDefinitionId], [SubstitutionDefinitionId])
	SELECT 153, [Id] FROM [sitebase].[SubstitutionDefinition] WHERE [Name] = 'SITE_URL'

SET IDENTITY_INSERT [sitebase].[MessageType] ON
INSERT INTO [sitebase].[MessageType]([Id],[Name]) VALUES(0, 'Secure Message')
SET IDENTITY_INSERT [sitebase].[MessageType] OFF

-- navigation

SET IDENTITY_INSERT [sitebase].[Navigation] ON
INSERT INTO [sitebase].[Navigation] ([Id],[ModificationCounter],[Name])VALUES(1,0,'Top Left')
INSERT INTO [sitebase].[Navigation] ([Id],[ModificationCounter],[Name])VALUES(2,0,'Top Right')
INSERT INTO [sitebase].[Navigation] ([Id],[ModificationCounter],[Name])VALUES(3,0,'Left')
INSERT INTO [sitebase].[Navigation] ([Id],[ModificationCounter],[Name])VALUES(4,0,'Mobile')
SET IDENTITY_INSERT [sitebase].[Navigation] OFF

-- contact type

SET IDENTITY_INSERT [sitebase].[ContactType] ON
INSERT INTO [sitebase].[ContactType] ([Id],[ModificationCounter],[Name])VALUES(1,0,'Default')
SET IDENTITY_INSERT [sitebase].[ContactType] OFF

-- contact comment type

SET IDENTITY_INSERT [sitebase].[ContactCommentType] ON
INSERT INTO [sitebase].[ContactCommentType] ([Id],[ModificationCounter],[Name],[Flagged],[DisplayOrder])VALUES(1,0,'Note',0,1)
SET IDENTITY_INSERT [sitebase].[ContactCommentType] OFF
