SET NOCOUNT ON

SELECT
	cms.[Role].RoleId,
	cms.[Role].Name
FROM
	cms.[Role]
ORDER BY
	cms.[Role].Name

SELECT
	cms.[RoleFunction].RoleId,
	cms.[RoleFunction].FunctionId,
	cms.[Function].Name
FROM
	cms.[RoleFunction]
INNER JOIN
	cms.[Function]
ON
	cms.[RoleFunction].FunctionId = cms.[Function].FunctionId
ORDER BY
	cms.[RoleFunction].RoleId,
	cms.[Function].Name