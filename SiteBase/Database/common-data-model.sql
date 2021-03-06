SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM [sys].[schemas] WHERE [name] = 'sitebase')
BEGIN
	EXEC('CREATE SCHEMA [sitebase] AUTHORIZATION [web]')
END
GO

CREATE TABLE [SqlUpdate] (
	[Version] [nvarchar](20) NOT NULL,
	[PatchNumber] [int] NOT NULL,
	[Applied] [datetime] NOT NULL CONSTRAINT [DF_SqlUpdate_Applied] DEFAULT getdate(),
	[Module] [nvarchar](20) NOT NULL,
	CONSTRAINT [PK_DataUpdate] PRIMARY KEY CLUSTERED ([Module] ASC, [Version] ASC, [PatchNumber] ASC) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [sitebase].[EntityType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_EntityType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](100) NULL,
	CONSTRAINT [PK_EntityType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_EntityType_Name] ON [sitebase].[EntityType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ComparisonOperator](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ComparisonOperator_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_ComparisonOperator] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ComparisonOperator_Name] ON [sitebase].[ComparisonOperator] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Language](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Language_ModificationCounter] DEFAULT 0,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Language_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Language_Name] ON [sitebase].[Language] ([Name]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_Language_Code] ON [sitebase].[Language] ([Code]) ON [PRIMARY]

CREATE TABLE [sitebase].[Country](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Country_ModificationCounter] DEFAULT 0,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Country_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Country_Name] ON [sitebase].[Country] ([Name]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_Country_Code] ON [sitebase].[Country] ([Code]) ON [PRIMARY]

CREATE TABLE [sitebase].[State](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_State_ModificationCounter] DEFAULT 0,
	[CountryId] [bigint] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryId]) REFERENCES [sitebase].[Country] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_State_Name] ON [sitebase].[State] ([CountryId],[Name]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_State_Code] ON [sitebase].[State] ([CountryId],[Code]) ON [PRIMARY]

CREATE TABLE [sitebase].[Gender](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Gender_ModificationCounter] DEFAULT 0,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
	CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Gender_Name] ON [sitebase].[Gender] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Relationship](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Relationship_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Relationship_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_Relationship] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Relationship_Name] ON [sitebase].[Relationship] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[PhoneType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_PhoneType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_PhoneType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_PhoneType_Name] ON [sitebase].[PhoneType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Address](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Address_ModificationCounter] DEFAULT 0,
	[Line1] [nvarchar](200) NULL,
	[Line2] [nvarchar](200) NULL,
	[City] [nvarchar](100) NULL,
	[StateId] [bigint] NULL,
	[CountryId] [bigint] NULL,
	[PostalCode] [nvarchar](20) NULL,
	[County] [nvarchar](50) NULL,
	[DefaultPhoneId] [bigint] NULL,
	[HomePhone] [nvarchar](20) NULL,
	[WorkPhone] [nvarchar](20) NULL,
	[WorkPhoneExt] [nvarchar](10) NULL,
	[MobilePhone] [nvarchar](20) NULL,
	[Fax] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [sitebase].[Country] ([Id]),
	CONSTRAINT [FK_Address_State] FOREIGN KEY ([StateId]) REFERENCES [sitebase].[State] ([Id]),
	CONSTRAINT [FK_Address_PhoneType] FOREIGN KEY ([DefaultPhoneId]) REFERENCES [sitebase].[PhoneType] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[PostalCode](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_PostalCode_ModificationCounter] DEFAULT 0,
	[Code] [nvarchar](20) NOT NULL,
	[City] [nvarchar](100) NULL,
	[StateCode] [nvarchar](20) NULL,
	[County] [nvarchar](50) NULL,
	CONSTRAINT [PK_PostalCode] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_PostalCode_Code] ON [sitebase].[PostalCode] ([Code]) ON [PRIMARY]

CREATE TABLE [sitebase].[Race](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Race_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Race_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_Race] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Race_Name] ON [sitebase].[Race] ([Name]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_Race_Code] ON [sitebase].[Race] ([Code]) ON [PRIMARY]

