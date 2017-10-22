SET NOCOUNT ON

SELECT
	element.ForumPost.TenantId,
	element.ForumPost.ElementId,
	element.ForumPost.ThreadId,
	element.ForumPost.PostId,
	element.ForumPost.ParentPostId,
	element.ForumPost.UserId,
	element.ForumPost.[Message],
	element.ForumPost.SortOrder,
	element.ForumPost.Created,
	element.ForumPost.Updated,
	cms.[User].Alias,
	cms.[Image].Width AS Width,
	cms.[Image].Height AS Height,
	cms.Upload.Updated AS Uploaded
FROM
	element.ForumPost
INNER JOIN
	cms.[User]
ON
	element.ForumPost.TenantId = cms.[User].TenantId AND
	element.ForumPost.UserId = cms.[User].UserId
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
	element.ForumPost.ThreadId  = @ThreadId AND
	element.ForumPost.PostId    = @PostId