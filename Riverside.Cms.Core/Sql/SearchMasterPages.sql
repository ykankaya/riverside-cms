SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH [MasterPages] AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY cms.MasterPage.Name ASC) AS RowNumber,
		cms.MasterPage.TenantId,
		cms.MasterPage.MasterPageId
    FROM
        cms.MasterPage
    WHERE
		cms.MasterPage.TenantId = @TenantId AND
        cms.MasterPage.Name LIKE '%' + @Search + '%'
)

SELECT
	cms.MasterPage.TenantId,
	cms.MasterPage.MasterPageId,
	cms.MasterPage.Name,
	cms.MasterPage.PageName,
	cms.MasterPage.PageDescription,
	cms.MasterPage.AncestorPageId,
	cms.MasterPage.AncestorPageLevel,
	cms.MasterPage.PageType,
	cms.MasterPage.HasOccurred,
	cms.MasterPage.HasImage,
	cms.MasterPage.ThumbnailImageWidth,
	cms.MasterPage.ThumbnailImageHeight,
	cms.MasterPage.ThumbnailImageResizeMode,
	cms.MasterPage.PreviewImageWidth,
	cms.MasterPage.PreviewImageHeight,
	cms.MasterPage.PreviewImageResizeMode,
	cms.MasterPage.ImageMinWidth,
	cms.MasterPage.ImageMinHeight,
	cms.MasterPage.Creatable,
	cms.MasterPage.Deletable,
	cms.MasterPage.Taggable,
	cms.MasterPage.Administration,
	cms.MasterPage.BeginRender,
	cms.MasterPage.EndRender
FROM
	[MasterPages]
INNER JOIN
	cms.MasterPage
ON
	[MasterPages].TenantId = cms.MasterPage.TenantId AND
	[MasterPages].MasterPageId = cms.MasterPage.MasterPageId
WHERE
	[MasterPages].RowNumber > @RowNumberLowerBound AND [MasterPages].RowNumber < @RowNumberUpperBound
ORDER BY
    [MasterPages].RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.MasterPage
WHERE
	cms.MasterPage.TenantId = @TenantId AND
	cms.MasterPage.Name LIKE '%' + @Search + '%'