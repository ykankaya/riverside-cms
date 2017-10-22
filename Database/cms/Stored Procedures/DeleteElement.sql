CREATE PROCEDURE [cms].[DeleteElement]
	@TenantId  bigint,
	@ElementId bigint
AS

SET NOCOUNT ON

DELETE 
	cms.Element
WHERE
	cms.Element.TenantId  = @TenantId AND
	cms.Element.ElementId = @ElementId