CREATE TABLE [dbo].[Blob] (
	[TenantId] [bigint] NOT NULL,
	[BlobId] [bigint] IDENTITY(1,1) NOT NULL,
	[Size] [int] NOT NULL,
	[ContentType] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[Folder1] [nvarchar](50) NULL,
	[Folder2] [nvarchar](50) NULL,
	[Folder3] [nvarchar](50) NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_Blob] PRIMARY KEY CLUSTERED ([TenantId] ASC, [BlobId] ASC)
)