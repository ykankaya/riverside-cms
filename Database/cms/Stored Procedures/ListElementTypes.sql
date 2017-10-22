CREATE PROCEDURE [cms].[ListElementTypes]
AS

SELECT
	cms.ElementType.ElementTypeId,
	cms.ElementType.Name
FROM
	cms.ElementType
ORDER BY
	cms.ElementType.Name