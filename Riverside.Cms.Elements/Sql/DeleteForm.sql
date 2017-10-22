SET NOCOUNT ON

DELETE
	element.FormField
WHERE
	element.FormField.TenantId  = @TenantId AND
	element.FormField.ElementId = @ElementId

DELETE
	element.Form
WHERE
	element.Form.TenantId  = @TenantId AND
	element.Form.ElementId = @ElementId