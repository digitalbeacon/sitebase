CREATE TABLE [sitebase].[ContactType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModificationCounter] [bigint] NOT NULL CONSTRAINT [DF_ContactType_ModificationCounter] DEFAULT 0,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UK_ContactType_Name] ON [sitebase].[ContactType] ([Name]) ON [PRIMARY]

ALTER TABLE [sitebase].[Contact]
	ADD [ContactTypeId] [bigint] NULL

ALTER TABLE [sitebase].[Contact]
	ADD CONSTRAINT [FK_Contact_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [sitebase].[ContactType] ([Id])

SET IDENTITY_INSERT [sitebase].[ContactType] ON
INSERT INTO [sitebase].[ContactType] ([Id],[ModificationCounter],[Name])VALUES(1,0,'Default')
SET IDENTITY_INSERT [sitebase].[ContactType] OFF

SET IDENTITY_INSERT [sitebase].[ContactCommentType] ON
INSERT INTO [sitebase].[ContactCommentType] ([Id],[ModificationCounter],[Name],[Flagged],[DisplayOrder])VALUES(1,0,'Note',0,1)
SET IDENTITY_INSERT [sitebase].[ContactCommentType] OFF