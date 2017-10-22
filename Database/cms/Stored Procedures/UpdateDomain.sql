CREATE PROCEDURE [cms].[UpdateDomain]
	@TenantId	 bigint,
	@DomainId    bigint,
	@Url         nvarchar(256),
	@RedirectUrl nvarchar(256)
AS

SET NOCOUNT ON

UPDATE
	cms.Domain
SET
	cms.Domain.Url         = @Url,
	cms.Domain.RedirectUrl = @RedirectUrl
WHERE
	cms.Domain.TenantId = @TenantId AND
	cms.Domain.DomainId = @DomainId
