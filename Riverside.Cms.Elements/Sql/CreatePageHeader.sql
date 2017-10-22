SET NOCOUNT ON

INSERT INTO
	element.PageHeader (TenantId, ElementId, PageTenantId, PageId, ShowName, ShowDescription, ShowImage, ShowCreated, ShowUpdated, ShowOccurred, ShowBreadcrumbs)
VALUES
	(@TenantId, @ElementId, @PageTenantId, @PageId, @ShowName, @ShowDescription, @ShowImage, @ShowCreated, @ShowUpdated, @ShowOccurred, @ShowBreadcrumbs)