SET NOCOUNT ON

UPDATE
	element.Forum
SET
	element.Forum.OwnerTenantId    = @TenantId,
	element.Forum.OwnerUserId	   = @OwnerUserId,
	element.Forum.OwnerOnlyThreads = @OwnerOnlyThreads
WHERE
	element.Forum.TenantId  = @TenantId AND
	element.Forum.ElementId = @ElementId