SET NOCOUNT ON

INSERT INTO
	element.Form (TenantId, ElementId, RecipientEmail, SubmitButtonLabel, SubmittedMessage)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Form.RecipientEmail,
	element.Form.SubmitButtonLabel,
	element.Form.SubmittedMessage
FROM
	element.Form
WHERE
	element.Form.TenantId  = @SourceTenantId AND
	element.Form.ElementId = @SourceElementId

INSERT INTO
	element.FormField (TenantId, ElementId, SortOrder, FormFieldType, Label, [Required])
SELECT
	@DestTenantId,
	@DestElementId,
	element.FormField.SortOrder,
	element.FormField.FormFieldType,
	element.FormField.Label,
	element.FormField.[Required]
FROM
	element.FormField
WHERE
	element.FormField.TenantId	= @SourceTenantId AND
	element.FormField.ElementId = @SourceElementId