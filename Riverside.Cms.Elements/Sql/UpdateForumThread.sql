SET NOCOUNT ON

UPDATE
	element.ForumThread
SET
	element.ForumThread.[Subject] = @Subject,
	element.ForumThread.[Message] = @Message,
	element.ForumThread.Notify    = @Notify,
	element.ForumThread.Updated   = @Updated
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId