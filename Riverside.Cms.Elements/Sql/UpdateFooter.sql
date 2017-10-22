SET NOCOUNT ON

UPDATE
	element.Footer
SET
	element.Footer.[Message]				= @Message,
	element.Footer.ShowLoggedOnUserOptions	= @ShowLoggedOnUserOptions,
	element.Footer.ShowLoggedOffUserOptions	= @ShowLoggedOffUserOptions
WHERE
	element.Footer.TenantId  = @TenantId AND
	element.Footer.ElementId = @ElementId