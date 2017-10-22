SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH ForumThreadPosts AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY element.ForumPost.SortOrder ASC) AS RowNumber,
		element.ForumPost.TenantId,
		element.ForumPost.ElementId,
		element.ForumPost.ThreadId,
		element.ForumPost.PostId,
		element.ForumPost.ParentPostId,
		cms.[User].UserId,
		cms.[User].Alias,
		element.ForumPost.[Message],
		element.ForumPost.SortOrder,
		element.ForumPost.Created,
		element.ForumPost.Updated,
		cms.[Image].Width AS Width,
		cms.[Image].Height AS Height,
		cms.[Upload].Updated AS Uploaded
	FROM
		element.ForumPost
	INNER JOIN
		cms.[User]
	ON
		element.ForumPost.TenantId = cms.[User].TenantId AND
		element.ForumPost.UserId   = cms.[User].UserId
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
	WHERE
		element.ForumPost.TenantId  = @TenantId AND
		element.ForumPost.ElementId = @ElementId AND
		element.ForumPost.ThreadId  = @ThreadId
)

SELECT
	ForumThreadPosts.TenantId,
	ForumThreadPosts.ElementId,
	ForumThreadPosts.ThreadId,
	ForumThreadPosts.PostId,
	ForumThreadPosts.ParentPostId,
	ForumThreadPosts.UserId,
	ForumThreadPosts.Alias,
	ForumThreadPosts.[Message],
	ForumThreadPosts.SortOrder,
	ForumThreadPosts.Created,
	ForumThreadPosts.Updated,
	ForumThreadPosts.Width,
	ForumThreadPosts.Height,
	ForumThreadPosts.Uploaded
FROM
	ForumThreadPosts
WHERE
	ForumThreadPosts.RowNumber > @RowNumberLowerBound AND ForumThreadPosts.RowNumber < @RowNumberUpperBound
ORDER BY
	ForumThreadPosts.RowNumber ASC

SELECT
	element.ForumThread.Replies
FROM
	element.ForumThread
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId