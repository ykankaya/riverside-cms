INSERT INTO
	element.NavBar (TenantId, ElementId, [Name], ShowLoggedOnUserOptions, ShowLoggedOffUserOptions)
SELECT
	@DestTenantId,
	@DestElementId,
	element.NavBar.[Name],
	element.NavBar.ShowLoggedOnUserOptions,
	element.NavBar.ShowLoggedOffUserOptions
FROM
	element.NavBar
WHERE
	element.NavBar.TenantId  = @SourceTenantId AND
	element.NavBar.ElementId = @SourceElementId

INSERT INTO
	element.NavBarTab (TenantId, ElementId, [Name], SortOrder, PageId)
SELECT
	@DestTenantId,
	@DestElementId,
	element.NavBarTab.[Name],
	element.NavBarTab.SortOrder,
	element.NavBarTab.PageId
FROM
	element.NavBarTab
WHERE
	element.NavBarTab.TenantId  = @SourceTenantId AND
	element.NavBarTab.ElementId = @SourceElementId