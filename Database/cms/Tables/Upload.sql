CREATE TABLE [cms].[Upload] (
	[TenantId] [bigint] NOT NULL,
	[UploadId] [bigint] IDENTITY(1,1) NOT NULL,
	[UploadType] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Size] [int] NOT NULL,
	[Committed] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_Upload] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UploadId] ASC),
 CONSTRAINT [FK_Upload_Tenant] FOREIGN KEY([TenantId]) REFERENCES [cms].[Tenant] ([TenantId])
)