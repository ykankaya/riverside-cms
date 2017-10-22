SET NOCOUNT ON

INSERT INTO
	element.Footer (TenantId, ElementId, [Message], ShowLoggedOnUserOptions, ShowLoggedOffUserOptions)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Footer.[Message],
	element.Footer.ShowLoggedOnUserOptions,
	element.Footer.ShowLoggedOffUserOptions
FROM
	element.Footer
WHERE
	element.Footer.TenantId  = @SourceTenantId AND
	element.Footer.ElementId = @SourceElementId