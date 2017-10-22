SET NOCOUNT ON

UPDATE
	element.NavBar
SET
	element.NavBar.[Name]					= @Name,
	element.NavBar.ShowLoggedOnUserOptions  = @ShowLoggedOnUserOptions,
	element.NavBar.ShowLoggedOffUserOptions = @ShowLoggedOffUserOptions
WHERE
	element.NavBar.TenantId  = @TenantId AND
	element.NavBar.ElementId = @ElementId

/*----- Update existing nav bar tabs -----*/

UPDATE
	element.NavBarTab
SET
	element.NavBarTab.[Name]	= NavBarTabs.[Name],
	element.NavBarTab.SortOrder = NavBarTabs.SortOrder,
	element.NavBarTab.PageId    = NavBarTabs.PageId
FROM
	element.NavBarTab
INNER JOIN
	@NavBarTabs NavBarTabs
ON
	element.NavBarTab.TenantId    = @TenantId AND
	element.NavBarTab.ElementId   = @ElementId AND
	element.NavBarTab.NavBarTabId = NavBarTabs.NavBarTabId

/*----- Delete nav bar tabs that are no longer needed -----*/

DELETE
	element.NavBarTab
FROM
	element.NavBarTab
LEFT JOIN
	@NavBarTabs NavBarTabs
ON
	element.NavBarTab.NavBarTabId = NavBarTabs.NavBarTabId
WHERE
	element.NavBarTab.TenantId  = @TenantId AND
	element.NavBarTab.ElementId = @ElementId AND
	NavBarTabs.NavBarTabId IS NULL
	
/*----- Create new nav bar tabs -----*/

INSERT INTO
	element.NavBarTab (TenantId, ElementId, [Name], SortOrder, PageId)
SELECT
	@TenantId, @ElementId, NavBarTabs.[Name], NavBarTabs.SortOrder, NavBarTabs.PageId
FROM
	@NavBarTabs NavBarTabs
WHERE
	NavBarTabs.NavBarTabId IS NULL