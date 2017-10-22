SET NOCOUNT ON

INSERT INTO
	element.PageHeader (TenantId, ElementId, PageTenantId, PageId, ShowName, ShowDescription, ShowImage, ShowCreated, ShowUpdated, ShowOccurred, ShowBreadcrumbs)
SELECT
	@DestTenantId,
	@DestElementId,
	element.PageHeader.PageTenantId,
	element.PageHeader.PageId,
	element.PageHeader.ShowName,
	element.PageHeader.ShowDescription,
	element.PageHeader.ShowImage,
	element.PageHeader.ShowCreated,
	element.PageHeader.ShowUpdated,
	element.PageHeader.ShowOccurred,
	element.PageHeader.ShowBreadcrumbs
FROM
	element.PageHeader
WHERE
	element.PageHeader.TenantId  = @SourceTenantId AND
	element.PageHeader.ElementId = @SourceElementId