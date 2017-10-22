CREATE TABLE [cms].[TagPage](
	[TenantId] [bigint] NOT NULL,
	[TagId] [bigint] NOT NULL,
	[PageId] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_TagPage] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TagId] ASC, [PageId] ASC),
 CONSTRAINT [FK_TagPage_Tag] FOREIGN KEY([TenantId], [TagId]) REFERENCES [cms].[Tag] ([TenantId], [TagId]),
 CONSTRAINT [FK_TagPage_Page] FOREIGN KEY([TenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)