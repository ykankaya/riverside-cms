CREATE PROCEDURE [cms].[ListPageTags]
	@TenantId bigint,
	@PageId	  bigint
AS

SET NOCOUNT ON

SELECT
	COUNT(cms.Tag.TagId) AS TagCount,
	cms.Tag.TagId,
	cms.Tag.Name
FROM
	cms.[Page]
INNER JOIN
	cms.[MasterPage]
ON
	cms.[Page].TenantId		= cms.MasterPage.TenantId AND
	cms.[Page].MasterPageId = cms.MasterPage.MasterPageId
INNER JOIN
	cms.TagPage
ON
	cms.[Page].TenantId = cms.TagPage.TenantId AND
	cms.[Page].PageId   = cms.TagPage.PageId
INNER JOIN
	cms.Tag
ON
	cms.TagPage.TenantId = cms.Tag.TenantId AND
	cms.TagPage.TagId	 = cms.Tag.TagId
WHERE
	cms.[Page].TenantId		= @TenantId AND (cms.[Page].ParentPageId = @PageId OR (@PageId IS NULL AND cms.[Page].ParentPageId IS NULL)) AND
	cms.MasterPage.PageType	= 1 /* PageType.Document */
GROUP BY
	cms.Tag.TagId,
	cms.Tag.Name
ORDER BY
	cms.Tag.Name