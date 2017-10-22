SET NOCOUNT ON

UPDATE
	element.Contact
SET
	element.Contact.DisplayName = @DisplayName,
	element.Contact.Preamble = @Preamble,
	element.Contact.[Address] = @Address,
	element.Contact.Email = @Email,
	element.Contact.FacebookUsername = @FacebookUsername,
	element.Contact.InstagramUsername = @InstagramUsername,
	element.Contact.LinkedInCompanyUsername = @LinkedInCompanyUsername,
	element.Contact.LinkedInPersonalUsername = @LinkedInPersonalUsername,
	element.Contact.TelephoneNumber1 = @TelephoneNumber1,
	element.Contact.TelephoneNumber2 = @TelephoneNumber2,
	element.Contact.TwitterUsername = @TwitterUsername,
	element.Contact.YouTubeChannelId = @YouTubeChannelId
WHERE
	element.Contact.TenantId = @TenantId AND
	element.Contact.ElementId = @ElementId