SET NOCOUNT ON

INSERT INTO
	element.NavBar (TenantId, ElementId, [Name], ShowLoggedOnUserOptions, ShowLoggedOffUserOptions)
VALUES
	(@TenantId, @ElementId, @Name, @ShowLoggedOnUserOptions, @ShowLoggedOffUserOptions)

INSERT INTO
	element.NavBarTab (TenantId, ElementId, [Name], SortOrder, PageId)
SELECT
	@TenantId, @ElementId, NavBarTabs.[Name], NavBarTabs.SortOrder, NavBarTabs.PageId
FROM
	@NavBarTabs NavBarTabs