CREATE TABLE [sitebase].[Person](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Person_ModificationCounter] DEFAULT 0,
	[Created] [datetime] NOT NULL,
	[Deleted] [datetime] NULL,
	[FirstName] [nvarchar](100) NULL,
	[MiddleName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Title] [nvarchar](100) NULL,
	[Suffix] [nvarchar](100) NULL,
	[GenderId] [bigint] NULL,
	[DateOfBirth] [datetime] NULL,
	[AddressId] [bigint] NULL,
	[EncryptedSsn] [nvarchar](100) NULL,
	[Ssn4] [nvarchar](4) NULL,
	[RaceId] [bigint] NULL,
	CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Person_Address] FOREIGN KEY ([AddressId]) REFERENCES [sitebase].[Address] ([Id]),
	CONSTRAINT [FK_Person_Gender] FOREIGN KEY ([GenderId]) REFERENCES [sitebase].[Gender] ([Id]),
	CONSTRAINT [FK_Person_Race] FOREIGN KEY ([RaceId]) REFERENCES [sitebase].[Race] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[Person_Archive](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_PersonArchive_ModificationCounter] DEFAULT 0,
	[RefId] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Archived] [datetime] NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[MiddleName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Title] [nvarchar](100) NULL,
	[Suffix] [nvarchar](100) NULL,
	[GenderId] [bigint] NULL,
	[DateOfBirth] [datetime] NULL,
	[AddressId] [bigint] NULL,
	[EncryptedSsn] [nvarchar](100) NULL,
	[Ssn4] [nvarchar](4) NULL,
	[RaceId] [bigint] NULL,
	[PhotoId] [bigint] NULL,
	[PhotoWidth] [int] NULL,
	[PhotoHeight] [int] NULL,
	CONSTRAINT [PK_PersonArchive] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_PersonArchive_Person] FOREIGN KEY ([RefId]) REFERENCES [sitebase].[Person] ([Id]),
	CONSTRAINT [FK_PersonArchive_Address] FOREIGN KEY ([AddressId]) REFERENCES [sitebase].[Address] ([Id]),
	CONSTRAINT [FK_PersonArchive_Gender] FOREIGN KEY ([GenderId]) REFERENCES [sitebase].[Gender] ([Id]),
	CONSTRAINT [FK_PersonArchive_Race] FOREIGN KEY ([RaceId]) REFERENCES [sitebase].[Race] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[Association](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Association_ModificationCounter] DEFAULT 0,
	[Key] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[AddressId] [bigint] NOT NULL,
	CONSTRAINT [PK_Association] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Association_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [sitebase].[Address] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Association_Key] ON [sitebase].[Association] ([Key]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_Association_Name] ON [sitebase].[Association] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_User_ModificationCounter] DEFAULT 0,
	[Deleted] [datetime] NULL,
	[Username] [nvarchar](100) NOT NULL,
	[DisplayName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[SuperUser] [bit] NOT NULL CONSTRAINT [DF_User_SuperUser] DEFAULT 0,
	[PersonId] [bigint] NOT NULL,
	[LanguageId] [bigint] NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_User_Person] FOREIGN KEY ([PersonId]) REFERENCES [sitebase].[Person] ([Id]),
	CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguageId]) REFERENCES [sitebase].[Language] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_User_Username] ON [sitebase].[User] ([Username]) ON [PRIMARY]

