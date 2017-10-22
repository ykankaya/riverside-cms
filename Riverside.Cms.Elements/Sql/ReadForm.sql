SET NOCOUNT ON

SELECT
	element.Form.TenantId,
	element.Form.ElementId,
	element.Form.RecipientEmail,
	element.Form.SubmitButtonLabel,
	element.Form.SubmittedMessage
FROM
	element.Form
WHERE
	element.Form.TenantId = @TenantId AND
	element.Form.ElementId = @ElementId

SELECT
	element.FormField.TenantId,
	element.FormField.ElementId,
	element.FormField.FormFieldId,
	element.FormField.SortOrder,
	element.FormField.FormFieldType,
	element.FormField.Label,
	element.FormField.[Required]
FROM
	element.FormField
WHERE
	element.FormField.TenantId = @TenantId AND
	element.FormField.ElementId = @ElementId
ORDER BY
	element.FormField.SortOrder