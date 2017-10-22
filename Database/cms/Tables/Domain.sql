CREATE TABLE [cms].[Domain] (
    [TenantId] [bigint] NOT NULL,
    [DomainId] [bigint] IDENTITY (1, 1) NOT NULL,
    [Url] [nvarchar](256) NOT NULL,
    [RedirectUrl] [nvarchar](256) NULL,
 CONSTRAINT [PK_Domain] PRIMARY KEY CLUSTERED ([TenantId] ASC, [DomainId] ASC),
 CONSTRAINT [FK_Domain_Web] FOREIGN KEY ([TenantId]) REFERENCES [cms].[Web] ([TenantId])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Domain_Url]
    ON [cms].[Domain]([Url] ASC)