CREATE TABLE [sitebase].[UserAssociation](
	[UserId] [bigint] NOT NULL,
	[AssociationId] [bigint] NOT NULL,
	CONSTRAINT [PK_UserAssociation] PRIMARY KEY CLUSTERED ([UserId],[AssociationId]) ON [PRIMARY],
	CONSTRAINT [FK_UserAssociation_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_UserAssociation_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[Preference] (
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Preference_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[Key] [nvarchar] (100) NOT NULL,
	[UserId] [bigint] NULL,
	[Value] [nvarchar](MAX) NOT NULL,
	CONSTRAINT [PK_Preference] PRIMARY KEY CLUSTERED ([Id] DESC) ON [PRIMARY],
	CONSTRAINT [FK_Preference_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_Preference_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Preference_01] ON [sitebase].[Preference] ([AssociationId], [Key], [UserId]) ON [PRIMARY]

CREATE TABLE [sitebase].[RoleGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_RoleGroup_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_RoleGroup_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_RoleGroup] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_RoleGroup_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_RoleGroup_01] ON [sitebase].[RoleGroup] ([AssociationId],[Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Role](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Role_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RoleGroupId] [bigint] NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Role_RoleGroup] FOREIGN KEY ([RoleGroupId]) REFERENCES [sitebase].[RoleGroup] ([Id]),
	CONSTRAINT [FK_Role_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Role_01] ON [sitebase].[Role] ([AssociationId],[Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[UserRole](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_UserRole_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_UserRole_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [sitebase].[Role] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_UserRole_01] ON [sitebase].[UserRole] ([AssociationId],[UserId],[RoleId]) ON [PRIMARY]

CREATE TABLE [sitebase].[SecurityQuestion](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_SecurityQuestion_ModificationCounter] DEFAULT 0,
	[Text] [nvarchar](255) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_SecurityQuestion_DisplayOrder]  DEFAULT 1,
	CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [sitebase].[PredicateGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_PredicateGroup_ModificationCounter] DEFAULT 0,
	[TypeId] [bigint] NOT NULL,
	[AssociationId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_PredicateGroup_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_PredicateGroup] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_PredicateGroup_EntityType] FOREIGN KEY ([TypeId]) REFERENCES [sitebase].[EntityType] ([Id]),
	CONSTRAINT [FK_PredicateGroup_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_PredicateGroup_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_PredicateGroup_01] ON [sitebase].[PredicateGroup] ([TypeId],[AssociationId],[UserId],[Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Predicate](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Predicate_ModificationCounter] DEFAULT 0,
	[GroupId] [bigint] NULL,
	[Field] [nvarchar](50) NOT NULL,
	[OperatorId] [bigint] NOT NULL,
	[SerializedValue] [nvarchar](MAX) NULL,
	[Grouping] [int] NOT NULL CONSTRAINT [DF_Predicate_Grouping] DEFAULT 0,
	CONSTRAINT [PK_Predicate] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Predicate_PredicateGroup] FOREIGN KEY ([GroupId]) REFERENCES [sitebase].[PredicateGroup] ([Id]),
	CONSTRAINT [FK_Predicate_ComparisonOperator] FOREIGN KEY ([OperatorId]) REFERENCES [sitebase].[ComparisonOperator] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[ModuleDefinition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ModuleDefinition_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	[VersionNumber] [nvarchar](20) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_ModuleDefinition_DisplayOrder] DEFAULT 1,
	[AllowMultiple] [bit] NOT NULL CONSTRAINT [DF_ModuleDefinition_AllowMultiple] DEFAULT 0,
	CONSTRAINT [PK_ModuleDefinition] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ModuleDefinition_Name] ON [sitebase].[ModuleDefinition] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ModuleSettingType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ModuleSettingType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_ModuleSettingType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_ModuleSettingType_Name] ON [sitebase].[ModuleSettingType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ModuleSettingDefinition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ModuleSettingDefinition_ModificationCounter] DEFAULT 0,
	[ModuleDefinitionId] [bigint] NOT NULL,
	[ModuleSettingTypeId] [bigint] NOT NULL,
	[Key] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IntroducedInVersion] [nvarchar](20) NOT NULL,
	[CustomEditor] [nvarchar](200) NULL,
	[DefaultValue] [nvarchar](MAX) NULL,
	[DefaultSubject] [nvarchar](MAX) NULL,
	[MinValue] [float] NULL,
	[MaxValue] [float] NULL,
	[Required] [bit] NOT NULL CONSTRAINT [DF_ModuleSettingDefinition_Required] DEFAULT 1,
	[Global] [bit] NOT NULL CONSTRAINT [DF_ModuleSettingDefinition_Global] DEFAULT 0,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_ModuleSettingDefinition_DisplayOrder]  DEFAULT 1,
	[Localizable] [bit] NOT NULL CONSTRAINT [DF_ModuleSettingDefinition_Localizable] DEFAULT 0,
	CONSTRAINT [PK_ModuleSettingDefinition] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ModuleSettingDefinition_ModuleDefinition] FOREIGN KEY ([ModuleDefinitionId]) REFERENCES [sitebase].[ModuleDefinition] ([Id]),
	CONSTRAINT [FK_ModuleSettingDefinition_ModuleSettingType] FOREIGN KEY ([ModuleSettingTypeId]) REFERENCES [sitebase].[ModuleSettingType] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ModuleSettingDefinition_Key] ON [sitebase].[ModuleSettingDefinition] ([Key]) ON [PRIMARY]

