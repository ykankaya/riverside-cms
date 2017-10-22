SET NOCOUNT ON

INSERT INTO
	element.Form (TenantId, ElementId, RecipientEmail, SubmitButtonLabel, SubmittedMessage)
VALUES
	(@TenantId, @ElementId, @RecipientEmail, @SubmitButtonLabel, @SubmittedMessage)

INSERT INTO
	element.FormField (TenantId, ElementId, SortOrder, FormFieldType, Label, [Required])
SELECT
	@TenantId, @ElementId, FormFields.SortOrder, FormFields.FormFieldType, FormFields.Label, FormFields.[Required]
FROM
	@FormFields FormFields