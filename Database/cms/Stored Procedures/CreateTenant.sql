CREATE PROCEDURE [cms].[CreateTenant]
	@Created	datetime,
	@Updated	datetime,
	@TenantId	bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Tenant (Created, Updated)
VALUES
	(@Created, @Updated)

SET @TenantId = SCOPE_IDENTITY()