CREATE TABLE [sitebase].[Module](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Module_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[ModuleDefinitionId] [bigint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](250) NOT NULL,
	[DefaultInstance] [bit] NOT NULL CONSTRAINT [DF_Module_DefaultInstance] DEFAULT 0,
	CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Module_ModuleDefinition] FOREIGN KEY ([ModuleDefinitionId]) REFERENCES [sitebase].[ModuleDefinition] ([Id]),
	CONSTRAINT [FK_Module_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Module_01] ON [sitebase].[Module] ([AssociationId], [Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ModuleSetting](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ModuleSetting_ModificationCounter] DEFAULT 0,
	[ModuleId] [bigint] NULL,
	[ModuleSettingDefinitionId] [bigint] NOT NULL,
	[Value] [nvarchar](MAX) NULL,
	[Subject] [nvarchar](MAX) NULL,
	CONSTRAINT [PK_ModuleSetting] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ModuleSetting_Module] FOREIGN KEY ([ModuleId]) REFERENCES [sitebase].[Module] ([Id]),
	CONSTRAINT [FK_ModuleSetting_ModuleSettingDefinition] FOREIGN KEY ([ModuleSettingDefinitionId]) REFERENCES [sitebase].[ModuleSettingDefinition] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[SubstitutionDefinition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_SubstitutionDefinition_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_SubstitutionDefinition] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_SubstitutionDefinition_Name] ON [sitebase].[SubstitutionDefinition] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ModuleSettingSubstitution](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ModuleSettingSubstitution_ModificationCounter] DEFAULT 0,
	[ModuleSettingDefinitionId] [bigint] NOT NULL,
	[SubstitutionDefinitionId] [bigint] NOT NULL,
	CONSTRAINT [PK_ModuleSettingSubstitution] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ModuleSettingSubstitution_ModuleSettingDefinition] FOREIGN KEY ([ModuleSettingDefinitionId]) REFERENCES [sitebase].[ModuleSettingDefinition] ([Id]),
	CONSTRAINT [FK_ModuleSettingSubstitution_SubstitutionDefinition] FOREIGN KEY ([SubstitutionDefinitionId]) REFERENCES [sitebase].[SubstitutionDefinition] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ModuleSettingSubstitution_01] ON [sitebase].[ModuleSettingSubstitution] ([ModuleSettingDefinitionId], [SubstitutionDefinitionId]) ON [PRIMARY]

CREATE TABLE [sitebase].[AuditAction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_AuditAction_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_AuditAction] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_AuditAction_Name] ON [sitebase].[AuditAction] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[AuditLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_AuditLog_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NULL,
	[Created] [datetime] NOT NULL,
	[AuditActionId] [bigint] NOT NULL,
	[UserId] [bigint] NULL,
	[RefId] [bigint] NULL,
	[EntityType] [nvarchar](200) NULL,
	[Details] [nvarchar](MAX) NULL,
	CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_AuditLog_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_AuditLog_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_AuditLog_AuditAction] FOREIGN KEY ([AuditActionId]) REFERENCES [sitebase].[AuditAction] ([Id])
) ON [PRIMARY]

-- permissions

CREATE TABLE [sitebase].[Permission](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Permission_ModificationCounter] DEFAULT 0,
	[Key1] [nvarchar](100) NOT NULL,
	[Key2] [bigint] NULL,
	[Key3] [nvarchar](100) NULL,
	[EntityTypeId] [bigint] NOT NULL,
	[EntityId] [bigint] NOT NULL,
	[Mask] [int] NOT NULL CONSTRAINT [DF_Permission_Granted] DEFAULT 0
	CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Permission_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [sitebase].[EntityType] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Permission_01] ON [sitebase].[Permission] ([Key1],[Key2],[Key3],[EntityTypeId],[EntityId]) ON [PRIMARY]

-- folders

