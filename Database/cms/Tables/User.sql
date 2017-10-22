CREATE TABLE [cms].[User](
	[TenantId] [bigint] NOT NULL,
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Confirmed] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[LockedOut] [bit] NOT NULL,
	[PasswordSaltedHash] [varchar](344) NULL,
	[PasswordSalt] [varchar](24) NULL,
	[PasswordChanged] [datetime] NULL,
	[LastPasswordFailure] [datetime] NULL,
	[PasswordFailures] [int] NOT NULL,
	[ResetPasswordTokenValue] [uniqueidentifier] NULL,
	[ResetPasswordTokenExpiry] [datetime] NULL,
	[ConfirmTokenValue] [uniqueidentifier] NULL,
	[ConfirmTokenExpiry] [datetime] NULL,
	[ImageTenantId] [bigint] NULL,
	[ThumbnailImageUploadId] [bigint] NULL,
	[PreviewImageUploadId] [bigint] NULL,
	[ImageUploadId] [bigint] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserId] ASC),
 CONSTRAINT [FK_User_Web] FOREIGN KEY([TenantId]) REFERENCES [cms].[Web] ([TenantId]),
 CONSTRAINT [FK_User_ThumbnailImage] FOREIGN KEY([ImageTenantId], [ThumbnailImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_User_PreviewImage] FOREIGN KEY([ImageTenantId], [PreviewImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_User_Image] FOREIGN KEY([ImageTenantId], [ImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_TenantId_Alias] ON [cms].[User] ([TenantId] ASC, [Alias] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_TenantId_Email] ON [cms].[User] ([TenantId] ASC, [Email] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_TenantId_ConfirmTokenValue] ON [cms].[User] ([TenantId] ASC, [ConfirmTokenValue] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_TenantId_ResetPasswordTokenValue] ON [cms].[User] ([TenantId] ASC, [ResetPasswordTokenValue] ASC)