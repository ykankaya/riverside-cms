SET NOCOUNT ON

DELETE
	element.NavBarTab
WHERE
	element.NavBarTab.TenantId  = @TenantId AND
	element.NavBarTab.ElementId = @ElementId

DELETE
	element.NavBar
WHERE
	element.NavBar.TenantId  = @TenantId AND
	element.NavBar.ElementId = @ElementId