CREATE TABLE [sitebase].[FolderType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_FolderType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_FolderType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_FolderType_Name] ON [sitebase].[FolderType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Folder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Folder_ModificationCounter] DEFAULT 0,
	[TypeId] [bigint] NOT NULL,
	[AssociationId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[ParentFolderId] [bigint] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Folder_DisplayOrder]  DEFAULT 1,
	CONSTRAINT [PK_Folder] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Folder_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_Folder_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_Folder_FolderType] FOREIGN KEY ([TypeId]) REFERENCES [sitebase].[FolderType] ([Id]),
	CONSTRAINT [FK_Folder_Folder] FOREIGN KEY ([ParentFolderId]) REFERENCES [sitebase].[Folder] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Folder_01] ON [sitebase].[Folder] ([TypeId], [AssociationId], [UserId], [ParentFolderId], [Name]) ON [PRIMARY]

-- content

CREATE TABLE [sitebase].[ContentGroupType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContentGroupType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	[CssClass] [nvarchar](100) NOT NULL,
	[DateFormat] [nvarchar](50) NOT NULL,
	[PageSize] [int] NULL,
	CONSTRAINT [PK_ContentGroupType] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ContentGroupType_Name] ON [sitebase].[ContentGroupType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ContentGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContentGroup_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[ContentGroupTypeId] [bigint] NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_ContentGroup_DisplayOrder] DEFAULT 0,
	CONSTRAINT [PK_ContentGroup] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ContentGroup_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_ContentGroup_ContentGroupType] FOREIGN KEY ([ContentGroupTypeId]) REFERENCES [sitebase].[ContentGroupType] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ContentGroup_01] ON [sitebase].[ContentGroup] ([AssociationId], [Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ContentEntry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContentEntry_ModificationCounter] DEFAULT 0,
	[LastModificationDate] [datetime] NOT NULL,
	[ContentGroupId] [bigint] NOT NULL,
	[ContentDate] [datetime] NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_ContentEntry_DisplayOrder] DEFAULT 0,
	[Title] [nvarchar](MAX) NULL,
	[Body] [nvarchar](MAX) NULL,
	CONSTRAINT [PK_ContentEntry] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ContentEntry_ContentGroup] FOREIGN KEY ([ContentGroupId]) REFERENCES [sitebase].[ContentGroup] ([Id])
) ON [PRIMARY]

-- secure files

CREATE TABLE [sitebase].[FileData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_FileData_ModificationCounter] DEFAULT 0,
	[Data] [image] NOT NULL,
	CONSTRAINT [PK_FileData] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [sitebase].[File](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_File_ModificationCounter] DEFAULT 0,
	[Created] [datetime] NOT NULL,
	[Deleted] [datetime] NULL,
	[AssociationId] [bigint] NOT NULL,
	[FolderId] [bigint] NULL,
	[Filename] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ContentType] [nvarchar](100) NULL,
	[DataCompressed] [bit] NOT NULL,
	[FileDataId] [bigint] NOT NULL,
	[CachedSize] [int] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	CONSTRAINT [PK_File] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_File_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_File_Folder] FOREIGN KEY ([FolderId]) REFERENCES [sitebase].[Folder] ([Id]),
	CONSTRAINT [FK_File_FileData] FOREIGN KEY ([FileDataId]) REFERENCES [sitebase].[FileData] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_File_01] ON [sitebase].[File] ([Deleted], [FolderId], [Filename]) ON [PRIMARY]

CREATE TABLE [sitebase].[File_Archive](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_FileArchive_ModificationCounter] DEFAULT 0,
	[RefId] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Archived] [datetime] NOT NULL,
	[Filename] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ContentType] [nvarchar](100) NULL,
	[DataCompressed] [bit] NOT NULL,
	[FileDataId] [bigint] NOT NULL,
	[CachedSize] [int] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	CONSTRAINT [PK_FileArchive] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_FileArchive_File] FOREIGN KEY ([RefId]) REFERENCES [sitebase].[File] ([Id]),
	CONSTRAINT [FK_FileArchive_FileData] FOREIGN KEY ([FileDataId]) REFERENCES [sitebase].[FileData] ([Id])
) ON [PRIMARY]

-- secure messaging

