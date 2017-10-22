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
	element.ForumPost.Updated
FROM
	element.ForumPost
WHERE
	element.ForumPost.TenantId  = @TenantId AND
	element.ForumPost.ElementId = @ElementId AND
	element.ForumPost.ThreadId  = @ThreadId AND
	element.ForumPost.PostId    = @PostId