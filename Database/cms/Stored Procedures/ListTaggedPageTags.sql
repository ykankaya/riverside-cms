CREATE PROCEDURE [cms].[ListTaggedPageTags]
	@TenantId bigint,
	@PageId   bigint,
	@Tags     TagTableType READONLY
AS

SET NOCOUNT ON

-- Get pages with all tags
DECLARE @Pages TABLE (
    PageId bigint NOT NULL
)

DECLARE @TagCount int
SELECT @TagCount = (SELECT COUNT(*) FROM @Tags)

INSERT INTO
	@Pages (PageId)
SELECT
	cms.[Page].PageId
FROM
	cms.[Page]
INNER JOIN
	cms.TagPage
ON
	cms.[Page].TenantId = cms.TagPage.TenantId AND
	cms.[Page].PageId	= cms.TagPage.PageId
INNER JOIN
	@Tags Tags
ON
	cms.TagPage.TagId = Tags.TagId
WHERE
	cms.[Page].TenantId = @TenantId AND (cms.[Page].ParentPageId = @PageId OR (@PageId IS NULL AND cms.[Page].ParentPageId IS NULL))
GROUP BY
	cms.[Page].PageId
HAVING
	COUNT(Tags.TagId) = @TagCount

SELECT
	COUNT(cms.Tag.TagId) AS TagCount,
	cms.Tag.TagId,
	cms.Tag.Name
FROM
	@Pages [PagesTable]
INNER JOIN
	cms.[Page]
ON
	cms.[Page].PageId = [PagesTable].PageId
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
	cms.[Tag]
ON
	cms.TagPage.TenantId = cms.Tag.TenantId AND
	cms.TagPage.TagId    = cms.Tag.TagId
LEFT JOIN
	@Tags Tags
ON
	cms.Tag.TagId = Tags.TagId
WHERE
	cms.[Page].TenantId = @TenantId AND
	cms.MasterPage.PageType	= 1 /* PageType.Document */ AND
	Tags.TagId IS NULL
GROUP BY
	cms.Tag.TagId,
	cms.Tag.Name
ORDER BY
	cms.Tag.Name