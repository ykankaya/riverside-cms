SET NOCOUNT ON

SELECT
	Pages.PageId,
	element.Forum.ThreadCount,
	element.Forum.PostCount
FROM
	@Pages Pages
INNER JOIN
	cms.PageZoneElement
ON
	cms.PageZoneElement.PageId = [Pages].PageId
INNER JOIN
	element.Forum
ON
	cms.PageZoneElement.TenantId  = element.Forum.TenantId AND
	cms.PageZoneElement.ElementId = element.Forum.ElementId
WHERE
	cms.PageZoneElement.TenantId = @TenantId