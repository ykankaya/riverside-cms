/*----- Remove all page images -----*/

UPDATE cms.[Page] SET ImageTenantId = NULL, ThumbnailImageUploadId = NULL, PreviewImageUploadId = NULL, ImageUploadId = NULL

DELETE cms.[Image]

DELETE cms.Upload