SET NOCOUNT ON

INSERT INTO
	element.PageList (TenantId, ElementId, PageTenantId, PageId, DisplayName, SortBy, SortAsc, ShowRelated, ShowDescription, ShowImage, ShowBackgroundImage, ShowCreated, ShowUpdated, ShowOccurred, ShowComments, ShowTags, ShowPager, MoreMessage, [Recursive], PageType, PageSize, NoPagesMessage, Preamble)
VALUES
	(@TenantId, @ElementId, @PageTenantId, @PageId, @DisplayName, @SortBy, @SortAsc, @ShowRelated, @ShowDescription, @ShowImage, @ShowBackgroundImage, @ShowCreated, @ShowUpdated, @ShowOccurred, @ShowComments, @ShowTags, @ShowPager, @MoreMessage, @Recursive, @PageType, @PageSize, @NoPagesMessage, @Preamble)