CREATE TABLE [sitebase].[MessageImportance](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MessageImportance_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_MessageImportance] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_MessageImportance_Name] ON [sitebase].[MessageImportance] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[NotificationPreference](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_NotificationPreference_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_NotificationPreference] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_NotificationPreference_Name] ON [sitebase].[NotificationPreference] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[MessageType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MessageType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_MessageType_Name] ON [sitebase].[MessageType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Message](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Message_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[SenderId] [bigint] NOT NULL,
	[SenderRoleId] [bigint] NULL,
	[SenderName] [nvarchar](200) NULL,
	[Subject] [nvarchar](MAX) NULL,
	[Content] [nvarchar](MAX) NULL,
	[DateSent] [datetime] NULL,
	[DateExpires] [datetime] NULL,
	[RelatedId] [bigint] NULL,
	[ReplyToId] [bigint] NULL,
	[Flagged] [bit] NOT NULL,
	[ReplyDisabled] [bit] NOT NULL,
	[ImportanceId] [bigint] NOT NULL,
	[Email] [bit] NOT NULL,
	[TypeId] [bigint] NOT NULL,
	CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Message_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_Message_User] FOREIGN KEY ([SenderId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_Message_Role] FOREIGN KEY ([SenderRoleId]) REFERENCES [sitebase].[Role] ([Id]),
	CONSTRAINT [FK_Message_RelatedId] FOREIGN KEY ([RelatedId]) REFERENCES [sitebase].[Message] ([Id]),
	CONSTRAINT [FK_Message_ReplyToId] FOREIGN KEY ([ReplyToId]) REFERENCES [sitebase].[Message] ([Id]),
	CONSTRAINT [FK_Message_MessageImportance] FOREIGN KEY ([ImportanceId]) REFERENCES [sitebase].[MessageImportance] ([Id]),
	CONSTRAINT [FK_Message_MessageType] FOREIGN KEY ([TypeId]) REFERENCES [sitebase].[MessageType] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[MessageRecipient](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MessageRecipient_ModificationCounter] DEFAULT 0,
	[MessageId] [bigint] NOT NULL,
	[RecipientTypeId] [bigint] NOT NULL,
	[RecipientId] [bigint] NOT NULL,
	[Name] [nvarchar] (200) NOT NULL,
	[FolderId] [bigint] NOT NULL,
	[Cc] [bit] NOT NULL CONSTRAINT [DF_MessageRecipient_Cc] DEFAULT 0,
	[Bcc] [bit] NOT NULL CONSTRAINT [DF_MessageRecipient_Bcc] DEFAULT 0,
	[DateAvailable] [datetime] NULL,
	[DateFirstRead] [datetime] NULL,
	[DateReplied] [datetime] NULL,
	[Flagged] [bit] NOT NULL CONSTRAINT [DF_MessageRecipient_Flagged] DEFAULT 0,
	[Read] [bit] NOT NULL CONSTRAINT [DF_MessageRecipient_Read] DEFAULT 0,
	CONSTRAINT [PK_MessageRecipient] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_MessageRecipient_Folder] FOREIGN KEY ([FolderId]) REFERENCES [sitebase].[Folder] ([Id]),
	CONSTRAINT [FK_MessageRecipient_Message] FOREIGN KEY ([MessageId]) REFERENCES [sitebase].[Message] ([Id]),
	CONSTRAINT [FK_MessageRecipient_EntityType] FOREIGN KEY ([RecipientTypeId]) REFERENCES [sitebase].[EntityType] ([Id])
) ON [PRIMARY]

CREATE UNIQUE INDEX [UK_MessageRecipient_01] ON [sitebase].[MessageRecipient]([MessageId],[RecipientTypeId],[RecipientId],[FolderId]) ON [PRIMARY]

CREATE TABLE [sitebase].[MessageAttachment](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MessageAttachment_ModificationCounter] DEFAULT 0,
	[MessageId] [bigint] NULL ,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_MessageAttachment_DateCreated] DEFAULT getdate(),
	[FileName] [nvarchar] (100) NOT NULL ,
	[ContentType] [nvarchar] (50) NOT NULL ,
	[Data] [image] NOT NULL,
	CONSTRAINT [PK_MessageAttachment] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_MessageAttachment_Message] FOREIGN KEY ([MessageId]) REFERENCES [sitebase].[Message] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[MessageTemplate] (
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MessageTemplate_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[Name] [nvarchar] (100) NOT NULL,
	[Subject] [nvarchar] (200) NOT NULL,
	[Content] [nvarchar](MAX) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_MessageTemplate_DateCreated] DEFAULT getdate(),
	CONSTRAINT [PK_MessageTemplate] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_MessageTemplate_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id])
) ON [PRIMARY]

