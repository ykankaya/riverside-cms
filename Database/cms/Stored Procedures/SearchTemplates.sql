CREATE PROCEDURE [cms].[SearchTemplates]
	@PageIndex int,
	@PageSize  int,
	@Search    nvarchar(50)
AS

SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH [Templates] AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY cms.Template.Name ASC) AS RowNumber,
		cms.Template.TenantId
    FROM
        cms.Template
    WHERE
        cms.Template.Name LIKE '%' + @Search + '%'
)

SELECT
	cms.Template.TenantId,
	cms.Template.Name,
	cms.Template.[Description],
	cms.Template.CreateUserEnabled,
	cms.Template.UserHasImage,
	cms.Template.UserThumbnailImageWidth,
	cms.Template.UserThumbnailImageHeight,
	cms.Template.UserThumbnailImageResizeMode,
	cms.Template.UserPreviewImageWidth,
	cms.Template.UserPreviewImageHeight,
	cms.Template.UserPreviewImageResizeMode,
	cms.Template.UserImageMinWidth,
	cms.Template.UserImageMinHeight
FROM
	[Templates]
INNER JOIN
	cms.Template
ON
	[Templates].TenantId = cms.Template.TenantId
WHERE
	[Templates].RowNumber > @RowNumberLowerBound AND [Templates].RowNumber < @RowNumberUpperBound
ORDER BY
    [Templates].RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.Template
WHERE
	cms.Template.Name LIKE '%' + @Search + '%'