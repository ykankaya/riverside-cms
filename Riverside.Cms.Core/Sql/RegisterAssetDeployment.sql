SET NOCOUNT ON

INSERT INTO
	cms.AssetDeployment (TenantId, Hostname, Deployed)
VALUES
	(@TenantId, @Hostname, @Deployed)