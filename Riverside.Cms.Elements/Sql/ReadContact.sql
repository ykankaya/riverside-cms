SET NOCOUNT ON

SELECT
	element.Contact.TenantId,
	element.Contact.ElementId,
	element.Contact.DisplayName,
	element.Contact.Preamble,
	element.Contact.[Address],
	element.Contact.Email,
	element.Contact.FacebookUsername,
	element.Contact.InstagramUsername,
	element.Contact.LinkedInCompanyUsername,
	element.Contact.LinkedInPersonalUsername,
	element.Contact.TelephoneNumber1,
	element.Contact.TelephoneNumber2,
	element.Contact.TwitterUsername,
	element.Contact.YouTubeChannelId
FROM
	element.Contact
WHERE
	element.Contact.TenantId = @TenantId AND
	element.Contact.ElementId = @ElementId