SET NOCOUNT ON

INSERT INTO
	element.Forum (TenantId, ElementId, ThreadCount, PostCount, OwnerTenantId, OwnerUserId, OwnerOnlyThreads)
VALUES
	(@TenantId, @ElementId, @ThreadCount, @PostCount, @OwnerTenantId, @OwnerUserId, @OwnerOnlyThreads)