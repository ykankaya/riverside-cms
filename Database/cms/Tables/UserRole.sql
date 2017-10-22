CREATE TABLE [cms].[UserRole](
    [TenantId] [bigint] NOT NULL,
    [UserId] [bigint] NOT NULL,
    [RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserId] ASC, [RoleId] ASC),
 CONSTRAINT [FK_UserRole_User] FOREIGN KEY([TenantId], [UserId]) REFERENCES [cms].[User] ([TenantId], [UserId]),
 CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId]) REFERENCES [cms].[Role] ([RoleId])
)