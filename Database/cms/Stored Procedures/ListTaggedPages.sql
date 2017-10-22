CREATE PROCEDURE [cms].[ListTaggedPages]
	@TenantId	bigint,
	@PageId		bigint,
	@SortBy		int,
	@SortAsc	bit,
	@PageType	int,
	@PageIndex	int,
	@PageSize	int,
	@Tags		TagTableType READONLY
AS

SET NOCOUNT ON

-- Get pages with all tags and specified parent

DECLARE @Pages TABLE (
    PageId bigint NOT NULL
)

DECLARE @TagCount int
SELECT @TagCount = (SELECT COUNT(*) FROM @Tags)

INSERT INTO
	@Pages (PageId)
SELECT
	cms.[Page].PageId
FROM
	cms.[Page]
INNER JOIN
	cms.TagPage
ON
	cms.[Page].TenantId = cms.TagPage.TenantId AND
	cms.[Page].PageId   = cms.TagPage.PageId
INNER JOIN
	@Tags Tags
ON
	cms.TagPage.TagId = Tags.TagId
WHERE
	cms.[Page].TenantId = @TenantId AND (cms.[Page].ParentPageId = @PageId OR (@PageId IS NULL AND cms.[Page].ParentPageId IS NULL))
GROUP BY
	cms.[Page].PageId
HAVING
	COUNT(Tags.TagId) = @TagCount

-- Get pages into order so that we can extract the right pages according to the paging and sorting parameters

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

;WITH [Pages] AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY
			CASE WHEN @SortBy = 0 /* PageSortBy.Created  */ AND @SortAsc = 0 THEN cms.[Page].Created  END DESC,
			CASE WHEN @SortBy = 1 /* PageSortBy.Updated  */ AND @SortAsc = 0 THEN cms.[Page].Updated  END DESC,
			CASE WHEN @SortBy = 2 /* PageSortBy.Occurred */ AND @SortAsc = 0 THEN cms.[Page].Occurred END DESC,
			CASE WHEN @SortBy = 3 /* PageSortBy.Name     */ AND @SortAsc = 0 THEN cms.[Page].Name     END DESC,
			CASE WHEN @SortBy = 0 /* PageSortBy.Created  */ AND @SortAsc = 1 THEN cms.[Page].Created  END ASC,
			CASE WHEN @SortBy = 1 /* PageSortBy.Updated  */ AND @SortAsc = 1 THEN cms.[Page].Updated  END ASC,
			CASE WHEN @SortBy = 2 /* PageSortBy.Occurred */ AND @SortAsc = 1 THEN cms.[Page].Occurred END ASC,
			CASE WHEN @SortBy = 3 /* PageSortBy.Name     */ AND @SortAsc = 1 THEN cms.[Page].Name     END ASC) AS RowNumber,
        cms.[Page].TenantId,
		cms.[Page].PageId
    FROM
        @Pages [PagesTable]
	INNER JOIN
		cms.[Page]
	ON
		cms.[Page].TenantId = @TenantId AND
		cms.[Page].PageId   = [PagesTable].PageId
	INNER JOIN
		cms.[MasterPage]
	ON
		cms.[Page].TenantId		= cms.MasterPage.TenantId AND
		cms.[Page].MasterPageId = cms.MasterPage.MasterPageId
    WHERE
		cms.MasterPage.PageType	= @PageType
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
	[Pages]
INNER JOIN
	cms.[Page]
ON
	[Pages].TenantId = cms.[Page].TenantId AND
	[Pages].PageId	 = cms.[Page].PageId
WHERE
	[Pages].RowNumber > @RowNumberLowerBound AND [Pages].RowNumber < @RowNumberUpperBound
ORDER BY
    [Pages].RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.[Page]
INNER JOIN
	@Pages [PagesTable]
ON
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].PageId   = [PagesTable].PageId
INNER JOIN
	cms.[MasterPage]
ON
	cms.[Page].TenantId		= cms.MasterPage.TenantId AND
	cms.[Page].MasterPageId = cms.MasterPage.MasterPageId
WHERE
	cms.MasterPage.PageType	= @PageType