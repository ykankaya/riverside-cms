CREATE TABLE [element].[TestimonialComment] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[TestimonialCommentId] [bigint] IDENTITY(1,1) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[AuthorTitle] [nvarchar](100) NOT NULL,
	[CommentDate] [nvarchar](30) NOT NULL
 CONSTRAINT [PK_TestimonialComment] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [TestimonialCommentId] ASC),
 CONSTRAINT [FK_TestimonialComment_Testimonial] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[Testimonial] ([TenantId], [ElementId])
)