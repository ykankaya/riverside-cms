SET NOCOUNT ON

DELETE
	element.ForumPost
WHERE
	element.ForumPost.TenantId  = @TenantId AND
	element.ForumPost.ElementId = @ElementId

DELETE
	element.ForumThread
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId

DELETE
	element.Forum
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId