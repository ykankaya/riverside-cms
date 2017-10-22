CREATE TABLE [element].[Footer] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[ShowLoggedOnUserOptions] [bit] NOT NULL,
	[ShowLoggedOffUserOptions] [bit] NOT NULL,
 CONSTRAINT [PK_Footer] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Footer_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)