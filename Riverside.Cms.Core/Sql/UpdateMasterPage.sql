SET NOCOUNT ON

UPDATE
	cms.MasterPage
SET
	cms.MasterPage.Name						= @Name,
	cms.MasterPage.PageName					= @PageName,
	cms.MasterPage.PageDescription			= @PageDescription,
	cms.MasterPage.AncestorPageId			= @AncestorPageId,
	cms.MasterPage.AncestorPageLevel		= @AncestorPageLevel,
	cms.MasterPage.PageType					= @PageType,
	cms.MasterPage.HasOccurred				= @HasOccurred,
	cms.MasterPage.HasImage					= @HasImage,
	cms.MasterPage.ThumbnailImageWidth		= @ThumbnailImageWidth,
	cms.MasterPage.ThumbnailImageHeight		= @ThumbnailImageHeight,
	cms.MasterPage.ThumbnailImageResizeMode	= @ThumbnailImageResizeMode,
	cms.MasterPage.PreviewImageWidth		= @PreviewImageWidth,
	cms.MasterPage.PreviewImageHeight		= @PreviewImageHeight,
	cms.MasterPage.PreviewImageResizeMode	= @PreviewImageResizeMode,
	cms.MasterPage.ImageMinWidth			= @ImageMinWidth,
	cms.MasterPage.ImageMinHeight			= @ImageMinHeight,
	cms.MasterPage.Creatable				= @Creatable,
	cms.MasterPage.Deletable				= @Deletable,
	cms.MasterPage.Taggable					= @Taggable,
	cms.MasterPage.Administration			= @Administration,
	cms.MasterPage.BeginRender				= @BeginRender,
	cms.MasterPage.EndRender				= @EndRender
WHERE
	cms.MasterPage.TenantId		= @TenantId AND
	cms.MasterPage.MasterPageId	= @MasterPageId