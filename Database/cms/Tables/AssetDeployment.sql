CREATE TABLE [cms].[AssetDeployment](
	[TenantId] [bigint] NOT NULL,
	[Hostname] [nvarchar](100) NOT NULL,
	[Deployed] [datetime] NOT NULL,
 CONSTRAINT [PK_AssetDeployment] PRIMARY KEY CLUSTERED ([TenantId] ASC, [Hostname] ASC),
 CONSTRAINT [FK_AssetDeployment_Web] FOREIGN KEY([TenantId]) REFERENCES [cms].[Web] ([TenantId])
)