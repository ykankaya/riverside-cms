SET NOCOUNT ON

INSERT INTO
	element.Contact (TenantId, ElementId, DisplayName, Preamble, [Address], Email, FacebookUsername, InstagramUsername, LinkedInCompanyUsername, LinkedInPersonalUsername, TelephoneNumber1, TelephoneNumber2, TwitterUsername, YouTubeChannelId)
VALUES
	(@TenantId, @ElementId, @DisplayName, @Preamble, @Address, @Email, @FacebookUsername, @InstagramUsername, @LinkedInCompanyUsername, @LinkedInPersonalUsername, @TelephoneNumber1, @TelephoneNumber2, @TwitterUsername, @YouTubeChannelId)