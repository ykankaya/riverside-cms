SET NOCOUNT ON

SELECT
	cms.AssetDeployment.TenantId,
	cms.AssetDeployment.Hostname,
	cms.AssetDeployment.Deployed
FROM
	cms.AssetDeployment
WHERE
	cms.AssetDeployment.TenantId = @TenantId AND
	cms.AssetDeployment.Hostname = @Hostname