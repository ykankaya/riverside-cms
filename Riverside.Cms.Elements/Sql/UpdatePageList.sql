SET NOCOUNT ON

UPDATE
	element.PageList
SET
	element.PageList.PageTenantId		 = @PageTenantId,
	element.PageList.PageId				 = @PageId,
	element.PageList.DisplayName		 = @DisplayName,
	element.PageList.SortBy				 = @SortBy,
	element.PageList.SortAsc			 = @SortAsc,
	element.PageList.ShowRelated		 = @ShowRelated,
	element.PageList.ShowDescription	 = @ShowDescription,
	element.PageList.ShowImage			 = @ShowImage,
	element.PageList.ShowBackgroundImage = @ShowBackgroundImage,
	element.PageList.ShowCreated		 = @ShowCreated,
	element.PageList.ShowUpdated		 = @ShowUpdated,
	element.PageList.ShowOccurred		 = @ShowOccurred,
	element.PageList.ShowComments		 = @ShowComments,
	element.PageList.ShowTags			 = @ShowTags,
	element.PageList.ShowPager			 = @ShowPager,
	element.PageList.MoreMessage		 = @MoreMessage,
	element.PageList.[Recursive]		 = @Recursive,
	element.PageList.PageType			 = @PageType,
	element.PageList.PageSize			 = @PageSize,
	element.PageList.NoPagesMessage		 = @NoPagesMessage,
	element.PageList.Preamble            = @Preamble
WHERE
	element.PageList.TenantId  = @TenantId AND
	element.PageList.ElementId = @ElementId