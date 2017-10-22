SET NOCOUNT ON

INSERT INTO
	element.Testimonial (TenantId, ElementId, DisplayName, Preamble)
VALUES
	(@TenantId, @ElementId, @DisplayName, @Preamble)

INSERT INTO
	element.TestimonialComment (TenantId, ElementId, SortOrder, Comment, Author, AuthorTitle, CommentDate)
SELECT
	@TenantId, @ElementId, TestimonialComments.SortOrder, TestimonialComments.Comment, TestimonialComments.Author, TestimonialComments.AuthorTitle, TestimonialComments.CommentDate
FROM
	@TestimonialComments TestimonialComments