CREATE PROCEDURE [cms].[CreateDomain]
	@TenantId    bigint,
	@Url         nvarchar(256),
	@RedirectUrl nvarchar(256),
	@DomainId    bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Domain (TenantId, Url, RedirectUrl)
VALUES
	(@TenantId, @Url, @RedirectUrl)

SET @DomainId = SCOPE_IDENTITY()