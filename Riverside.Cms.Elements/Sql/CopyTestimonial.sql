SET NOCOUNT ON

INSERT INTO
	element.Testimonial (TenantId, ElementId, DisplayName, Preamble)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Testimonial.DisplayName,
	element.Testimonial.Preamble
FROM
	element.Testimonial
WHERE
	element.Testimonial.TenantId  = @SourceTenantId AND
	element.Testimonial.ElementId = @SourceElementId

INSERT INTO
	element.TestimonialComment (TenantId, ElementId, SortOrder, Comment, Author, AuthorTitle, CommentDate)
SELECT
	@DestTenantId,
	@DestElementId,
	element.TestimonialComment.SortOrder,
	element.TestimonialComment.Comment,
	element.TestimonialComment.Author,
	element.TestimonialComment.AuthorTitle,
	element.TestimonialComment.CommentDate
FROM
	element.TestimonialComment
WHERE
	element.TestimonialComment.TenantId	= @SourceTenantId AND
	element.TestimonialComment.ElementId = @SourceElementId