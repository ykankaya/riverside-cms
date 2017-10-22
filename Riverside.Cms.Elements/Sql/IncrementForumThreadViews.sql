SET NOCOUNT ON

UPDATE
	element.ForumThread
SET
	element.ForumThread.[Views] = element.ForumThread.[Views] + 1
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId