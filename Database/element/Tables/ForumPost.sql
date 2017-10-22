CREATE TABLE [element].[ForumPost] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[ThreadId] [bigint] NOT NULL,
	[PostId] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentPostId] [bigint] NULL,
	[UserId] [bigint] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_ForumPost] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [ThreadId] ASC, [PostId] ASC),
 CONSTRAINT [FK_ForumPost_ForumThread] FOREIGN KEY([TenantId], [ElementId], [ThreadId]) REFERENCES [element].[ForumThread] ([TenantId], [ElementId], [ThreadId]),
 CONSTRAINT [FK_ForumPost_User] FOREIGN KEY([TenantId], [UserId]) REFERENCES [cms].[User] ([TenantId], [UserId])
)
GO

CREATE NONCLUSTERED INDEX [IX_ForumPost_TenantId_ElementId_ThreadId_SortOrder] ON [element].[ForumPost] ([TenantId] ASC, [ElementId] ASC, [ThreadId] ASC, [SortOrder] ASC)