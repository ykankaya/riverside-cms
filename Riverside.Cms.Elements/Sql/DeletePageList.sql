SET NOCOUNT ON

DELETE
	element.PageList
WHERE
	element.PageList.TenantId  = @TenantId AND
	element.PageList.ElementId = @ElementId