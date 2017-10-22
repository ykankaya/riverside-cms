SET NOCOUNT ON

SELECT
	element.CarouselSlide.TenantId,
	element.CarouselSlide.ElementId,
	element.CarouselSlide.CarouselSlideId,
	element.CarouselSlide.ImageTenantId,
	element.CarouselSlide.ThumbnailImageUploadId,
	element.CarouselSlide.PreviewImageUploadId,
	element.CarouselSlide.ImageUploadId,
	element.CarouselSlide.[Name],
	element.CarouselSlide.[Description],
	element.CarouselSlide.PageTenantId,
	element.CarouselSlide.PageId,
	element.CarouselSlide.PageText,
	element.CarouselSlide.SortOrder
FROM
	element.CarouselSlide
WHERE
	element.CarouselSlide.TenantId = @TenantId AND
	element.CarouselSlide.ElementId = @ElementId AND
	element.CarouselSlide.CarouselSlideId = @CarouselSlideId