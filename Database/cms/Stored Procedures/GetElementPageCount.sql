CREATE PROCEDURE [cms].[GetElementPageCount]
	@TenantId  bigint,
	@ElementId bigint
AS

SET NOCOUNT ON

SELECT
	COUNT(*) AS ElementPageCount
FROM
	cms.PageZoneElement
WHERE
	cms.PageZoneElement.TenantId = @TenantId AND
	cms.PageZoneElement.ElementId = @ElementId