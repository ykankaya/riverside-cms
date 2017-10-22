SET NOCOUNT ON

DELETE
	element.Contact
WHERE
	element.Contact.TenantId  = @TenantId AND
	element.Contact.ElementId = @ElementId