CREATE PROCEDURE [cms].[ListPageTagsRecursive]
	@TenantId bigint,
	@PageId	  bigint
AS

SET NOCOUNT ON

DECLARE @Folders TABLE(
    TenantId bigint NOT NULL,
    PageId   bigint NULL
)

-- Record all child folders under page that is passed to this stored procedure 

;WITH [Folders] AS
(
    SELECT
        cms.[Page].TenantId,
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
		ChildPage.TenantId,
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
	@Folders (TenantId, PageId)
SELECT
	[Folders].TenantId,
	[Folders].PageId
FROM
	[Folders]

-- Record the page that is passed to this stored procedure

IF (@PageId IS NULL)
	INSERT INTO
		@Folders (TenantId, PageId)
	VALUES
		(@TenantId, NULL)
ELSE
	INSERT INTO
		@Folders (TenantId, PageId)
	SELECT
		cms.[Page].TenantId,
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

-- Get tags

SELECT
	COUNT(cms.Tag.TagId) AS TagCount,
	cms.Tag.TagId,
	cms.Tag.Name
FROM
	@Folders [FoldersTable]
INNER JOIN
	cms.[Page]
ON
	cms.[Page].TenantId = [FoldersTable].TenantId AND (cms.[Page].ParentPageId = [FoldersTable].PageId OR ([FoldersTable].PageId IS NULL AND cms.[Page].ParentPageId IS NULL))
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
	cms.MasterPage.PageType	= 1 /* PageType.Document */
GROUP BY
	cms.Tag.TagId,
	cms.Tag.Name
ORDER BY
	cms.Tag.Name