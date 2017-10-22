/*----- Populate functions -----*/

INSERT INTO cms.[Function] (FunctionId, Name) VALUES (1, 'UpdatePageElements')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (2, 'UpdateMasterPageElements')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (3, 'CreatePages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (4, 'CreateMasterPages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (5, 'UpdatePages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (6, 'UpdateMasterPages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (7, 'DeletePages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (8, 'DeleteMasterPages')
INSERT INTO cms.[Function] (FunctionId, Name) VALUES (9, 'UpdateTheme')
GO

/*----- Populate roles -----*/

INSERT INTO cms.[Role] (RoleId, Name) VALUES (1, 'User')
INSERT INTO cms.[Role] (RoleId, Name) VALUES (2, 'Editor')
INSERT INTO cms.[Role] (RoleId, Name) VALUES (3, 'EditorInChief')
INSERT INTO cms.[Role] (RoleId, Name) VALUES (4, 'Administrator')
GO

/*----- Map functions to roles -----*/

/*----- Users can't perform any CMS administration tasks -----*/

/*----- Editors can update page content, create pages, updates pages and delete pages -----*/

INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (2, 1)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (2, 3)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (2, 5)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (2, 7)
GO

/*----- Editor in chiefs can do what an editor can do, plus update site content -----*/

INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (3, 1)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (3, 2)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (3, 3)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (3, 5)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (3, 7)
GO

/*----- Administrators can do what editor in chiefs can do, plus create master pages, update master pages and delete master pages -----*/

INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 1)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 2)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 3)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 4)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 5)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 6)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 7)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 8)
INSERT INTO cms.RoleFunction (RoleId, FunctionId) VALUES (4, 9)
GO
