SET NOCOUNT ON

SELECT
	element.NavBar.TenantId,
	element.NavBar.ElementId,
	element.NavBar.[Name],
	element.NavBar.ShowLoggedOnUserOptions,
	element.NavBar.ShowLoggedOffUserOptions
FROM
	element.NavBar
WHERE
	element.NavBar.TenantId = @TenantId AND
	element.NavBar.ElementId = @ElementId

SELECT
	element.NavBarTab.TenantId,
	element.NavBarTab.ElementId,
	element.NavBarTab.NavBarTabId,
	element.NavBarTab.[Name],
	element.NavBarTab.SortOrder,
	element.NavBarTab.PageId
FROM
	element.NavBarTab
WHERE
	element.NavBarTab.TenantId = @TenantId AND
	element.NavBarTab.ElementId = @ElementId
ORDER BY
	element.NavBarTab.SortOrder