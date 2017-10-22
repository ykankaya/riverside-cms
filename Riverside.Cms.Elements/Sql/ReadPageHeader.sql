SET NOCOUNT ON

SELECT
	element.PageHeader.TenantId,
	element.PageHeader.ElementId,
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
	element.PageHeader.TenantId = @TenantId AND
	element.PageHeader.ElementId = @ElementId