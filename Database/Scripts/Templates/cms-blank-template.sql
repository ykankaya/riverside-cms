/*----- Populate element types -----*/

DECLARE @FooterElementTypeId uniqueidentifier
DECLARE @NavBarElementTypeId uniqueidentifier
DECLARE @PageHeaderElementTypeId uniqueidentifier
SET @FooterElementTypeId = (SELECT ElementTypeId FROM cms.ElementType WHERE Name = 'Footer')
SET @NavBarElementTypeId = (SELECT ElementTypeId FROM cms.ElementType WHERE Name = 'Navigation bar')
SET @PageHeaderElementTypeId = (SELECT ElementTypeId FROM cms.ElementType WHERE Name = 'Page header')

/*----- Create "Blank" template -----*/

DECLARE @TemplateTenantId bigint
INSERT INTO cms.Tenant (Created, Updated) VALUES (GETUTCDATE(), GETUTCDATE())
SET @TemplateTenantId = SCOPE_IDENTITY()

INSERT INTO cms.Template (TenantId, Name, [Description], CreateUserEnabled, UserHasImage, UserThumbnailImageWidth, UserThumbnailImageHeight, UserThumbnailImageResizeMode, UserPreviewImageWidth, UserPreviewImageHeight, UserPreviewImageResizeMode, UserImageMinWidth, UserImageMinHeight)
VALUES (@TemplateTenantId, 'Blank', NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)

/*----- Create "Blank" elements -----*/

/*----- Navigation bar -----*/

DECLARE @NavBarElementId bigint
INSERT INTO cms.Element (TenantId, ElementTypeId, Name) VALUES (@TemplateTenantId, @NavBarElementTypeId, 'Navigation bar')
SET @NavBarElementId = SCOPE_IDENTITY()
INSERT INTO element.NavBar (TenantId, ElementId, Name, ShowLoggedOnUserOptions, ShowLoggedOffUserOptions) VALUES (@TemplateTenantId, @NavBarElementId, 'Blank', 1, 0)

/*----- Footer -----*/

DECLARE @FooterElementId bigint
INSERT INTO cms.Element (TenantId, ElementTypeId, Name) VALUES (@TemplateTenantId, @FooterElementTypeId, 'Footer')
SET @FooterElementId = SCOPE_IDENTITY()
INSERT INTO element.Footer (TenantId, ElementId, [Message], ShowLoggedOnUserOptions, ShowLoggedOffUserOptions) VALUES (@TemplateTenantId, @FooterElementId, 'Copyright © %YEAR%. All rights reserved.', 0, 1)

/*----- Page headers -----*/

DECLARE @AdminPageHeaderElementId bigint
INSERT INTO cms.Element (TenantId, ElementTypeId, Name) VALUES (@TemplateTenantId, @PageHeaderElementTypeId, 'Page header')
SET @AdminPageHeaderElementId = SCOPE_IDENTITY()
INSERT INTO element.PageHeader (TenantId, ElementId, PageTenantId, PageId, ShowName, ShowDescription, ShowImage, ShowCreated, ShowUpdated, ShowOccurred, ShowBreadcrumbs)
VALUES (@TemplateTenantId, @AdminPageHeaderElementId, NULL, NULL, 1, 1, 0, 0, 0, 0, 0)

/*----- Template pages -----*/

/*----- Home folder -----*/

DECLARE @HomeFolderTemplatePageId bigint
INSERT INTO cms.TemplatePage (TenantId, Name, PageName, PageDescription, PageType, HasOccurred, AncestorPageLevel, ParentTemplatePageId, ShowOnNavigation, Creatable, Deletable, Taggable, Administration, HasImage, ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode, ImageMinWidth, ImageMinHeight, BeginRender, EndRender)
VALUES (@TemplateTenantId, 'Home', 'Home', NULL, 0 /* PageType.Folder */, 0, NULL, NULL, 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', '')
SET @HomeFolderTemplatePageId = SCOPE_IDENTITY()

DECLARE @HomeFolderHeaderTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @HomeFolderTemplatePageId, 'Header', 0, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '', '')
SET @HomeFolderHeaderTemplatePageZoneId = SCOPE_IDENTITY()

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @HomeFolderTemplatePageId, @HomeFolderHeaderTemplatePageZoneId, 0, @NavBarElementId, NULL, NULL)

/*----- General folder -----*/

