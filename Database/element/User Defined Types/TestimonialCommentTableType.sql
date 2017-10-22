CREATE TYPE [element].[TestimonialCommentTableType] AS TABLE
(
	[TestimonialCommentId] [bigint] NULL,
	[SortOrder] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[AuthorTitle] [nvarchar](100) NOT NULL,
	[CommentDate] [nvarchar](30) NOT NULL
)