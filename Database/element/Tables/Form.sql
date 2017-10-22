CREATE TABLE [element].[Form] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[RecipientEmail] [nvarchar](max) NOT NULL,
	[SubmitButtonLabel] [nvarchar](100) NOT NULL,
	[SubmittedMessage] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Form] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Form_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)