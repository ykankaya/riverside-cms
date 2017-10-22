CREATE TABLE [cms].[Tag] (
	[TenantId] [bigint] NOT NULL,
	[TagId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TagId] ASC),
 CONSTRAINT [FK_Tag_Web] FOREIGN KEY([TenantId]) REFERENCES [cms].[Web] ([TenantId])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Tag_TenantId_Name] ON [cms].[Tag] ([TenantId] ASC, [Name] ASC)