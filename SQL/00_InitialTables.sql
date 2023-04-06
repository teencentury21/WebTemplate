-- Users
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
CREATE TABLE Users (
    user_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) NOT NULL,
    password VARCHAR(255) NOT NULL,
    is_admin BIT NOT NULL DEFAULT 0
);

-- Functions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Functions]') AND type in (N'U'))
CREATE TABLE Functions (
    function_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    parent_function_id INT NULL,
    function_name VARCHAR(50) NOT NULL,
    function_description TEXT NULL,
    FOREIGN KEY (parent_function_id) REFERENCES Functions(function_id)
);

-- User_Functions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User_Functions]') AND type in (N'U'))
CREATE TABLE User_Functions (
    user_id INT NOT NULL,
    function_id INT NOT NULL,
    PRIMARY KEY (user_id, function_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (function_id) REFERENCES Functions(function_id)
);

-- Groups
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U'))
CREATE TABLE Groups (
    group_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    group_name VARCHAR(50) NOT NULL
);

-- Group_Functions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Group_Functions]') AND type in (N'U'))
CREATE TABLE Group_Functions (
    group_id INT NOT NULL,
    function_id INT NOT NULL,
    PRIMARY KEY (group_id, function_id),
    FOREIGN KEY (group_id) REFERENCES Groups(group_id),
    FOREIGN KEY (function_id) REFERENCES Functions(function_id)
);