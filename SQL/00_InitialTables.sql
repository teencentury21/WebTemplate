USE Northwind

-- Users
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U')) DROP TABLE [dbo].[Users]
CREATE TABLE Users(
	user_id INT IDENTITY, 
	username NVARCHAR(50) NOT NULL ,
	userno VARCHAR(20) NOT NULL ,
	password VARCHAR(255) NOT NULL ,
	is_active BIT NOT NULL ,
	is_admin BIT NOT NULL ,
	email VARCHAR(200) NOT NULL ,
	role VARCHAR(10) NOT NULL ,
	lastlogin DATETIME2 NOT NULL,
	setting VARCHAR(1000) NOT NULL ,
	remark VARCHAR(200) NOT NULL ,
	cdt DATETIME2 NOT NULL ,
	udt DATETIME2 NOT NULL ,	
);

ALTER TABLE Users ADD CONSTRAINT df_Users_username DEFAULT '' FOR username
ALTER TABLE Users ADD CONSTRAINT df_Users_userno DEFAULT '' FOR userno
ALTER TABLE Users ADD CONSTRAINT df_Users_password DEFAULT '' FOR password
ALTER TABLE Users ADD CONSTRAINT df_Users_is_active DEFAULT 0 FOR is_active
ALTER TABLE Users ADD CONSTRAINT df_Users_is_admin DEFAULT 0 FOR is_admin
ALTER TABLE Users ADD CONSTRAINT df_Users_email DEFAULT '' FOR email
ALTER TABLE Users ADD CONSTRAINT df_Users_role DEFAULT '' FOR role
ALTER TABLE Users ADD CONSTRAINT df_Users_lastlogin DEFAULT GETDATE() FOR lastlogin
ALTER TABLE Users ADD CONSTRAINT df_Users_setting DEFAULT '' FOR setting
ALTER TABLE Users ADD CONSTRAINT df_Users_remark DEFAULT '' FOR remark
ALTER TABLE Users ADD CONSTRAINT df_Users_cdt DEFAULT GETDATE() FOR cdt
ALTER TABLE Users ADD CONSTRAINT df_Users_udt DEFAULT GETDATE() FOR udt

INSERT INTO Users (username, userno, password, is_active, is_admin, email, role, lastlogin, setting, remark, cdt, udt)
VALUES('admin', 'admin', 'd75aa8a2058c87b1fa97e5821735cb0586d19cb0e0c5f173c40b84fb2c2c2621', 1, 1, 'admin@darfon.com.tw', '', GETDATE(), '', '', GETDATE(), GETDATE())

-- Functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Functions]') AND type in (N'U')) DROP TABLE [dbo].[Functions]
CREATE TABLE Functions (
    function_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    parent_function_id INT NULL,
    function_name VARCHAR(50) NOT NULL,
    function_description TEXT NULL,
	editor VARCHAR(50) NOT NULL ,
	cdt DATETIME2 NOT NULL ,
	udt DATETIME2 NOT NULL ,
    FOREIGN KEY (parent_function_id) REFERENCES Functions(function_id)
);
ALTER TABLE Functions ADD CONSTRAINT df_Functions_function_description DEFAULT 'System' FOR function_description
ALTER TABLE Functions ADD CONSTRAINT df_Functions_editor DEFAULT 'System' FOR editor
ALTER TABLE Functions ADD CONSTRAINT df_Functions_cdt DEFAULT GETDATE() FOR cdt
ALTER TABLE Functions ADD CONSTRAINT df_Functions_udt DEFAULT GETDATE() FOR udt

-- User_Functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User_Functions]') AND type in (N'U')) DROP TABLE [dbo].[User_Functions]
CREATE TABLE User_Functions (
    user_id INT NOT NULL,
    function_id INT NOT NULL,
	editor VARCHAR(50) NOT NULL ,
    cdt DATETIME2 NOT NULL ,
	udt DATETIME2 NOT NULL ,
	--PRIMARY KEY (user_id, function_id),
    --FOREIGN KEY (user_id) REFERENCES Users(user_id),
    --FOREIGN KEY (function_id) REFERENCES Functions(function_id)
);
ALTER TABLE User_Functions ADD CONSTRAINT df_User_Functions_editor DEFAULT 'System' FOR editor
ALTER TABLE User_Functions ADD CONSTRAINT df_User_Functions_cdt DEFAULT GETDATE() FOR cdt
ALTER TABLE User_Functions ADD CONSTRAINT df_User_Functions_udt DEFAULT GETDATE() FOR udt

