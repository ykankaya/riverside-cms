SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH Webs AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY cms.Web.Name ASC) AS RowNumber,
        cms.Web.TenantId
    FROM
        cms.Web
    WHERE
        cms.Web.Name LIKE '%' + @Search + '%'
)

SELECT
	cms.Web.TenantId,
	cms.Web.Name,
	cms.Web.CreateUserEnabled,
	cms.Web.UserHasImage,
	cms.Web.UserThumbnailImageWidth,
	cms.Web.UserThumbnailImageHeight,
	cms.Web.UserThumbnailImageResizeMode,
	cms.Web.UserPreviewImageWidth,
	cms.Web.UserPreviewImageHeight,
	cms.Web.UserPreviewImageResizeMode,
	cms.Web.UserImageMinWidth,
	cms.Web.UserImageMinHeight,
	cms.Web.FontOption,
	cms.Web.ColourOption
FROM
	Webs
INNER JOIN
	cms.Web
ON
	Webs.TenantId = cms.Web.TenantId
WHERE
	Webs.RowNumber > @RowNumberLowerBound AND Webs.RowNumber < @RowNumberUpperBound
ORDER BY
    Webs.RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.Web
WHERE
	cms.Web.Name LIKE '%' + @Search + '%'