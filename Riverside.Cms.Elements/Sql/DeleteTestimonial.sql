SET NOCOUNT ON

DELETE
	element.TestimonialComment
WHERE
	element.TestimonialComment.TenantId  = @TenantId AND
	element.TestimonialComment.ElementId = @ElementId

DELETE
	element.Testimonial
WHERE
	element.Testimonial.TenantId  = @TenantId AND
	element.Testimonial.ElementId = @ElementId