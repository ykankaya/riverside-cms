UPDATE
	cms.MasterPage
SET
	cms.MasterPage.Administration = (CASE WHEN cms.MasterPage.MasterPageId = @MasterPageId THEN 1 ELSE 0 END)
WHERE
	cms.MasterPage.TenantId = @TenantId