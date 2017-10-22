CREATE PROCEDURE [cms].[LoadTagsForPages]
	@TenantId bigint,
	@Pages	  PageTableType READONLY
AS

SET NOCOUNT ON

SELECT DISTINCT
	cms.Tag.TenantId,
	cms.Tag.TagId,
	cms.Tag.Name,
	cms.Tag.Created,
	cms.Tag.Updated
FROM
	@Pages Pages
INNER JOIN
	cms.TagPage
ON
	cms.TagPage.PageId = Pages.PageId
INNER JOIN
	cms.Tag
ON
	cms.TagPage.TenantId = cms.Tag.TenantId AND
	cms.TagPage.TagId	 = cms.Tag.TagId
WHERE
	cms.TagPage.TenantId = @TenantId

SELECT
	cms.TagPage.TenantId,
	cms.TagPage.TagId,
	cms.TagPage.PageId
FROM
	@Pages Pages
INNER JOIN
	cms.TagPage
ON
	cms.TagPage.PageId = Pages.PageId
INNER JOIN
	cms.Tag
ON
	cms.TagPage.TenantId = cms.Tag.TenantId AND
	cms.TagPage.TagId	 = cms.Tag.TagId	
WHERE
	cms.TagPage.TenantId = @TenantId
ORDER BY
	cms.TagPage.PageId,
	cms.Tag.Name