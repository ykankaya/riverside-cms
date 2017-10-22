SET NOCOUNT ON

UPDATE
	cms.Web
SET
	cms.Web.FontOption	 = @FontOption,
	cms.Web.ColourOption = @ColourOption
WHERE
	cms.Web.TenantId = @TenantId