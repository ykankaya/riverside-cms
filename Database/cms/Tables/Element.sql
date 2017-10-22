CREATE TABLE [cms].[Element] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] IDENTITY(1,1) NOT NULL,
	[ElementTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Element] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Element_Tenant] FOREIGN KEY([TenantId]) REFERENCES [cms].[Tenant] ([TenantId]),
 CONSTRAINT [FK_Element_ElementType] FOREIGN KEY([ElementTypeId]) REFERENCES [cms].[ElementType] ([ElementTypeId])
)