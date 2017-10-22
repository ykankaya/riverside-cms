SET NOCOUNT ON

UPDATE
	element.PageHeader
SET
	element.PageHeader.PageTenantId    = @PageTenantId,
	element.PageHeader.PageId          = @PageId,
	element.PageHeader.ShowName        = @ShowName,
	element.PageHeader.ShowDescription = @ShowDescription,
	element.PageHeader.ShowImage	   = @ShowImage,
	element.PageHeader.ShowCreated     = @ShowCreated,
	element.PageHeader.ShowUpdated     = @ShowUpdated,
	element.PageHeader.ShowOccurred    = @ShowOccurred,
	element.PageHeader.ShowBreadcrumbs = @ShowBreadcrumbs
WHERE
	element.PageHeader.TenantId = @TenantId AND
	element.PageHeader.ElementId = @ElementId