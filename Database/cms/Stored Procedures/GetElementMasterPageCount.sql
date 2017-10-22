CREATE PROCEDURE [cms].[GetElementMasterPageCount]
	@TenantId  bigint,
	@ElementId bigint
AS

SET NOCOUNT ON

SELECT
	COUNT(*) AS ElementMasterPageCount
FROM
	cms.MasterPageZoneElement
WHERE
	cms.MasterPageZoneElement.TenantId = @TenantId AND
	cms.MasterPageZoneElement.ElementId = @ElementId