SET NOCOUNT ON

SELECT
	element.Footer.TenantId,
	element.Footer.ElementId,
	element.Footer.[Message],
	element.Footer.ShowLoggedOnUserOptions,
	element.Footer.ShowLoggedOffUserOptions
FROM
	element.Footer
WHERE
	element.Footer.TenantId = @TenantId AND
	element.Footer.ElementId = @ElementId