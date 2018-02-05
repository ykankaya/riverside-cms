CREATE TABLE [dbo].[TenantPlace]
(
	[TenantId] [bigint] NOT NULL,
	[PlaceId] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([TenantId] ASC)
)
