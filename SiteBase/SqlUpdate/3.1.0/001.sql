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