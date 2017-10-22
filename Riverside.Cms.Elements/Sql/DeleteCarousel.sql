SET NOCOUNT ON

DELETE
	element.CarouselSlide
WHERE
	element.CarouselSlide.TenantId  = @TenantId AND
	element.CarouselSlide.ElementId = @ElementId