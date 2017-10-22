CREATE PROCEDURE [cms].[ListNamedTags]
	@TenantId bigint,
	@Tags	  TagTableType READONLY
AS

SET NOCOUNT ON

SELECT
	cms.Tag.TenantId,
	cms.Tag.TagId,
	cms.Tag.Name,
	cms.Tag.Created,
	cms.Tag.Updated
FROM
	cms.Tag
INNER JOIN
	@Tags Tags
ON
	cms.Tag.Name = Tags.Name
WHERE
	cms.Tag.TenantId = @TenantId
ORDER BY
	cms.Tag.Name