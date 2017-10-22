CREATE PROCEDURE [cms].[ReadElement]
	@TenantId  bigint,
	@ElementId bigint
AS

SET NOCOUNT ON

SELECT
	cms.Element.TenantId,
	cms.Element.ElementId,
	cms.Element.ElementTypeId,
	cms.Element.Name
FROM
	cms.Element
WHERE
	cms.Element.TenantId  = @TenantId AND
	cms.Element.ElementId = @ElementId