CREATE UNIQUE INDEX [UK_MessageTemplate_01] ON [sitebase].[MessageTemplate]([AssociationId], [Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ContactType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContactType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ContactType_Name] ON [sitebase].[ContactType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[Contact] (
	[PersonId] [bigint] NOT NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_Contact_Enabled] DEFAULT 1,
	[ContactTypeId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[AssociationId] [bigint] NOT NULL,
	[RelationshipId] [bigint] NULL,
	[PhotoId] [bigint] NULL,
	[PhotoWidth] [int] NULL,
	[PhotoHeight] [int] NULL,
	CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([PersonId]) ON [PRIMARY],
	CONSTRAINT [FK_Contact_Person] FOREIGN KEY ([PersonId]) REFERENCES [sitebase].[Person] ([Id]),
	CONSTRAINT [FK_Contact_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [sitebase].[ContactType] ([Id]),
	CONSTRAINT [FK_Contact_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_Contact_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_Contact_Relationship] FOREIGN KEY ([RelationshipId]) REFERENCES [sitebase].[Relationship] ([Id]),
	CONSTRAINT [FK_Contact_File] FOREIGN KEY ([PhotoId]) REFERENCES [sitebase].[File] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[ContactCommentType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContactCommentType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	[Flagged] [bit] NOT NULL CONSTRAINT [DF_ContactCommentType_Flagged] DEFAULT 0,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_ContactCommentType_DisplayOrder] DEFAULT 999,
	CONSTRAINT [PK_ContactCommentType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_ContactCommentType_Name] ON [sitebase].[ContactCommentType] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[ContactComment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContactComment_ModificationCounter] DEFAULT 0,
	[ContactId] [bigint] NOT NULL,
	[CommentTypeId] [bigint] NOT NULL,
	[Text] [nvarchar](MAX) NULL,
	[Date] [datetime] NOT NULL,
	CONSTRAINT [PK_ContactComment] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_ContactComment_Contact] FOREIGN KEY ([ContactId]) REFERENCES [sitebase].[Contact] ([PersonId]),
	CONSTRAINT [FK_ContactComment_ContactCommentType] FOREIGN KEY ([CommentTypeId]) REFERENCES [sitebase].[ContactCommentType] ([Id])
) ON [PRIMARY]

-- queued email

CREATE TABLE [sitebase].[QueuedEmail](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_MailQueue_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NULL,
	[MessageId] [bigint] NULL,
	[TemplateId] [bigint] NULL,
	[Priority] [int] NULL,
	[SenderEmail] [nvarchar] (200) NULL,
	[Subject] [nvarchar](MAX) NULL,
	[Body] [nvarchar](MAX) NULL,
	[SendDate] [datetime] NOT NULL,
	[DateProcessed] [datetime] NULL,
	[DateSent] [datetime] NULL,
	[ErrorMessage] [nvarchar](MAX) NULL,
	CONSTRAINT [PK_QueuedEmail] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_QueuedEmail_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_QueuedEmail_Message] FOREIGN KEY ([MessageId]) REFERENCES [sitebase].[Message] ([Id]),
	CONSTRAINT [FK_QueuedEmail_ModuleSettingDefinition] FOREIGN KEY ([TemplateId]) REFERENCES [sitebase].[ModuleSettingDefinition] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[QueuedEmailRecipient](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_QueuedEmailRecipient_ModificationCounter] DEFAULT 0,
	[QueuedEmailId] [bigint] NOT NULL,
	[UserId] [bigint] NULL,
	[PersonId] [bigint] NULL,
	[Email] [nvarchar] (200) NOT NULL,
	[Cc] [bit] NOT NULL CONSTRAINT [DF_QueuedEmailRecipient_Cc] DEFAULT 0,
	[Bcc] [bit] NOT NULL CONSTRAINT [DF_QueuedEmailRecipient_Bcc] DEFAULT 0,
	CONSTRAINT [PK_QueuedEmailRecipient] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_QueuedEmailRecipient_QueuedEmail] FOREIGN KEY ([QueuedEmailId]) REFERENCES [sitebase].[QueuedEmail] ([Id]),
	CONSTRAINT [FK_QueuedEmailRecipient_User] FOREIGN KEY ([UserId]) REFERENCES [sitebase].[User] ([Id]),
	CONSTRAINT [FK_QueuedEmailRecipient_Person] FOREIGN KEY ([PersonId]) REFERENCES [sitebase].[Person] ([Id])
) ON [PRIMARY]

