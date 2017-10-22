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
	element.ForumThread.Updated
FROM
	element.ForumThread
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId