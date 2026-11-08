CREATE TABLE TP_SecurityQuestion (
    QuestionID INT IDENTITY(1,1) NOT NULL,
    QuestionText VARCHAR(200) NOT NULL,
    CONSTRAINT TP_PK_SecurityQuestion PRIMARY KEY (QuestionID)
);
GO

CREATE TABLE TP_UserSecurityAnswer (
    UserID INT NOT NULL,
    QuestionID INT NOT NULL,
    AnswerHash VARCHAR(200) NOT NULL,
    CONSTRAINT TP_PK_UserSecurityAnswer PRIMARY KEY (UserID, QuestionID),
    CONSTRAINT TP_FK_UserSecurityAnswer_User FOREIGN KEY (UserID) REFERENCES TP_UserAccount (UserID),
    CONSTRAINT TP_FK_UserSecurityAnswer_Question FOREIGN KEY (QuestionID) REFERENCES TP_SecurityQuestion (QuestionID)
);
GO

INSERT INTO TP_SecurityQuestion (QuestionText) VALUES
    ('What was the name of your first pet?'),
    ('What city were you born in?'),
    ('What was the name of your elementary school?'),
    ('What is your mother''s maiden name?'),
    ('What was the make of your first car?'),
    ('What was your childhood nickname?'),
    ('What is the name of the street you grew up on?'),
    ('What was the first concert you went to?'),
    ('What is your favorite restaurant?'),
    ('What was the name of your best friend in high school?');
GO

ALTER TABLE TP_UserAccount ADD
    IsVerified BIT NOT NULL CONSTRAINT TP_DF_UserAccount_IsVerified DEFAULT (0),
    VerificationCode VARCHAR(10) NULL;
GO

ALTER TABLE TP_UserAccount ALTER COLUMN Email VARCHAR(200) NOT NULL;
GO

UPDATE TP_UserAccount SET IsVerified = 1;
GO

-- =============================================
-- Step 1.3: Account stored procedures
-- =============================================

ALTER PROCEDURE TP_AddUserAccount
    @Username varchar(50),
    @Password varchar(200),
    @UserType varchar(20),
    @FirstName varchar(50),
    @LastName varchar(50),
    @Email varchar(200),
    @Phone varchar(20)
AS
    INSERT INTO TP_UserAccount (Username, Password, UserType, FirstName, LastName, Email, Phone)
    VALUES (@Username, @Password, @UserType, @FirstName, @LastName, @Email, @Phone)

    RETURN SCOPE_IDENTITY()
GO

ALTER PROCEDURE TP_GetUserLogin
    @Username varchar(50)
AS
    SELECT UserID, Username, Password, UserType, FirstName, LastName, Email, Phone, IsVerified
    FROM TP_UserAccount
    WHERE Username = @Username
GO

CREATE PROCEDURE TP_GetSecurityQuestions
AS
    SELECT QuestionID, QuestionText
    FROM TP_SecurityQuestion
    ORDER BY QuestionID
GO

CREATE PROCEDURE TP_AddSecurityAnswer
    @UserID int,
    @QuestionID int,
    @AnswerHash varchar(200)
AS
    INSERT INTO TP_UserSecurityAnswer (UserID, QuestionID, AnswerHash)
    VALUES (@UserID, @QuestionID, @AnswerHash)
GO

CREATE PROCEDURE TP_GetUserSecurityQuestions
    @UserID int
AS
    SELECT a.QuestionID, q.QuestionText, a.AnswerHash
    FROM TP_UserSecurityAnswer a
    INNER JOIN TP_SecurityQuestion q ON a.QuestionID = q.QuestionID
    WHERE a.UserID = @UserID
GO

CREATE PROCEDURE TP_GetUserByEmail
    @Email varchar(200)
AS
    SELECT UserID, Username, UserType, FirstName, LastName, Email, Phone
    FROM TP_UserAccount
    WHERE Email = @Email
GO

CREATE PROCEDURE TP_UpdatePassword
    @UserID int,
    @Password varchar(200)
AS
    UPDATE TP_UserAccount
    SET Password = @Password
    WHERE UserID = @UserID
GO

CREATE PROCEDURE TP_SaveVerificationCode
    @UserID int,
    @Code varchar(10)
AS
    UPDATE TP_UserAccount
    SET VerificationCode = @Code
    WHERE UserID = @UserID
GO

CREATE PROCEDURE TP_VerifyAccount
    @UserID int,
    @Code varchar(10)
AS
    UPDATE TP_UserAccount
    SET IsVerified = 1, VerificationCode = NULL
    WHERE UserID = @UserID AND VerificationCode = @Code
GO
