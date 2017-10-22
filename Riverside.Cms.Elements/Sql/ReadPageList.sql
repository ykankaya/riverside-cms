SET NOCOUNT ON

SELECT
	element.PageList.TenantId,
	element.PageList.ElementId,
	element.PageList.PageTenantId,
	element.PageList.PageId,
	element.PageList.DisplayName,
	element.PageList.SortBy,
	element.PageList.SortAsc,
	element.PageList.ShowRelated,
	element.PageList.ShowDescription,
	element.PageList.ShowImage,
	element.PageList.ShowBackgroundImage,
	element.PageList.ShowCreated,
	element.PageList.ShowUpdated,
	element.PageList.ShowOccurred,
	element.PageList.ShowComments,
	element.PageList.ShowTags,
	element.PageList.ShowPager,
	element.PageList.MoreMessage,
	element.PageList.[Recursive],
	element.PageList.PageType,
	element.PageList.PageSize,
	element.PageList.NoPagesMessage,
	element.PageList.Preamble
FROM
	element.PageList
WHERE
	element.PageList.TenantId = @TenantId AND
	element.PageList.ElementId = @ElementId