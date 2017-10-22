/*----- Populate websites -----*/

DECLARE @WebTenantId bigint
INSERT INTO cms.Tenant (Created, Updated) VALUES (GETUTCDATE(), GETUTCDATE())
SET @WebTenantId = SCOPE_IDENTITY()

DECLARE @TemplateTenantId bigint
SET @TemplateTenantId = (SELECT TenantId FROM cms.Template WHERE cms.Template.Name = 'Blank')

INSERT INTO
	cms.Web (TenantId, Name, CreateUserEnabled, UserHasImage, UserThumbnailImageWidth, UserThumbnailImageHeight, UserThumbnailImageResizeMode, UserPreviewImageWidth, UserPreviewImageHeight, UserPreviewImageResizeMode, UserImageMinWidth, UserImageMinHeight, FontOption, ColourOption)
SELECT
	@WebTenantId,
	'Riverside Test Site',
	cms.Template.CreateUserEnabled,
	cms.Template.UserHasImage,
	cms.Template.UserThumbnailImageWidth,
	cms.Template.UserThumbnailImageHeight,
	cms.Template.UserThumbnailImageResizeMode,
	cms.Template.UserPreviewImageWidth,
	cms.Template.UserPreviewImageHeight,
	cms.Template.UserPreviewImageResizeMode,
	cms.Template.UserImageMinWidth,
	cms.Template.UserImageMinHeight,
	NULL,
	NULL
FROM
	cms.Template
WHERE
	cms.Template.TenantId = @TemplateTenantId

/*----- Populate domains -----*/

INSERT INTO cms.Domain (TenantId, Url, RedirectUrl) VALUES (@WebTenantId, 'http://localhost:53459', NULL)
GO