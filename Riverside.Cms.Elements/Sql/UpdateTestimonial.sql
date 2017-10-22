SET NOCOUNT ON

UPDATE
	element.Testimonial
SET
	element.Testimonial.DisplayName = @DisplayName,
	element.Testimonial.Preamble = @Preamble
WHERE
	element.Testimonial.TenantId  = @TenantId AND
	element.Testimonial.ElementId = @ElementId

/*----- Update existing testimonial comments -----*/

UPDATE
	element.TestimonialComment
SET
	element.TestimonialComment.SortOrder	= TestimonialComments.SortOrder,
	element.TestimonialComment.Comment		= TestimonialComments.Comment,
	element.TestimonialComment.Author		= TestimonialComments.Author,
	element.TestimonialComment.AuthorTitle	= TestimonialComments.AuthorTitle,
	element.TestimonialComment.CommentDate	= TestimonialComments.CommentDate
FROM
	element.TestimonialComment
INNER JOIN
	@TestimonialComments TestimonialComments
ON
	element.TestimonialComment.TenantId				= @TenantId AND
	element.TestimonialComment.ElementId			= @ElementId AND
	element.TestimonialComment.TestimonialCommentId = TestimonialComments.TestimonialCommentId

/*----- Delete testimonial comments that are no longer needed -----*/

DELETE
	element.TestimonialComment
FROM
	element.TestimonialComment
LEFT JOIN
	@TestimonialComments TestimonialComments
ON
	element.TestimonialComment.TestimonialCommentId = TestimonialComments.TestimonialCommentId
WHERE
	element.TestimonialComment.TenantId  = @TenantId AND
	element.TestimonialComment.ElementId = @ElementId AND
	TestimonialComments.TestimonialCommentId IS NULL
	
/*----- Create new testimonial comments -----*/

INSERT INTO
	element.TestimonialComment (TenantId, ElementId, SortOrder, Comment, Author, AuthorTitle, CommentDate)
SELECT
	@TenantId, @ElementId, TestimonialComments.SortOrder, TestimonialComments.Comment, TestimonialComments.Author, TestimonialComments.AuthorTitle, TestimonialComments.CommentDate
FROM
	@TestimonialComments TestimonialComments
WHERE
	TestimonialComments.TestimonialCommentId IS NULL