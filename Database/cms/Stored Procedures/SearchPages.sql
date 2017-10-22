CREATE PROCEDURE [cms].[SearchPages]
	@TenantId  bigint,
	@PageIndex int,
	@PageSize  int,
	@Search    nvarchar(50)
AS

SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH Pages AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY cms.[Page].Name ASC) AS RowNumber,
        cms.[Page].TenantId,
		cms.[Page].PageId
    FROM
        cms.[Page]
    WHERE
		cms.[Page].TenantId = @TenantId AND
        cms.[Page].Name LIKE '%' + @Search + '%'
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
	Pages
INNER JOIN
	cms.[Page]
ON
	Pages.TenantId = cms.[Page].TenantId AND
	Pages.PageId   = cms.[Page].PageId
WHERE
	Pages.RowNumber > @RowNumberLowerBound AND Pages.RowNumber < @RowNumberUpperBound
ORDER BY
    Pages.RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.[Page]
WHERE
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].Name LIKE '%' + @Search + '%'