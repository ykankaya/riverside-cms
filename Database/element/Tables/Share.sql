CREATE TABLE [element].[Share] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[DisplayName] [nvarchar](256) NULL,
	[ShareOnDigg] [bit] NOT NULL,
	[ShareOnFacebook] [bit] NOT NULL,
	[ShareOnGoogle] [bit] NOT NULL,
	[ShareOnLinkedIn] [bit] NOT NULL,
	[ShareOnPinterest] [bit] NOT NULL,
	[ShareOnReddit] [bit] NOT NULL,
	[ShareOnStumbleUpon] [bit] NOT NULL,
	[ShareOnTumblr] [bit] NOT NULL,
	[ShareOnTwitter] [bit] NOT NULL,
 CONSTRAINT [PK_Share] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Share_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)