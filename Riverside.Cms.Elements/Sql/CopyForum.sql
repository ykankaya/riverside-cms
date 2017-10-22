SET NOCOUNT ON

INSERT INTO
	element.Forum (TenantId, ElementId, ThreadCount, PostCount, OwnerTenantId, OwnerUserId, OwnerOnlyThreads)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Forum.ThreadCount,
	element.Forum.PostCount,
	element.Forum.OwnerTenantId,
	element.Forum.OwnerUserId,
	element.Forum.OwnerOnlyThreads
FROM
	element.Forum
WHERE
	element.Forum.TenantId  = @SourceTenantId AND
	element.Forum.ElementId = @SourceElementId