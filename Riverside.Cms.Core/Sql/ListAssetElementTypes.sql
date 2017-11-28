SET NOCOUNT ON

SELECT
	cms.AssetElementType.ElementTypeId
FROM
	cms.AssetElementType
WHERE
	cms.AssetElementType.TenantId = @TenantId