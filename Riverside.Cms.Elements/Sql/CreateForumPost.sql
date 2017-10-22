SET NOCOUNT ON

-- Get next available sort order
DECLARE @SortOrder int
SET @SortOrder = (SELECT (ISNULL(MAX(SortOrder), -1) + 1) FROM element.ForumPost WHERE element.ForumPost.TenantId = @TenantId AND element.ForumPost.ElementId = @ElementId AND element.ForumPost.ThreadId = @ThreadId)

-- Create new post and get allocated post identifier
INSERT INTO
	element.ForumPost (TenantId, ElementId, ThreadId, ParentPostId, UserId, [Message], SortOrder, Created, Updated)
VALUES
	(@TenantId, @ElementId, @ThreadId, @ParentPostId, @UserId, @Message, @SortOrder, @Created, @Created)
SET @PostId = SCOPE_IDENTITY()

-- Update thread replies count and last post id
UPDATE
	element.ForumThread
SET
	element.ForumThread.Replies			   = @SortOrder + 1,
	element.ForumThread.LastPostId		   = @PostId,
	element.ForumThread.Updated			   = @Created,
	element.ForumThread.LastMessageCreated = @Created
WHERE
	element.ForumThread.TenantId  = @TenantId AND
	element.ForumThread.ElementId = @ElementId AND
	element.ForumThread.ThreadId  = @ThreadId

-- Update forum post count
UPDATE
	element.Forum
SET
	element.Forum.PostCount = element.Forum.PostCount + 1
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId