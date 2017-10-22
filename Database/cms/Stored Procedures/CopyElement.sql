CREATE PROCEDURE [cms].[CopyElement]
	@SourceTenantId  bigint,
	@SourceElementId bigint,
	@DestTenantId	 bigint,
	@DestElementId   bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Element (TenantId, ElementTypeId, Name)
SELECT
	@DestTenantId,
	cms.Element.ElementTypeId,
	cms.Element.Name
FROM
	cms.Element
WHERE
	cms.Element.TenantId  = @SourceTenantId AND
	cms.Element.ElementId = @SourceElementId

SET @DestElementId = SCOPE_IDENTITY()