CREATE PROCEDURE [cms].[ReadDomainByUrl]
	@Url nvarchar(256)
AS

SET NOCOUNT ON

SELECT
	cms.Domain.TenantId,
	cms.Domain.DomainId,
	cms.Domain.Url,
	cms.Domain.RedirectUrl,
	cms.Web.Name
FROM
	cms.Domain
INNER JOIN
	cms.Web
ON
	cms.Domain.TenantId = cms.Web.TenantId
WHERE
	cms.Domain.Url = @Url
