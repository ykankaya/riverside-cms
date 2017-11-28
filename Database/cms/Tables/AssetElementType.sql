CREATE TABLE [cms].[AssetElementType] (
	[TenantId] [bigint] NOT NULL,
	[ElementTypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AssetElementType] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementTypeId] ASC),
 CONSTRAINT [FK_AssetElementType_Tenant] FOREIGN KEY([TenantId]) REFERENCES [cms].[Tenant] ([TenantId]),
 CONSTRAINT [FK_AssetElementType_ElementType] FOREIGN KEY([ElementTypeId]) REFERENCES [cms].[ElementType] ([ElementTypeId])
)