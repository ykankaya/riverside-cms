CREATE TABLE [element].[ForumThread] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[ThreadId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Notify] [bit] NOT NULL,
	[Views] [int] NOT NULL,
	[Replies] [int] NOT NULL,
	[LastPostId] [bigint] NULL,
	[LastMessageCreated] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_ForumThread] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [ThreadId] ASC),
 CONSTRAINT [FK_ForumThread_Forum] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[Forum] ([TenantId], [ElementId]),
 CONSTRAINT [FK_ForumThread_User] FOREIGN KEY([TenantId], [UserId]) REFERENCES [cms].[User] ([TenantId], [UserId])
)
GO

CREATE NONCLUSTERED INDEX [IX_ForumThread_TenantId_ElementId_LastMessageCreated] ON [element].[ForumThread] ([TenantId] ASC, [ElementId] ASC, [LastMessageCreated] ASC)