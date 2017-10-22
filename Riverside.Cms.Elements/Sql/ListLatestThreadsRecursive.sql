SET NOCOUNT ON

DECLARE @Folders TABLE(
    TenantId bigint NOT NULL,
    PageId bigint NULL
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
		cms.[Page].TenantId     = cms.[MasterPage].TenantId AND
		cms.[Page].MasterPageId = cms.[MasterPage].MasterPageId
    WHERE
		cms.[Page].TenantId = @TenantId AND (cms.[Page].ParentPageId = @PageId OR (@PageId IS NULL AND cms.[Page].ParentPageId IS NULL)) AND
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
		ChildPage.TenantId     = ChildMasterPage.TenantId AND
		ChildPage.MasterPageId = ChildMasterPage.MasterPageId
	INNER JOIN
		[Folders]
	ON
		ChildPage.TenantId = @TenantId AND
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
		cms.[Page].TenantId       = @TenantId AND
		cms.[Page].PageId		  = @PageId AND
		cms.[MasterPage].PageType = 0 /* PageType.Folder */

-- Get pages with specified parent

DECLARE @Pages TABLE (
    PageId bigint NOT NULL
)

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

-- Get forums on these pages

DECLARE @Forums TABLE (
	PageId bigint NOT NULL,
	ElementId bigint NOT NULL
)

INSERT INTO
	@Forums (PageId, ElementId)
SELECT
	cms.PageZoneElement.PageId,
	cms.PageZoneElement.ElementId
FROM
	@Pages [PagesTable]
INNER JOIN
	cms.PageZoneElement
ON
	cms.PageZoneElement.PageId = [PagesTable].PageId
INNER JOIN
	element.Forum
ON
	cms.PageZoneElement.TenantId  = element.Forum.TenantId AND
	cms.PageZoneElement.ElementId = element.Forum.ElementId
WHERE
	cms.PageZoneElement.TenantId = @TenantId

/*----- Now pick off latest threads from these forums -----*/

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH ForumThreads AS
(
	SELECT TOP (@RowNumberUpperBound)
		ROW_NUMBER() OVER (ORDER BY element.ForumThread.LastMessageCreated DESC) AS RowNumber,
		[ForumsTable].PageId,
		element.ForumThread.ElementId,
		element.ForumThread.ThreadId
	FROM
		element.ForumThread
	INNER JOIN
		@Forums [ForumsTable]
	ON
		element.ForumThread.TenantId = @TenantId AND
		element.ForumThread.ElementId = [ForumsTable].ElementId
)

SELECT
	ForumThreads.PageId,
	element.ForumThread.TenantId,
	element.ForumThread.ElementId,
	element.ForumThread.ThreadId,
	element.ForumThread.UserId,
	cms.[User].Alias,
	element.ForumThread.[Subject],
	element.ForumThread.[Message],
	element.ForumThread.Notify,
	element.ForumThread.[Views],
	element.ForumThread.Replies,
	element.ForumThread.LastPostId,
	element.ForumThread.LastMessageCreated,
	element.ForumThread.Created,
	element.ForumThread.Updated,
	LastPostUser.UserId AS LastPostUserId,
	LastPostUser.Alias AS LastPostAlias,
	cms.[Image].Width AS Width,
	cms.[Image].Height AS Height,
	cms.Upload.Updated AS Uploaded,
	LastPostImage.Width AS LastPostWidth,
	LastPostImage.Height AS LastPostHeight,
	LastPostUpload.Updated AS LastPostUploaded
FROM
	ForumThreads
INNER JOIN
	element.ForumThread
ON
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = ForumThreads.ElementId AND
	element.ForumThread.ThreadId  = ForumThreads.ThreadId
INNER JOIN
	cms.[User]
ON
	element.ForumThread.TenantId = cms.[User].TenantId AND
	element.ForumThread.UserId = cms.[User].UserId
LEFT JOIN
	element.ForumPost
ON
	element.ForumThread.TenantId   = element.ForumPost.TenantId AND
	element.ForumThread.ElementId  = element.ForumPost.ElementId AND
	element.ForumThread.ThreadId   = element.ForumPost.ThreadId  AND
	element.ForumThread.LastPostId = element.ForumPost.PostId
LEFT JOIN
	cms.[User] LastPostUser 
ON
	element.ForumPost.TenantId = LastPostUser.TenantId AND
	element.ForumPost.UserId = LastPostUser.UserId
LEFT JOIN
	cms.Upload
ON
	cms.[User].ImageTenantId = cms.Upload.TenantId AND
	cms.[User].ThumbnailImageUploadId = cms.Upload.UploadId
LEFT JOIN
	cms.[Image]
ON
	cms.Upload.TenantId = cms.[Image].TenantId AND
	cms.Upload.UploadId = cms.[Image].UploadId
LEFT JOIN
	cms.Upload LastPostUpload
ON
	LastPostUser.ImageTenantId = LastPostUpload.TenantId AND
	LastPostUser.ThumbnailImageUploadId = LastPostUpload.UploadId
LEFT JOIN
	cms.[Image] LastPostImage
ON
	LastPostUpload.TenantId = LastPostImage.TenantId AND
	LastPostUpload.UploadId = LastPostImage.UploadId
WHERE
	ForumThreads.RowNumber > @RowNumberLowerBound AND ForumThreads.RowNumber < @RowNumberUpperBound
ORDER BY
	ForumThreads.RowNumber ASC