CREATE TABLE [sitebase].[QueuedEmailAttachment](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_QueuedEmailAttachment_ModificationCounter] DEFAULT 0,
	[QueuedEmailId] [bigint] NULL,
	[FileName] [nvarchar] (100) NOT NULL,
	[ContentType] [nvarchar] (50) NOT NULL,
	[Data] [image] NOT NULL,
	CONSTRAINT [PK_QueuedEmailAttachment] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_QueuedEmailAttachment_QueuedEmail] FOREIGN KEY ([QueuedEmailId]) REFERENCES [sitebase].[QueuedEmail] ([Id])
) ON [PRIMARY]

-- navigation

CREATE TABLE [sitebase].[Navigation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Navigation_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_Navigation] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [UK_Navigation_Name] ON [sitebase].[Navigation] ([Name]) ON [PRIMARY]

CREATE TABLE [sitebase].[NavigationItem](
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_NavigationItem_ModificationCounter] DEFAULT 0,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_NavigationItem_Enabled] DEFAULT 1,
	[AssociationId] [bigint] NULL,
	[NavigationId] [bigint] NOT NULL,
	[ParentId] [bigint] NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_NavigationItem_DisplayOrder] DEFAULT 1,
	[Text] [nvarchar] (100) NOT NULL,
	[ModuleId] [int] NULL,
	[Url] [nvarchar] (100) NULL,
	--[SystemItem] [bit] NOT NULL CONSTRAINT [DF_NavigationItem_SystemItem] DEFAULT 0,
	--[AuthenticationFlag] [bit] NULL,
	[ImageUrl] [nvarchar] (100) NULL,
	CONSTRAINT [PK_NavigationItem] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_NavigationItem_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_NavigationItem_Navigation] FOREIGN KEY ([NavigationId]) REFERENCES [sitebase].[Navigation] ([Id]),
	CONSTRAINT [FK_NavigationItem_NavigationItem] FOREIGN KEY ([ParentId]) REFERENCES [sitebase].[NavigationItem] ([Id])
) ON [PRIMARY]

-- localization

CREATE TABLE [sitebase].[Resource](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_Resource_ModificationCounter] DEFAULT 0,
	[LanguageId] [bigint] NOT NULL,
	[Type] [nvarchar](100) NULL,
	[Key] [nvarchar](100) NOT NULL,
	[Property] [nvarchar](100) NULL,
	[Value] [nvarchar](MAX) NOT NULL,
	[FileId] [bigint] NULL,
	CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_Resource_Language] FOREIGN KEY ([LanguageId]) REFERENCES [sitebase].[Language] ([Id]),
	CONSTRAINT [FK_Resource_File] FOREIGN KEY ([FileId]) REFERENCES [sitebase].[File] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_Resource_01] ON [sitebase].[Resource] ([LanguageId],[Type],[Key],[Property]) ON [PRIMARY]

-- home

CREATE TABLE [sitebase].[RoleHome](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_RoleHome_ModificationCounter] DEFAULT 0,
	[AssociationId] [bigint] NOT NULL,
	[EntityTypeId] [bigint] NOT NULL,
	[EntityId] [bigint] NOT NULL,
	[Url] [nvarchar] (100) NOT NULL,
	[Redirect] [bit] NOT NULL CONSTRAINT [DF_RoleHome_Redirect] DEFAULT 0,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_RoleHome_DisplayOrder] DEFAULT 1,
	CONSTRAINT [PK_RoleHome] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
	CONSTRAINT [FK_RoleHome_Association] FOREIGN KEY ([AssociationId]) REFERENCES [sitebase].[Association] ([Id]),
	CONSTRAINT [FK_RoleHome_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [sitebase].[EntityType] ([Id])
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_RoleHome_01] ON [sitebase].[RoleHome] ([AssociationId],[EntityTypeId],[EntityId]) ON [PRIMARY]
