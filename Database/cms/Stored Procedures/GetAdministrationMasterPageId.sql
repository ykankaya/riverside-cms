CREATE PROCEDURE [cms].[GetAdministrationMasterPageId]
	@TenantId  bigint
AS

SET NOCOUNT ON

SELECT
	cms.MasterPage.MasterPageId
FROM
	cms.MasterPage
WHERE
	cms.MasterPage.TenantId		  = @TenantId AND
	cms.MasterPage.Administration = 1