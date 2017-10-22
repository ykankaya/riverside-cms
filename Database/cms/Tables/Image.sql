CREATE TABLE [cms].[Image](
	[TenantId] [bigint] NOT NULL,
	[UploadId] [bigint] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UploadId] ASC),
 CONSTRAINT [FK_Image_Upload] FOREIGN KEY([TenantId], [UploadId]) REFERENCES [cms].[Upload] ([TenantId], [UploadId])
)