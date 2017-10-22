CREATE TABLE [element].[FormField] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[FormFieldId] [bigint] IDENTITY(1,1) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[FormFieldType] [int] NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Required] [bit] NOT NULL,
 CONSTRAINT [PK_FormField] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [FormFieldId] ASC),
 CONSTRAINT [FK_FormField_Form] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[Form] ([TenantId], [ElementId])
)