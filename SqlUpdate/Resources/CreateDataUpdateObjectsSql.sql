CREATE TABLE [SqlUpdate](
	[Version] [nvarchar](20) NOT NULL,
	[PatchNumber] [int] NOT NULL,
	[Applied] [datetime] NOT NULL CONSTRAINT [DF_SqlUpdate_Applied] DEFAULT getdate(),
	[Module] [nvarchar](20) NOT NULL,
	CONSTRAINT [PK_DataUpdate] PRIMARY KEY CLUSTERED ([Module] ASC, [Version] ASC, [PatchNumber] ASC) ON [PRIMARY]
) ON [PRIMARY]