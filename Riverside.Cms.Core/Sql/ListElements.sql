SET NOCOUNT ON

SELECT
	cms.Element.TenantId,
	cms.Element.ElementId,
	cms.Element.ElementTypeId,
	cms.Element.Name
FROM
	cms.Element
WHERE
	cms.Element.TenantId = @TenantId
ORDER BY
	cms.Element.Name