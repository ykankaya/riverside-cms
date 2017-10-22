CREATE PROCEDURE [cms].[GetHomePageId]
	@TenantId bigint
AS

SET NOCOUNT ON

SELECT
	cms.[Page].PageId
FROM
	cms.[Page]
WHERE
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].ParentPageId IS NULL