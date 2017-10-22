CREATE TABLE [element].[Contact] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[DisplayName] [nvarchar](256) NULL,
	[Preamble] [nvarchar](max) NULL,
	[Address][nvarchar](256) NULL,
	[Email][nvarchar](256) NULL,
	[FacebookUsername][nvarchar](50) NULL,
	[InstagramUsername][nvarchar](30) NULL,
	[LinkedInCompanyUsername][nvarchar](50) NULL,
	[LinkedInPersonalUsername][nvarchar](50) NULL,
	[TelephoneNumber1][nvarchar](50) NULL,
	[TelephoneNumber2][nvarchar](50) NULL,
	[TwitterUsername][nvarchar](15) NULL,
	[YouTubeChannelId][nvarchar](50) NULL
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Contact_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)