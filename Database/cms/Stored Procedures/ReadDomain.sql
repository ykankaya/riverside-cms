CREATE PROCEDURE [cms].[ReadDomain]
	@TenantId bigint,
	@DomainId bigint
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
	cms.Domain.TenantId = @TenantId AND
	cms.Domain.DomainId = @DomainId