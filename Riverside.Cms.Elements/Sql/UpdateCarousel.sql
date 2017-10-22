SET NOCOUNT ON

/*----- Update existing carousel slides -----*/

UPDATE
	element.CarouselSlide
SET
	element.CarouselSlide.ImageTenantId			 = CarouselSlides.ImageTenantId,
	element.CarouselSlide.ThumbnailImageUploadId = CarouselSlides.ThumbnailImageUploadId,
	element.CarouselSlide.PreviewImageUploadId   = CarouselSlides.PreviewImageUploadId,
	element.CarouselSlide.ImageUploadId			 = CarouselSlides.ImageUploadId,
	element.CarouselSlide.[Name]				 = CarouselSlides.[Name],
	element.CarouselSlide.[Description]			 = CarouselSlides.[Description],
	element.CarouselSlide.PageTenantId			 = CarouselSlides.PageTenantId,
	element.CarouselSlide.PageId				 = CarouselSlides.PageId,
	element.CarouselSlide.PageText				 = CarouselSlides.PageText,
	element.CarouselSlide.SortOrder				 = CarouselSlides.SortOrder
FROM
	element.CarouselSlide
INNER JOIN
	@CarouselSlides CarouselSlides
ON
	element.CarouselSlide.TenantId		  = @TenantId AND
	element.CarouselSlide.ElementId		  = @ElementId AND
	element.CarouselSlide.CarouselSlideId = CarouselSlides.CarouselSlideId

/*----- Delete carousel slides that are no longer needed -----*/

DELETE
	element.CarouselSlide
FROM
	element.CarouselSlide
LEFT JOIN
	@CarouselSlides CarouselSlides
ON
	element.CarouselSlide.CarouselSlideId = CarouselSlides.CarouselSlideId
WHERE
	element.CarouselSlide.TenantId  = @TenantId AND
	element.CarouselSlide.ElementId = @ElementId AND
	CarouselSlides.CarouselSlideId IS NULL
	
/*----- Create new carousel slides -----*/

INSERT INTO
	element.CarouselSlide (TenantId, ElementId, ImageTenantId, ThumbnailImageUploadId, PreviewImageUploadId, ImageUploadId, Name, [Description], PageTenantId, PageId, PageText, SortOrder)
SELECT
	@TenantId, @ElementId, CarouselSlides.ImageTenantId, CarouselSlides.ThumbnailImageUploadId, CarouselSlides.PreviewImageUploadId, CarouselSlides.ImageUploadId, CarouselSlides.Name,
	CarouselSlides.[Description], CarouselSlides.PageTenantId, CarouselSlides.PageId, CarouselSlides.PageText, CarouselSlides.SortOrder
FROM
	@CarouselSlides CarouselSlides
WHERE
	CarouselSlides.CarouselSlideId IS NULL