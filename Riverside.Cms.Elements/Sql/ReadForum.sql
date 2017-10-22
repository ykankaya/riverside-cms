SET NOCOUNT ON

SELECT
	element.Forum.TenantId,
	element.Forum.ElementId,
	element.Forum.ThreadCount,
	element.Forum.PostCount,
	element.Forum.OwnerTenantId,
	element.Forum.OwnerUserId,
	element.Forum.OwnerOnlyThreads
FROM
	element.Forum
WHERE
	element.Forum.TenantId = @TenantId AND
	element.Forum.ElementId = @ElementId