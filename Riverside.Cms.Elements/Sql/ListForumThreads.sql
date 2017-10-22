SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH ForumThreads AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY element.ForumThread.LastMessageCreated DESC) AS RowNumber,
		element.ForumThread.ThreadId
	FROM
		element.ForumThread
	WHERE
		element.ForumThread.TenantId  = @TenantId AND
		element.ForumThread.ElementId = @ElementId
)

SELECT
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
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = ForumThreads.ThreadId
INNER JOIN
	cms.[User]
ON
	element.ForumThread.TenantId = cms.[User].TenantId AND
	element.ForumThread.UserId   = cms.[User].UserId
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
	element.ForumPost.UserId   = LastPostUser.UserId
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

SELECT
	element.Forum.ThreadCount
FROM
	element.Forum
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId