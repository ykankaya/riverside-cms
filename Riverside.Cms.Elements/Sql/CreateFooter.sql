SET NOCOUNT ON

INSERT INTO
	element.Footer (TenantId, ElementId, [Message], ShowLoggedOnUserOptions, ShowLoggedOffUserOptions)
VALUES
	(@TenantId, @ElementId, @Message, @ShowLoggedOnUserOptions, @ShowLoggedOffUserOptions)