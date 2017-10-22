SET NOCOUNT ON

INSERT INTO
	element.Share (TenantId, ElementId, DisplayName, ShareOnDigg, ShareOnFacebook, ShareOnGoogle, ShareOnLinkedIn, ShareOnPinterest, ShareOnReddit, ShareOnStumbleUpon, ShareOnTumblr, ShareOnTwitter)
VALUES
	(@TenantId, @ElementId, @DisplayName, @ShareOnDigg, @ShareOnFacebook, @ShareOnGoogle, @ShareOnLinkedIn, @ShareOnPinterest, @ShareOnReddit, @ShareOnStumbleUpon, @ShareOnTumblr, @ShareOnTwitter)