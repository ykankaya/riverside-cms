SET NOCOUNT ON

DECLARE @PostCount int

DELETE
	element.ForumPost
WHERE
	element.ForumPost.TenantId  = @TenantId AND
	element.ForumPost.ElementId = @ElementId AND
	element.ForumPost.ThreadId  = @ThreadId

SET @PostCount = @@ROWCOUNT

DELETE
	element.ForumThread
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId

UPDATE
	element.Forum
SET
	element.Forum.ThreadCount = element.Forum.ThreadCount - 1,
	element.Forum.PostCount   = element.Forum.PostCount - @PostCount
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId