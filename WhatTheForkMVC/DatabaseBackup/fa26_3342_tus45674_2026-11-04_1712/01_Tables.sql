-- What the Fork, Project 3: every table and stored procedure renamed with the TP_ prefix.
-- Creates new TP_ objects next to the existing ones; nothing existing is changed or dropped.
-- Backup of [fa26_3342_tus45674] on cis-mssql1.temple.edu, 2026-11-04 17:12
-- Run this first.

CREATE TABLE [dbo].[TP_Cuisines] (
    [CuisineID] INT IDENTITY(1,1) NOT NULL,
    [CuisineName] VARCHAR(50) NOT NULL,
    CONSTRAINT [TP_PK__Cuisines__B1C3E7ABA83F20DB] PRIMARY KEY CLUSTERED ([CuisineID])
);
GO

CREATE TABLE [dbo].[TP_Reservation] (
    [ReservationID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [UserID] INT NULL,
    [ContactName] VARCHAR(100) NOT NULL,
    [Phone] VARCHAR(20) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [ReservationDate] DATETIME NOT NULL,
    [PartySize] INT NOT NULL,
    [SpecialRequests] VARCHAR(1000) NULL,
    [Status] VARCHAR(20) NOT NULL CONSTRAINT [TP_DF__Reservati__Statu__0F624AF8] DEFAULT ('Pending'),
    [DateCreated] DATETIME NOT NULL CONSTRAINT [TP_DF__Reservati__DateC__10566F31] DEFAULT (getdate()),
    CONSTRAINT [TP_PK__Reservat__B7EE5F04407850A3] PRIMARY KEY CLUSTERED ([ReservationID])
);
GO

CREATE TABLE [dbo].[TP_RestaurantHours] (
    [RestaurantHoursID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [DayOfWeek] TINYINT NOT NULL,
    [OpenTime] TIME(0) NULL,
    [CloseTime] TIME(0) NULL,
    [IsClosed] BIT NOT NULL CONSTRAINT [TP_DF_RestaurantHours_IsClosed] DEFAULT ((0)),
    CONSTRAINT [TP_PK_RestaurantHours] PRIMARY KEY CLUSTERED ([RestaurantHoursID]),
    CONSTRAINT [TP_UQ_RestaurantHours_Day] UNIQUE NONCLUSTERED ([RestaurantID], [DayOfWeek]),
    CONSTRAINT [TP_CK_RestaurantHours_DayOfWeek] CHECK ([DayOfWeek]>=(1) AND [DayOfWeek]<=(7))
);
GO

CREATE TABLE [dbo].[TP_RestaurantPhoto] (
    [PhotoID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [PhotoType] VARCHAR(20) NOT NULL,
    [Caption] VARCHAR(200) NULL,
    [ImageData] VARBINARY(MAX) NOT NULL,
    [ImageType] VARCHAR(50) NOT NULL,
    [ImageLength] INT NOT NULL,
    [DateUploaded] DATETIME NOT NULL CONSTRAINT [TP_DF__Restauran__DateU__0A9D95DB] DEFAULT (getdate()),
    CONSTRAINT [TP_PK__Restaura__21B7B5826B07692B] PRIMARY KEY CLUSTERED ([PhotoID])
);
GO

CREATE TABLE [dbo].[TP_SerRestaurants](
[ID] INT NOT NULL,
[Restaurant] varbinary(max),
[Rep] int
);
GO

CREATE TABLE [dbo].[TP_SerUsers](
[ID] INT NOT NULL,
[Account] varbinary(max)
);
GO

CREATE TABLE [dbo].[TP_SerReviews](
[ID] INT NOT NULL,
[RestID] INT,
[Reviewer] INT,
[Review] varbinary(max)
);
GO

CREATE TABLE [dbo].[TP_SerReservations](
[ID] INT NOT NULL,
[restID] INT,
[Reservation] varbinary(max)
);
GO

CREATE TABLE [dbo].[TP_Restaurants] (
    [RestaurantID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantName] VARCHAR(50) NOT NULL,
    [CuisineID] INT NOT NULL,
    [Description] VARCHAR(1000) NOT NULL,
    [StreetAddress] VARCHAR(200) NOT NULL,
    [City] VARCHAR(60) NOT NULL,
    [State] CHAR(2) NOT NULL,
    [ZipCode] VARCHAR(20) NOT NULL,
    [Phone] VARCHAR(20) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [OwnerName] VARCHAR(100) NOT NULL,
    [WebsiteURL] VARCHAR(200) NOT NULL,
    [PriceRange] INT NOT NULL,
    [LogoImagePath] VARCHAR(200) NULL,
    [CoverImagePath] VARCHAR(200) NULL,
    [DateAdded] DATETIME NOT NULL,
    [RepresentativeUserID] INT NULL,
    CONSTRAINT [TP_PK_Restaurant] PRIMARY KEY CLUSTERED ([RestaurantID]),
    CONSTRAINT [TP_CK_Restaurant_PriceRange] CHECK ([PriceRange]>=(1) AND [PriceRange]<=(4))
);
GO

CREATE TABLE [dbo].[TP_RestaurantSocialLink] (
    [SocialLinkID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [PlatformID] INT NOT NULL,
    [LinkUrl] VARCHAR(200) NOT NULL,
    CONSTRAINT [TP_PK_RestaurantSocialLink] PRIMARY KEY CLUSTERED ([SocialLinkID]),
    CONSTRAINT [TP_UQ_RestaurantSocialLink] UNIQUE NONCLUSTERED ([RestaurantID], [PlatformID])
);
GO

CREATE TABLE [dbo].[TP_Review] (
    [ReviewID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [ReviewerID] INT NOT NULL,
    [ReviewDate] DATETIME NOT NULL CONSTRAINT [TP_DF_Review_ReviewDate] DEFAULT (getdate()),
    [ReviewTitle] VARCHAR(100) NOT NULL,
    [ReviewText] VARCHAR(4000) NOT NULL,
    [FoodRating] TINYINT NOT NULL,
    [ServiceRating] TINYINT NOT NULL,
    [PriceRating] TINYINT NOT NULL,
    [HelpfulCount] INT NOT NULL CONSTRAINT [TP_DF_Review_HelpfulCount] DEFAULT ((0)),
    [NotHelpfulCount] INT NOT NULL CONSTRAINT [TP_DF_Review_NotHelpfulCount] DEFAULT ((0)),
    [AtmosphereRating] TINYINT NULL,
    [VisitDate] DATETIME NULL,
    [ReviewPhoto] VARBINARY(MAX) NULL,
    [ReviewPhotoType] VARCHAR(50) NULL,
    [ResponseText] VARCHAR(2000) NULL,
    [ResponseDate] DATETIME NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [TP_PK_Review] PRIMARY KEY CLUSTERED ([ReviewID]),
    CONSTRAINT [TP_CK_Review_AtmosphereRating] CHECK ([AtmosphereRating] IS NULL OR [AtmosphereRating]>=(1) AND [AtmosphereRating]<=(5)),
    CONSTRAINT [TP_CK_Review_FoodRating] CHECK ([FoodRating]>=(1) AND [FoodRating]<=(5)),
    CONSTRAINT [TP_CK_Review_HelpfulCount] CHECK ([HelpfulCount]>=(0)),
    CONSTRAINT [TP_CK_Review_NotHelpfulCount] CHECK ([NotHelpfulCount]>=(0)),
    CONSTRAINT [TP_CK_Review_PriceRating] CHECK ([PriceRating]>=(1) AND [PriceRating]<=(5)),
    CONSTRAINT [TP_CK_Review_ReviewPhotoSize] CHECK ([ReviewPhoto] IS NULL OR datalength([ReviewPhoto])<=(1048576)),
    CONSTRAINT [TP_CK_Review_ServiceRating] CHECK ([ServiceRating]>=(1) AND [ServiceRating]<=(5))
);
GO

CREATE TABLE [dbo].[TP_Reviewer] (
    [ReviewerID] INT IDENTITY(1,1) NOT NULL,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [HomeCity] VARCHAR(60) NULL,
    [HomeState] CHAR(2) NULL,
    [DateJoined] DATETIME NOT NULL CONSTRAINT [TP_DF_Reviewer_DateJoined] DEFAULT (getdate()),
    [UserID] INT NULL,
    CONSTRAINT [TP_PK_Reviewer] PRIMARY KEY CLUSTERED ([ReviewerID])
);
GO

CREATE TABLE [dbo].[TP_SocialMediaPlatform] (
    [PlatformID] INT IDENTITY(1,1) NOT NULL,
    [PlatformName] VARCHAR(30) NOT NULL,
    CONSTRAINT [TP_PK_SocialMediaPlatform] PRIMARY KEY CLUSTERED ([PlatformID]),
    CONSTRAINT [TP_UQ_SocialMediaPlatform_PlatformName] UNIQUE NONCLUSTERED ([PlatformName])
);
GO

CREATE TABLE [dbo].[TP_UserAccount] (
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Username] VARCHAR(50) NOT NULL,
    [Password] VARCHAR(200) NOT NULL,
    [UserType] VARCHAR(20) NOT NULL,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [Phone] VARCHAR(20) NULL,
    [DateCreated] DATETIME NOT NULL CONSTRAINT [TP_DF_UserAccount_DateCreated] DEFAULT (getdate()),
    CONSTRAINT [TP_PK_UserAccount] PRIMARY KEY CLUSTERED ([UserID]),
    CONSTRAINT [TP_UQ_UserAccount_Username] UNIQUE NONCLUSTERED ([Username]),
    CONSTRAINT [TP_CK_UserAccount_UserType] CHECK ([UserType]='Representative' OR [UserType]='Reviewer')
);
GO

-- Foreign keys

ALTER TABLE [dbo].[TP_Reservation] ADD CONSTRAINT [TP_FK__Reservati__Resta__0D7A0286] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]);
GO
ALTER TABLE [dbo].[TP_Reservation] ADD CONSTRAINT [TP_FK__Reservati__UserI__0E6E26BF] FOREIGN KEY ([UserID]) REFERENCES [dbo].[TP_UserAccount] ([UserID]);
GO
ALTER TABLE [dbo].[TP_RestaurantPhoto] ADD CONSTRAINT [TP_FK__Restauran__Resta__09A971A2] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]);
GO
ALTER TABLE [dbo].[TP_Restaurants] ADD CONSTRAINT [TP_FK_Restaurant_Cuisine] FOREIGN KEY ([CuisineID]) REFERENCES [dbo].[TP_Cuisines] ([CuisineID]);
GO
ALTER TABLE [dbo].[TP_RestaurantHours] ADD CONSTRAINT [TP_FK_RestaurantHours_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[TP_RestaurantSocialLink] ADD CONSTRAINT [TP_FK_RestaurantSocialLink_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[TP_SocialMediaPlatform] ([PlatformID]);
GO
ALTER TABLE [dbo].[TP_RestaurantSocialLink] ADD CONSTRAINT [TP_FK_RestaurantSocialLink_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[TP_Review] ADD CONSTRAINT [TP_FK_Review_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[TP_Review] ADD CONSTRAINT [TP_FK_Review_Reviewer] FOREIGN KEY ([ReviewerID]) REFERENCES [dbo].[TP_Reviewer] ([ReviewerID]);
GO
ALTER TABLE [dbo].[TP_Reviewer] ADD CONSTRAINT [TP_FK_Reviewer_UserAccount] FOREIGN KEY ([UserID]) REFERENCES [dbo].[TP_UserAccount] ([UserID]);
GO
