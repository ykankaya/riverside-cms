SET NOCOUNT ON

-- Create new thread and get allocated thread identifier
INSERT INTO
	element.ForumThread (TenantId, ElementId, UserId, [Subject], [Message], Notify, [Views], Replies, LastPostId, LastMessageCreated, Created, Updated)
VALUES
	(@TenantId, @ElementId, @UserId, @Subject, @Message, @Notify, 0, 0, NULL, @Created, @Created, @Created)

SET @ThreadId = SCOPE_IDENTITY()

-- Increment forum thread and post counts
UPDATE
	element.Forum
SET
	element.Forum.ThreadCount = element.Forum.ThreadCount + 1
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId