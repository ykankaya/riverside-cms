SET NOCOUNT ON

SELECT
	element.Share.TenantId,
	element.Share.ElementId,
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
	element.Share.TenantId = @TenantId AND
	element.Share.ElementId = @ElementId