SET NOCOUNT ON

UPDATE
	element.ForumPost
SET
	element.ForumPost.[Message] = @Message,
	element.ForumPost.Updated   = @Updated
WHERE
	element.ForumPost.TenantId  = @TenantId AND
	element.ForumPost.ElementId = @ElementId AND
	element.ForumPost.ThreadId  = @ThreadId AND
	element.ForumPost.PostId    = @PostId