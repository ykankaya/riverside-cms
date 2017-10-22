SET NOCOUNT ON

UPDATE
	element.Share
SET
	element.Share.DisplayName		 = @DisplayName,
	element.Share.ShareOnDigg		 = @ShareOnDigg,
	element.Share.ShareOnFacebook	 = @ShareOnFacebook,
	element.Share.ShareOnGoogle		 = @ShareOnGoogle,
	element.Share.ShareOnLinkedIn	 = @ShareOnLinkedIn,
	element.Share.ShareOnPinterest	 = @ShareOnPinterest,
	element.Share.ShareOnReddit		 = @ShareOnReddit,
	element.Share.ShareOnStumbleUpon = @ShareOnStumbleUpon,
	element.Share.ShareOnTumblr		 = @ShareOnTumblr,
	element.Share.ShareOnTwitter	 = @ShareOnTwitter
WHERE
	element.Share.TenantId = @TenantId AND
	element.Share.ElementId = @ElementId