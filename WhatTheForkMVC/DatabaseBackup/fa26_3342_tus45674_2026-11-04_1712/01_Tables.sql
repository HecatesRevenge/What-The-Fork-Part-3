-- Backup of [fa26_3342_tus45674] on cis-mssql1.temple.edu, 2026-11-04 17:12
-- Run this first.

CREATE TABLE [dbo].[TP_Cuisines] (
    [CuisineID] INT IDENTITY(1,1) NOT NULL,
    [CuisineName] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK__Cuisines__B1C3E7ABA83F20DB] PRIMARY KEY CLUSTERED ([CuisineID])
);
GO

CREATE TABLE [dbo].[Customer] (
    [customerID] INT NOT NULL,
    [firstName] VARBINARY(50) NOT NULL,
    [lastName] VARCHAR(50) NOT NULL,
    [phone] VARCHAR(50) NOT NULL,
    [email] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK__Table__B611CB9D66E7AE89] PRIMARY KEY CLUSTERED ([customerID])
);
GO

CREATE TABLE [dbo].[Reservation] (
    [ReservationID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [UserID] INT NULL,
    [ContactName] VARCHAR(100) NOT NULL,
    [Phone] VARCHAR(20) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [ReservationDate] DATETIME NOT NULL,
    [PartySize] INT NOT NULL,
    [SpecialRequests] VARCHAR(1000) NULL,
    [Status] VARCHAR(20) NOT NULL CONSTRAINT [DF__Reservati__Statu__0F624AF8] DEFAULT ('Pending'),
    [DateCreated] DATETIME NOT NULL CONSTRAINT [DF__Reservati__DateC__10566F31] DEFAULT (getdate()),
    CONSTRAINT [PK__Reservat__B7EE5F04407850A3] PRIMARY KEY CLUSTERED ([ReservationID])
);
GO

CREATE TABLE [dbo].[TP_RestaurantHours] (
    [RestaurantHoursID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [DayOfWeek] TINYINT NOT NULL,
    [OpenTime] TIME(0) NULL,
    [CloseTime] TIME(0) NULL,
    [IsClosed] BIT NOT NULL CONSTRAINT [DF_RestaurantHours_IsClosed] DEFAULT ((0)),
    CONSTRAINT [PK_RestaurantHours] PRIMARY KEY CLUSTERED ([RestaurantHoursID]),
    CONSTRAINT [UQ_RestaurantHours_Day] UNIQUE NONCLUSTERED ([RestaurantID], [DayOfWeek]),
    CONSTRAINT [CK_RestaurantHours_DayOfWeek] CHECK ([DayOfWeek]>=(1) AND [DayOfWeek]<=(7))
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
    [DateUploaded] DATETIME NOT NULL CONSTRAINT [DF__Restauran__DateU__0A9D95DB] DEFAULT (getdate()),
    CONSTRAINT [PK__Restaura__21B7B5826B07692B] PRIMARY KEY CLUSTERED ([PhotoID])
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
    CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED ([RestaurantID]),
    CONSTRAINT [CK_Restaurant_PriceRange] CHECK ([PriceRange]>=(1) AND [PriceRange]<=(4))
);
GO

CREATE TABLE [dbo].[TP_RestaurantSocialLink] (
    [SocialLinkID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [PlatformID] INT NOT NULL,
    [LinkUrl] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_RestaurantSocialLink] PRIMARY KEY CLUSTERED ([SocialLinkID]),
    CONSTRAINT [UQ_RestaurantSocialLink] UNIQUE NONCLUSTERED ([RestaurantID], [PlatformID])
);
GO

CREATE TABLE [dbo].[Review] (
    [ReviewID] INT IDENTITY(1,1) NOT NULL,
    [RestaurantID] INT NOT NULL,
    [ReviewerID] INT NOT NULL,
    [ReviewDate] DATETIME NOT NULL CONSTRAINT [DF_Review_ReviewDate] DEFAULT (getdate()),
    [ReviewTitle] VARCHAR(100) NOT NULL,
    [ReviewText] VARCHAR(4000) NOT NULL,
    [FoodRating] TINYINT NOT NULL,
    [ServiceRating] TINYINT NOT NULL,
    [PriceRating] TINYINT NOT NULL,
    [HelpfulCount] INT NOT NULL CONSTRAINT [DF_Review_HelpfulCount] DEFAULT ((0)),
    [NotHelpfulCount] INT NOT NULL CONSTRAINT [DF_Review_NotHelpfulCount] DEFAULT ((0)),
    [AtmosphereRating] TINYINT NULL,
    [VisitDate] DATETIME NULL,
    [ReviewPhoto] VARBINARY(MAX) NULL,
    [ReviewPhotoType] VARCHAR(50) NULL,
    [ResponseText] VARCHAR(2000) NULL,
    [ResponseDate] DATETIME NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED ([ReviewID]),
    CONSTRAINT [CK_Review_AtmosphereRating] CHECK ([AtmosphereRating] IS NULL OR [AtmosphereRating]>=(1) AND [AtmosphereRating]<=(5)),
    CONSTRAINT [CK_Review_FoodRating] CHECK ([FoodRating]>=(1) AND [FoodRating]<=(5)),
    CONSTRAINT [CK_Review_HelpfulCount] CHECK ([HelpfulCount]>=(0)),
    CONSTRAINT [CK_Review_NotHelpfulCount] CHECK ([NotHelpfulCount]>=(0)),
    CONSTRAINT [CK_Review_PriceRating] CHECK ([PriceRating]>=(1) AND [PriceRating]<=(5)),
    CONSTRAINT [CK_Review_ReviewPhotoSize] CHECK ([ReviewPhoto] IS NULL OR datalength([ReviewPhoto])<=(1048576)),
    CONSTRAINT [CK_Review_ServiceRating] CHECK ([ServiceRating]>=(1) AND [ServiceRating]<=(5))
);
GO

CREATE TABLE [dbo].[Reviewer] (
    [ReviewerID] INT IDENTITY(1,1) NOT NULL,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [HomeCity] VARCHAR(60) NULL,
    [HomeState] CHAR(2) NULL,
    [DateJoined] DATETIME NOT NULL CONSTRAINT [DF_Reviewer_DateJoined] DEFAULT (getdate()),
    [UserID] INT NULL,
    CONSTRAINT [PK_Reviewer] PRIMARY KEY CLUSTERED ([ReviewerID])
);
GO

CREATE TABLE [dbo].[SocialMediaPlatform] (
    [PlatformID] INT IDENTITY(1,1) NOT NULL,
    [PlatformName] VARCHAR(30) NOT NULL,
    CONSTRAINT [PK_SocialMediaPlatform] PRIMARY KEY CLUSTERED ([PlatformID]),
    CONSTRAINT [UQ_SocialMediaPlatform_PlatformName] UNIQUE NONCLUSTERED ([PlatformName])
);
GO

CREATE TABLE [dbo].[UserAccount] (
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Username] VARCHAR(50) NOT NULL,
    [Password] VARCHAR(200) NOT NULL,
    [UserType] VARCHAR(20) NOT NULL,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [Phone] VARCHAR(20) NULL,
    [DateCreated] DATETIME NOT NULL CONSTRAINT [DF_UserAccount_DateCreated] DEFAULT (getdate()),
    CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED ([UserID]),
    CONSTRAINT [UQ_UserAccount_Username] UNIQUE NONCLUSTERED ([Username]),
    CONSTRAINT [CK_UserAccount_UserType] CHECK ([UserType]='Representative' OR [UserType]='Reviewer')
);
GO

-- Foreign keys

ALTER TABLE [dbo].[Reservation] ADD CONSTRAINT [FK__Reservati__Resta__0D7A0286] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]);
GO
ALTER TABLE [dbo].[Reservation] ADD CONSTRAINT [FK__Reservati__UserI__0E6E26BF] FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserAccount] ([UserID]);
GO
ALTER TABLE [dbo].[TP_RestaurantPhoto] ADD CONSTRAINT [FK__Restauran__Resta__09A971A2] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]);
GO
ALTER TABLE [dbo].[TP_Restaurants] ADD CONSTRAINT [FK_Restaurant_Cuisine] FOREIGN KEY ([CuisineID]) REFERENCES [dbo].[TP_Cuisines] ([CuisineID]);
GO
ALTER TABLE [dbo].[TP_RestaurantHours] ADD CONSTRAINT [FK_RestaurantHours_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[TP_RestaurantSocialLink] ADD CONSTRAINT [FK_RestaurantSocialLink_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[SocialMediaPlatform] ([PlatformID]);
GO
ALTER TABLE [dbo].[TP_RestaurantSocialLink] ADD CONSTRAINT [FK_RestaurantSocialLink_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[Review] ADD CONSTRAINT [FK_Review_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[TP_Restaurants] ([RestaurantID]) ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[Review] ADD CONSTRAINT [FK_Review_Reviewer] FOREIGN KEY ([ReviewerID]) REFERENCES [dbo].[Reviewer] ([ReviewerID]);
GO
ALTER TABLE [dbo].[Reviewer] ADD CONSTRAINT [FK_Reviewer_UserAccount] FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserAccount] ([UserID]);
GO
