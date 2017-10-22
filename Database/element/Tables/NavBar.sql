CREATE TABLE [element].[NavBar] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ShowLoggedOnUserOptions] [bit] NOT NULL,
	[ShowLoggedOffUserOptions] [bit] NOT NULL,
 CONSTRAINT [PK_NavBar] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_NavBar_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)