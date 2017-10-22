SET NOCOUNT ON

UPDATE
	element.Form
SET
	element.Form.RecipientEmail	   = @RecipientEmail,
	element.Form.SubmitButtonLabel = @SubmitButtonLabel,
	element.Form.SubmittedMessage  = @SubmittedMessage
WHERE
	element.Form.TenantId  = @TenantId AND
	element.Form.ElementId = @ElementId

/*----- Update existing form fields -----*/

UPDATE
	element.FormField
SET
	element.FormField.SortOrder		= FormFields.SortOrder,
	element.FormField.FormFieldType = FormFields.FormFieldType,
	element.FormField.Label			= FormFields.Label,
	element.FormField.[Required]	= FormFields.[Required]
FROM
	element.FormField
INNER JOIN
	@FormFields FormFields
ON
	element.FormField.TenantId    = @TenantId AND
	element.FormField.ElementId   = @ElementId AND
	element.FormField.FormFieldId = FormFields.FormFieldId

/*----- Delete form fields that are no longer needed -----*/

DELETE
	element.FormField
FROM
	element.FormField
LEFT JOIN
	@FormFields FormFields
ON
	element.FormField.FormFieldId = FormFields.FormFieldId
WHERE
	element.FormField.TenantId  = @TenantId AND
	element.FormField.ElementId = @ElementId AND
	FormFields.FormFieldId IS NULL
	
/*----- Create new form fields -----*/

INSERT INTO
	element.FormField (TenantId, ElementId, SortOrder, FormFieldType, Label, [Required])
SELECT
	@TenantId, @ElementId, FormFields.SortOrder, FormFields.FormFieldType, FormFields.Label, FormFields.[Required]
FROM
	@FormFields FormFields
WHERE
	FormFields.FormFieldId IS NULL