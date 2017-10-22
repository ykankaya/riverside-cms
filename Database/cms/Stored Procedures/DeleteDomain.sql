CREATE PROCEDURE [cms].[DeleteDomain]
	@TenantId bigint,
	@DomainId bigint
AS

SET NOCOUNT ON

DELETE 
	cms.Domain
WHERE
	cms.Domain.TenantId = @TenantId AND
	cms.Domain.DomainId = @DomainId