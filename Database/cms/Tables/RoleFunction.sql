CREATE TABLE [cms].[RoleFunction](
    [RoleId] [bigint] NOT NULL,
    [FunctionId] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleFunction] PRIMARY KEY CLUSTERED ([RoleId] ASC, [FunctionId] ASC),
 CONSTRAINT [FK_RoleFunction_Role] FOREIGN KEY([RoleId]) REFERENCES [cms].[Role] ([RoleId]),
 CONSTRAINT [FK_RoleFunction_Function] FOREIGN KEY([FunctionId]) REFERENCES [cms].[Function] ([FunctionId])
)