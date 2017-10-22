CREATE TYPE [element].[FormFieldTableType] AS TABLE
(
	[FormFieldId] [bigint] NULL,
	[SortOrder] [int] NOT NULL,
	[FormFieldType] [int] NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Required] [bit] NOT NULL
)