CREATE PROCEDURE [cms].[SearchDomains]
	@TenantId  bigint,
	@PageIndex int,
	@PageSize  int,
	@Search    nvarchar(50)
AS

SET NOCOUNT ON

DECLARE @RowNumberLowerBound int
DECLARE @RowNumberUpperBound int
SET @RowNumberLowerBound = @PageSize * @PageIndex
SET @RowNumberUpperBound = @RowNumberLowerBound + @PageSize + 1;

WITH [Domains] AS
(
    SELECT TOP (@RowNumberUpperBound)
        ROW_NUMBER() OVER (ORDER BY cms.Domain.Url ASC) AS RowNumber,
        cms.Domain.TenantId,
		cms.Domain.DomainId
    FROM
        cms.Domain
    WHERE
		cms.Domain.TenantId = @TenantId AND
        cms.Domain.Url LIKE '%' + @Search + '%'
)

SELECT
	cms.Domain.TenantId,
	cms.Domain.DomainId,
	cms.Domain.Url,
	cms.Domain.RedirectUrl,
	cms.Web.Name
FROM
	[Domains]
INNER JOIN
	cms.Domain
ON
	[Domains].TenantId = cms.Domain.TenantId AND
	[Domains].DomainId = cms.Domain.DomainId
INNER JOIN
	cms.Web
ON
	cms.Domain.TenantId = cms.Web.TenantId
WHERE
	[Domains].RowNumber > @RowNumberLowerBound AND [Domains].RowNumber < @RowNumberUpperBound
ORDER BY
    [Domains].RowNumber ASC

SELECT
	COUNT(*) AS Total
FROM
	cms.Domain
WHERE
	cms.Domain.TenantId = @TenantId AND
	cms.Domain.Url LIKE '%' + @Search + '%'