SET NOCOUNT ON

SELECT
	element.ForumThread.TenantId,
	element.ForumThread.ElementId,
	element.ForumThread.ThreadId,
	element.ForumThread.UserId,
	element.ForumThread.[Subject],
	element.ForumThread.[Message],
	element.ForumThread.Notify,
	element.ForumThread.[Views],
	element.ForumThread.Replies,
	element.ForumThread.LastPostId,
	element.ForumThread.LastMessageCreated,
	element.ForumThread.Created,
	element.ForumThread.Updated,
	cms.[User].Alias,
	cms.[Image].Width AS Width,
	cms.[Image].Height AS Height,
	cms.[Upload].Updated AS Uploaded
FROM
	element.ForumThread
INNER JOIN
	cms.[User]
ON
	element.ForumThread.TenantId = cms.[User].TenantId AND
	element.ForumThread.UserId = cms.[User].UserId
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
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId