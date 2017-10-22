SET NOCOUNT ON

DELETE 
	cms.Web
WHERE
	cms.Web.TenantId = @TenantId