DECLARE @FolderTemplatePageId bigint
INSERT INTO cms.TemplatePage (TenantId, Name, PageName, PageDescription, PageType, HasOccurred, AncestorPageLevel, ParentTemplatePageId, ShowOnNavigation, Creatable, Deletable, Taggable, Administration, HasImage, ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode, ImageMinWidth, ImageMinHeight, BeginRender, EndRender)
VALUES (@TemplateTenantId, 'Folder', 'Folder', 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Itaque, optio corporis quae nulla aspernatur in alias at numquam rerum ea excepturi expedita tenetur.', 0 /* PageType.Folder */, 0, 1 /* PageLevel.Parent */, @HomeFolderTemplatePageId, 1, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', '')
SET @FolderTemplatePageId = SCOPE_IDENTITY()

DECLARE @FolderHeaderTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @FolderTemplatePageId, 'Header', 0, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '', '')
SET @FolderHeaderTemplatePageZoneId = SCOPE_IDENTITY()

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @FolderTemplatePageId, @FolderHeaderTemplatePageZoneId, 0, @NavBarElementId, NULL, NULL)

/*----- General document -----*/

DECLARE @DocumentTemplatePageId bigint
INSERT INTO cms.TemplatePage (TenantId, Name, PageName, PageDescription, PageType, HasOccurred, AncestorPageLevel, ParentTemplatePageId, ShowOnNavigation, Creatable, Deletable, Taggable, Administration, HasImage, ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode, ImageMinWidth, ImageMinHeight, BeginRender, EndRender)
VALUES (@TemplateTenantId, 'Document', 'Document', 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Itaque, optio corporis quae nulla aspernatur in alias at numquam rerum ea excepturi expedita tenetur.', 1 /* PageType.Document */, 0, 1 /* PageLevel.Parent */, @FolderTemplatePageId, 0, 1, 1, 1, 0, 1, 400, 400, 2 /* ResizeMode.Crop */, 1200, 1200, 1 /* ResizeMode.MaintainAspect */, 400, 400, '', '')
SET @DocumentTemplatePageId = SCOPE_IDENTITY()

DECLARE @DocumentHeaderTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @DocumentTemplatePageId,'Header', 0, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '', '')
SET @DocumentHeaderTemplatePageZoneId = SCOPE_IDENTITY()

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @DocumentTemplatePageId, @DocumentHeaderTemplatePageZoneId, 0, @NavBarElementId, NULL, NULL)

/*----- Admin page -----*/

DECLARE @AdministrationTemplatePageId bigint
INSERT INTO cms.TemplatePage (TenantId, Name, PageName, PageDescription, PageType, HasOccurred, AncestorPageLevel, ParentTemplatePageId, ShowOnNavigation, Creatable, Deletable, Taggable, Administration, HasImage, ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode, ImageMinWidth, ImageMinHeight, BeginRender, EndRender)
VALUES (@TemplateTenantId, 'Administration page', 'Administration page', NULL, 1 /* PageType.Document */, 0, 1 /* PageLevel.Parent */, @HomeFolderTemplatePageId, 0, 0, 0, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', '')
SET @AdministrationTemplatePageId = SCOPE_IDENTITY()

DECLARE @AdministrationHeaderTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, 'Header', 0, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '', '')
SET @AdministrationHeaderTemplatePageZoneId = SCOPE_IDENTITY()

DECLARE @AdministrationTitleTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, 'Title', 1, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '<div class="container"><div class="rcms-zone-title">', '</div>')
SET @AdministrationTitleTemplatePageZoneId = SCOPE_IDENTITY()

DECLARE @AdministrationMainTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, 'Main', 2, 1 /* MasterPageZoneAdminType.Editable */, 1 /* MasterPageZoneContentType.Main */, '<div class="rcms-zone-main">', '</div>')
SET @AdministrationMainTemplatePageZoneId = SCOPE_IDENTITY()

DECLARE @AdministrationFooterTemplatePageZoneId bigint
INSERT INTO cms.TemplatePageZone (TenantId, TemplatePageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, 'Footer', 3, 0 /* MasterPageZoneAdminType.Static */, 0 /* MasterPageZoneContentType.Standard */, '<div class="rcms-zone-footer">', '</div></div>')
SET @AdministrationFooterTemplatePageZoneId = SCOPE_IDENTITY()

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, @AdministrationHeaderTemplatePageZoneId, 0, @NavBarElementId, NULL, NULL)

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, @AdministrationTitleTemplatePageZoneId, 0, @AdminPageHeaderElementId, NULL, NULL)

INSERT INTO cms.TemplatePageZoneElement (TenantId, TemplatePageId, TemplatePageZoneId, SortOrder, ElementId, BeginRender, EndRender)
VALUES (@TemplateTenantId, @AdministrationTemplatePageId, @AdministrationFooterTemplatePageZoneId, 0, @FooterElementId, NULL, NULL)