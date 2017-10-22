SET NOCOUNT ON

SELECT
	element.Testimonial.TenantId,
	element.Testimonial.ElementId,
	element.Testimonial.DisplayName,
	element.Testimonial.Preamble
FROM
	element.Testimonial
WHERE
	element.Testimonial.TenantId = @TenantId AND
	element.Testimonial.ElementId = @ElementId

SELECT
	element.TestimonialComment.TenantId,
	element.TestimonialComment.ElementId,
	element.TestimonialComment.TestimonialCommentId,
	element.TestimonialComment.SortOrder,
	element.TestimonialComment.Comment,
	element.TestimonialComment.Author,
	element.TestimonialComment.AuthorTitle,
	element.TestimonialComment.CommentDate
FROM
	element.TestimonialComment
WHERE
	element.TestimonialComment.TenantId = @TenantId AND
	element.TestimonialComment.ElementId = @ElementId
ORDER BY
	element.TestimonialComment.SortOrder