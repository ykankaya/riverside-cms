CREATE PROCEDURE [cms].[UpdatePageTags]
	@TenantId	bigint,
	@PageId		bigint,
	@Created	datetime,
	@Tags		TagTableType READONLY
AS

SET NOCOUNT ON

-- Delete tag / page relationships that no longer exist

DELETE
	cms.TagPage
FROM
	cms.TagPage
INNER JOIN
	cms.Tag
ON
	cms.TagPage.TenantId = cms.Tag.TenantId AND
	cms.TagPage.TagId    = cms.Tag.TagId AND
	cms.TagPage.PageId   = @PageId
LEFT JOIN
	@Tags Tags
ON
	cms.Tag.TenantId = @TenantId AND
	cms.Tag.Name     = Tags.Name
WHERE
	Tags.Name IS NULL

-- Delete tags that no longer exist

DELETE
	cms.Tag
FROM
	cms.Tag
LEFT JOIN
	cms.TagPage
ON
	cms.Tag.TenantId = cms.TagPage.TenantId AND
	cms.Tag.TagId    = cms.TagPage.TagId
WHERE
	cms.Tag.TenantId = @TenantId AND
	cms.TagPage.PageId IS NULL

-- Create tags that do not already exist

INSERT INTO
	cms.Tag (TenantId, Name, Created, Updated)
SELECT
	@TenantId,
	Tags.Name,
	@Created,
	@Created
FROM
	@Tags Tags
LEFT JOIN
	cms.Tag
ON
	cms.Tag.TenantId = @TenantId AND
	cms.Tag.Name = Tags.Name
WHERE
	cms.Tag.TagId IS NULL

-- Create tag / page relationships that do not already exist

INSERT INTO
	cms.TagPage (TenantId, TagId, PageId, Created, Updated)
SELECT
	@TenantId,
	cms.Tag.TagId,
	@PageId,
	@Created,
	@Created
FROM
	cms.Tag
INNER JOIN
	@Tags Tags
ON
	cms.Tag.TenantId = @TenantId AND
	cms.Tag.Name  = Tags.Name
LEFT JOIN
	cms.TagPage
ON
	cms.Tag.TenantId   = cms.TagPage.TenantId AND
	cms.Tag.TagId	   = cms.TagPage.TagId AND
	cms.TagPage.PageId = @PageId
WHERE
	cms.TagPage.PageId IS NULL