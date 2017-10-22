SET NOCOUNT ON

DELETE
	element.TagCloud
WHERE
	element.TagCloud.TenantId  = @TenantId AND
	element.TagCloud.ElementId = @ElementId