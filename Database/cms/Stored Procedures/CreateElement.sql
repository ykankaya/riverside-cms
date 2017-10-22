CREATE PROCEDURE [cms].[CreateElement]
	@TenantId	   bigint,
	@ElementTypeId uniqueidentifier,
	@Name          nvarchar(50),
	@ElementId     bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Element (TenantId, ElementTypeId, Name)
VALUES
	(@TenantId, @ElementTypeId, @Name)

SET @ElementId = SCOPE_IDENTITY()