-- Groups
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U')) DROP TABLE [dbo].[Groups]
CREATE TABLE Groups (
    group_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    group_name VARCHAR(50) NOT NULL,
    group_description TEXT NULL,
	editor VARCHAR(50) NOT NULL ,
	cdt DATETIME2 NOT NULL ,
	udt DATETIME2 NOT NULL ,
);
ALTER TABLE Groups ADD CONSTRAINT df_Groups_group_description DEFAULT '' FOR group_description
ALTER TABLE Groups ADD CONSTRAINT df_Groups_editor DEFAULT 'System' FOR editor
ALTER TABLE Groups ADD CONSTRAINT df_Groups_cdt DEFAULT GETDATE() FOR cdt
ALTER TABLE Groups ADD CONSTRAINT df_Groups_udt DEFAULT GETDATE() FOR udt

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Group_Functions]') AND type in (N'U')) DROP TABLE [dbo].[Group_Functions] 
CREATE TABLE Group_Functions (
    group_id INT NOT NULL,
    function_id INT NOT NULL,
	editor VARCHAR(50) NOT NULL ,
	cdt DATETIME2 NOT NULL ,
	udt DATETIME2 NOT NULL ,
    --PRIMARY KEY (group_id, function_id),
    --FOREIGN KEY (group_id) REFERENCES Groups(group_id),
    --FOREIGN KEY (function_id) REFERENCES Functions(function_id)
);

ALTER TABLE Group_Functions ADD CONSTRAINT df_Group_Functions_editor DEFAULT 'System' FOR editor
ALTER TABLE Group_Functions ADD CONSTRAINT df_Group_Functions_cdt DEFAULT GETDATE() FOR cdt
ALTER TABLE Group_Functions ADD CONSTRAINT df_Group_Functions_udt DEFAULT GETDATE() FOR udt


-- INI
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INI]') AND type in (N'U')) DROP TABLE [dbo].[INI] 
CREATE TABLE INI (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Item VARCHAR(50) NOT NULL,
    Data VARCHAR(3000) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    Editor VARCHAR(10) NOT NULL,
    Cdt DATETIME NOT NULL DEFAULT GETDATE(),
    Udt DATETIME NOT NULL DEFAULT GETDATE()
);
INSERT INTO INI (Item, Data, Description, Editor, Cdt, Udt) VALUES ('Domain','Darfon','AD Login Domain','System',GETDATE(),GETDATE())
INSERT INTO INI (Item, Data, Description, Editor, Cdt, Udt) VALUES ('GAIA','N','Connect with GAIA','System',GETDATE(),GETDATE())
INSERT INTO INI (Item, Data, Description, Editor, Cdt, Udt) VALUES ('SHAKey','DFE','SHAKey','System',GETDATE(),GETDATE())
INSERT INTO INI (Item, Data, Description, Editor, Cdt, Udt) VALUES ('j04m/','d871cfd11d4e9d8024d6491b60ea62a24c3d5b149b5fb71238271632c0b31659','SHA Result','System',GETDATE(),GETDATE())


-- Transaction_Log
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transaction_Log]') AND type in (N'U')) DROP TABLE [dbo].[Transaction_Log] 
CREATE TABLE Transaction_Log(
	Id UNIQUEIDENTIFIER NOT NULL ,
	Application_Name VARCHAR(50) NOT NULL ,
	Data VARCHAR(1000) NOT NULL ,
	Description NVARCHAR(4000) NOT NULL ,
	Editor VARCHAR(50) NOT NULL ,
	Message NVARCHAR(4000) NOT NULL ,
	Cdt DATETIME NOT NULL ,
)

ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Id DEFAULT NEWID() FOR Id
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Application_Name DEFAULT '' FOR Application_Name
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Data DEFAULT '' FOR Data
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Description DEFAULT '' FOR Description
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Editor DEFAULT '' FOR Editor
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Message DEFAULT '' FOR Message
ALTER TABLE Transaction_Log ADD CONSTRAINT df_Transaction_Log_Cdt DEFAULT GETDATE() FOR Cdt

SELECT * FROM Users
SELECT * FROM Groups
SELECT * FROM Functions
SELECT * FROM User_Functions
SELECT * FROM Group_Functions
SELECT * FROM INI
SELECT * FROM Transaction_Log