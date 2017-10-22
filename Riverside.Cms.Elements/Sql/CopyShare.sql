SET NOCOUNT ON

INSERT INTO
	element.Share (TenantId, ElementId, DisplayName, ShareOnDigg, ShareOnFacebook, ShareOnGoogle, ShareOnLinkedIn, ShareOnPinterest, ShareOnReddit, ShareOnStumbleUpon, ShareOnTumblr, ShareOnTwitter)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Share.DisplayName,
	element.Share.ShareOnDigg,
	element.Share.ShareOnFacebook,
	element.Share.ShareOnGoogle,
	element.Share.ShareOnLinkedIn,
	element.Share.ShareOnPinterest,
	element.Share.ShareOnReddit,
	element.Share.ShareOnStumbleUpon,
	element.Share.ShareOnTumblr,
	element.Share.ShareOnTwitter
FROM
	element.Share
WHERE
	element.Share.TenantId  = @SourceTenantId AND
	element.Share.ElementId = @SourceElementId