CREATE PROCEDURE [cms].[ReadPageHierarchy]
	@TenantId bigint,
	@PageId   bigint
AS

SET NOCOUNT ON

;WITH [Pages] AS
(
	SELECT
		0 AS [Level],
		cms.[Page].PageId,
		cms.[Page].ParentPageId
	FROM
		cms.[Page]
	WHERE
		cms.[Page].TenantId = @TenantId AND
		cms.[Page].PageId   = @PageId
	UNION ALL
	SELECT
		[Pages].[Level] + 1 AS [Level],
		ParentPage.PageId,
		ParentPage.ParentPageId
	FROM
		cms.[Page] ParentPage
	INNER JOIN
		[Pages]
	ON
		ParentPage.TenantId = @TenantId AND
		ParentPage.PageId   = [Pages].ParentPageId
)

SELECT
	cms.[Page].TenantId,
	cms.[Page].PageId,
	cms.[Page].ParentPageId,
	cms.[Page].MasterPageId,
	cms.[Page].Name,
	cms.[Page].[Description],
	cms.[Page].Created,
	cms.[Page].Updated,
	cms.[Page].Occurred,
	cms.[Page].ImageTenantId,
	cms.[Page].ThumbnailImageUploadId,
	cms.[Page].PreviewImageUploadId,
	cms.[Page].ImageUploadId
FROM
	cms.[Page]
INNER JOIN
	[Pages]
ON
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].PageId   = [Pages].PageId
ORDER BY
	[Pages].[Level] DESC