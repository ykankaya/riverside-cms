CREATE PROCEDURE [cms].[DeleteDomainsByTenant]
	@TenantId bigint
AS

SET NOCOUNT ON

DELETE 
	cms.Domain
WHERE
	cms.Domain.TenantId = @TenantId
