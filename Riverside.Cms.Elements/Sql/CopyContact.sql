SET NOCOUNT ON

INSERT INTO
	element.Contact (TenantId, ElementId, DisplayName, Preamble, [Address], Email, FacebookUsername, InstagramUsername, LinkedInCompanyUsername, LinkedInPersonalUsername, TelephoneNumber1, TelephoneNumber2, TwitterUsername, YouTubeChannelId)
SELECT
	@DestTenantId,
	@DestElementId,
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
	element.Contact.TenantId  = @SourceTenantId AND
	element.Contact.ElementId = @SourceElementId