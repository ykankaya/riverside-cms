SET NOCOUNT ON

INSERT INTO
	element.PageList (TenantId, ElementId, PageTenantId, PageId, DisplayName, SortBy, SortAsc, ShowRelated, ShowDescription, ShowImage, ShowBackgroundImage, ShowCreated, ShowUpdated, ShowOccurred, ShowComments, ShowTags, ShowPager, MoreMessage, [Recursive], PageType, PageSize, NoPagesMessage, Preamble)
SELECT
	@DestTenantId,
	@DestElementId,
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
	element.PageList.TenantId  = @SourceTenantId AND
	element.PageList.ElementId = @SourceElementId