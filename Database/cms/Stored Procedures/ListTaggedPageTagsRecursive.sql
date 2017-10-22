CREATE PROCEDURE [cms].[ListTaggedPageTagsRecursive]
	@TenantId bigint,
	@PageId   bigint,
	@Tags	  TagTableType READONLY
AS

SET NOCOUNT ON

DECLARE @Folders TABLE(
    PageId bigint NULL
)

-- Record all child folders under page that is passed to this stored procedure 

;WITH [Folders] AS
(
    SELECT
		cms.[Page].PageId
    FROM
        cms.[Page]
	INNER JOIN
		cms.[MasterPage]
	ON
		cms.[Page].TenantId		= cms.[MasterPage].TenantId AND
		cms.[Page].MasterPageId = cms.[MasterPage].MasterPageId
    WHERE
		cms.[Page].TenantId		  = @TenantId AND (cms.[Page].ParentPageId = @PageId OR (@PageId IS NULL AND cms.[Page].ParentPageId IS NULL)) AND
		cms.[MasterPage].PageType = 0 /* PageType.Folder */
	UNION ALL
	SELECT
		ChildPage.PageId
	FROM
		cms.[Page] ChildPage
	INNER JOIN
		cms.[MasterPage] ChildMasterPage
	ON
		ChildPage.TenantId	   = ChildMasterPage.TenantId AND
		ChildPage.MasterPageId = ChildMasterPage.MasterPageId
	INNER JOIN
		[Folders]
	ON
		ChildPage.TenantId	   = @TenantId AND
		ChildPage.ParentPageId = [Folders].PageId
	WHERE
		ChildMasterPage.PageType = 0 /* PageType.Folder */
)

INSERT INTO
	@Folders (PageId)
SELECT
	[Folders].PageId
FROM
	[Folders]

-- Record the page that is passed to this stored procedure

IF (@PageId IS NULL)
	INSERT INTO
		@Folders (PageId)
	VALUES
		(NULL)
ELSE
	INSERT INTO
		@Folders (PageId)
	SELECT
		cms.[Page].PageId
	FROM
		cms.[Page]
	INNER JOIN
		cms.[MasterPage]
	ON
		cms.[Page].TenantId		= cms.[MasterPage].TenantId AND
		cms.[Page].MasterPageId = cms.[MasterPage].MasterPageId
	WHERE
		cms.[Page].TenantId		  = @TenantId AND
		cms.[Page].PageId		  = @PageId AND
		cms.[MasterPage].PageType = 0 /* PageType.Folder */

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
	@Folders [FoldersTable]
ON
	cms.[Page].TenantId = @TenantId AND (cms.[Page].ParentPageId = [FoldersTable].PageId OR ([FoldersTable].PageId IS NULL AND cms.[Page].ParentPageId IS NULL))
INNER JOIN
	cms.TagPage
ON
	cms.[Page].TenantId = cms.TagPage.TenantId AND
	cms.[Page].PageId   = cms.TagPage.PageId
INNER JOIN
	@Tags Tags
ON
	cms.TagPage.TagId = Tags.TagId
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