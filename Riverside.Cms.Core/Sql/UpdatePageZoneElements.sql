SET NOCOUNT ON

/*----- Update existing page zone elements -----*/

UPDATE
	cms.PageZoneElement
SET
	cms.PageZoneElement.SortOrder = PageZoneElements.SortOrder
FROM
	cms.PageZoneElement
INNER JOIN
	@PageZoneElements PageZoneElements
ON
	cms.PageZoneElement.TenantId		  = @TenantId AND
	cms.PageZoneElement.PageId			  = @PageId AND
	cms.PageZoneElement.PageZoneId		  = @PageZoneId AND
	cms.PageZoneElement.PageZoneElementId = PageZoneElements.PageZoneElementId

/*----- Delete page zone elements that are no longer needed -----*/

DELETE
	cms.PageZoneElement
FROM
	cms.PageZoneElement
LEFT JOIN
	@PageZoneElements PageZoneElements
ON
	cms.PageZoneElement.PageZoneElementId = PageZoneElements.PageZoneElementId
WHERE
	cms.PageZoneElement.TenantId		  = @TenantId AND
	cms.PageZoneElement.PageId			  = @PageId AND
	cms.PageZoneElement.PageZoneId		  = @PageZoneId AND
	PageZoneElements.PageZoneElementId IS NULL
	
/*----- Create new page zone elements -----*/

INSERT INTO
	cms.PageZoneElement (TenantId, PageId, PageZoneId, SortOrder, ElementId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId)
SELECT
	@TenantId, @PageId, @PageZoneId, PageZoneElements.SortOrder, PageZoneElements.ElementId, NULL, NULL, NULL
FROM
	@PageZoneElements PageZoneElements
WHERE
	PageZoneElements.PageZoneElementId IS NULL