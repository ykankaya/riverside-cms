CREATE TABLE [element].[Forum] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[ThreadCount] [int] NOT NULL,
	[PostCount] [int] NOT NULL,
	[OwnerTenantId] [bigint] NULL,
	[OwnerUserId] [bigint] NULL,
	[OwnerOnlyThreads] [bit] NOT NULL,
 CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Forum_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_Forum_User] FOREIGN KEY([OwnerTenantId], [OwnerUserId]) REFERENCES [cms].[User] ([TenantId], [UserId])
)