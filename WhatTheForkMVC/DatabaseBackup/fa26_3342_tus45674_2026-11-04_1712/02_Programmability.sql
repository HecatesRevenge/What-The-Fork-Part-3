-- Backup of [fa26_3342_tus45674] on cis-mssql1.temple.edu, 2026-11-04 17:12
-- Run this after 01_Tables.sql.

-- SQL_STORED_PROCEDURE: [dbo].[GetCuisines]
-- =============================================
-- What the Fork - Project 2
-- Script 04: Stored procedures for the Search page

CREATE PROCEDURE TP_GetCuisines
AS
    SELECT CuisineID, CuisineName
    FROM TP_Cuisines
    ORDER BY CuisineName
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetLocations]
CREATE PROCEDURE TP_GetLocations
AS
    SELECT City, State, COUNT(*) AS RestaurantCount
    FROM TP_Restaurants
    GROUP BY City, State
    ORDER BY City, State
GO

-- SQL_STORED_PROCEDURE: [dbo].[SearchRestaurants]
CREATE PROCEDURE TP_SearchRestaurants
    @CuisineList varchar(500),
    @City varchar(60),
    @State char(2)
AS
    SELECT r.RestaurantID, r.RestaurantName, c.CuisineName, r.LogoImagePath, r.PriceRange,
           r.City + ', ' + r.State AS Location,
           COUNT(rv.ReviewID) AS ReviewCount,
           AVG((rv.FoodRating + rv.ServiceRating + rv.AtmosphereRating) / 3.0) AS OverallRating,
           AVG(rv.FoodRating * 1.0) AS FoodRating,
           AVG(rv.ServiceRating * 1.0) AS ServiceRating,
           AVG(rv.AtmosphereRating * 1.0) AS AtmosphereRating,
           AVG(rv.PriceRating * 1.0) AS PriceRating
    FROM TP_Restaurants r
    INNER JOIN Cuisines c ON r.CuisineID = c.CuisineID
    LEFT JOIN Review rv ON r.RestaurantID = rv.RestaurantID
    WHERE ',' + @CuisineList + ',' LIKE '%,' + CAST(r.CuisineID AS varchar) + ',%'
      AND (@City = '' OR r.City = @City)
      AND (@State = '' OR r.State = @State)
    GROUP BY r.RestaurantID, r.RestaurantName, c.CuisineName, r.LogoImagePath, r.PriceRange, r.City, r.State
    ORDER BY OverallRating DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantLogo]
CREATE PROCEDURE TP_GetRestaurantLogo
    @RestaurantID int
AS
    SELECT ImageData, ImageType
    FROM RestaurantPhoto
    WHERE RestaurantID = @RestaurantID AND PhotoType = 'Logo'
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantByID]
CREATE PROCEDURE TP_GetRestaurantByID
    @RestaurantID int
AS
    SELECT r.RestaurantID, r.RestaurantName, r.CuisineID, c.CuisineName, r.Description,
           r.StreetAddress, r.City, r.State, r.ZipCode, r.Phone, r.Email, r.WebsiteUrl,
           r.OwnerName, r.PriceRange, r.LogoImagePath, r.CoverImagePath, r.RepresentativeUserID,
           COUNT(rv.ReviewID) AS ReviewCount,
           AVG((rv.FoodRating + rv.ServiceRating + rv.AtmosphereRating) / 3.0) AS OverallRating,
           AVG(rv.FoodRating * 1.0) AS FoodRating,
           AVG(rv.ServiceRating * 1.0) AS ServiceRating,
           AVG(rv.AtmosphereRating * 1.0) AS AtmosphereRating,
           AVG(rv.PriceRating * 1.0) AS PriceRating
    FROM TP_Restaurants r
    INNER JOIN Cuisines c ON r.CuisineID = c.CuisineID
    LEFT JOIN Review rv ON r.RestaurantID = rv.RestaurantID
    WHERE r.RestaurantID = @RestaurantID
    GROUP BY r.RestaurantID, r.RestaurantName, r.CuisineID, c.CuisineName, r.Description,
             r.StreetAddress, r.City, r.State, r.ZipCode, r.Phone, r.Email, r.WebsiteUrl,
             r.OwnerName, r.PriceRange, r.LogoImagePath, r.CoverImagePath, r.RepresentativeUserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantHours]
CREATE PROCEDURE TP_GetRestaurantHours
    @RestaurantID int
AS
    SELECT DayOfWeek, OpenTime, CloseTime, IsClosed
    FROM TP_RestaurantHours
    WHERE RestaurantID = @RestaurantID
    ORDER BY DayOfWeek
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantSocialLinks]
-- =============================================
-- GetRestaurantSocialLinks
-- Retrieves the restaurant's social media links
-- =============================================
CREATE PROCEDURE TP_GetRestaurantSocialLinks
    @RestaurantID int
AS
    SELECT s.PlatformID, p.PlatformName, s.LinkUrl
    FROM TP_RestaurantSocialLink s
    INNER JOIN SocialMediaPlatform p ON s.PlatformID = p.PlatformID
    WHERE s.RestaurantID = @RestaurantID
    ORDER BY p.PlatformName
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantPhotos]
-- =============================================
-- GetRestaurantPhotos
-- Retrieves all the uploaded photos for a restaurant
-- =============================================
CREATE PROCEDURE TP_GetRestaurantPhotos
    @RestaurantID int
AS
    SELECT PhotoID, PhotoType, Caption, ImageData, ImageType
    FROM TP_RestaurantPhoto
    WHERE RestaurantID = @RestaurantID
    ORDER BY PhotoID
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRatingCounts]
-- =============================================
-- GetRatingCounts
-- Counts how many 1, 2, 3, 4 and 5 star food ratings a restaurant has.
-- Used to show the rating bars on the profile page.
-- =============================================
CREATE PROCEDURE TP_GetRatingCounts
    @RestaurantID int
AS
    SELECT FoodRating AS Stars, COUNT(*) AS RatingCount
    FROM Review
    WHERE RestaurantID = @RestaurantID
    GROUP BY FoodRating
    ORDER BY FoodRating DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReviewsByRestaurantID]
CREATE PROCEDURE GetReviewsByRestaurant
    @RestaurantID int
AS
    SELECT rv.ReviewID, rv.ReviewDate, rv.VisitDate, rv.ReviewTitle, rv.ReviewText,
           rv.FoodRating, rv.ServiceRating, rv.AtmosphereRating, rv.PriceRating,
           rv.HelpfulCount, rv.NotHelpfulCount, rv.ReviewPhoto, rv.ReviewPhotoType,
           rv.ResponseText, rv.ResponseDate,
           p.FirstName, p.LastName, p.HomeCity, p.HomeState
    FROM Review rv
    INNER JOIN Reviewer p ON rv.ReviewerID = p.ReviewerID
    WHERE rv.RestaurantID = @RestaurantID
    ORDER BY rv.ReviewDate DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReviewerID]
CREATE PROCEDURE GetReviewerID
    @UserID int
AS
    SELECT ReviewerID
    FROM Reviewer
    WHERE UserID = @UserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReviewsByReviewer]
CREATE PROCEDURE GetReviewsByReviewer
    @ReviewerID int
AS
    SELECT rv.ReviewID, rv.RestaurantID, r.RestaurantName, rv.ReviewDate, rv.VisitDate,
           rv.ReviewTitle, rv.FoodRating, rv.ServiceRating, rv.AtmosphereRating, rv.PriceRating
    FROM Review rv
    INNER JOIN TP_Restaurants r ON rv.RestaurantID = r.RestaurantID
    WHERE rv.ReviewerID = @ReviewerID
    ORDER BY rv.ReviewDate DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReviewByID]
CREATE PROCEDURE GetReviewByID
    @ReviewID int,
    @ReviewerID int
AS
    SELECT ReviewID, RestaurantID, VisitDate, ReviewTitle, ReviewText,
           FoodRating, ServiceRating, AtmosphereRating, PriceRating,
           ReviewPhoto, ReviewPhotoType
    FROM Review
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddReview]
CREATE PROCEDURE AddReview
    @RestaurantID int,
    @ReviewerID int,
    @VisitDate datetime,
    @ReviewTitle varchar(100),
    @ReviewText varchar(4000),
    @FoodRating int,
    @ServiceRating int,
    @AtmosphereRating int,
    @PriceRating int,
    @ReviewPhoto varbinary(MAX),
    @ReviewPhotoType varchar(50)
AS
    INSERT INTO Review (RestaurantID, ReviewerID, ReviewDate, VisitDate, ReviewTitle, ReviewText,
                        FoodRating, ServiceRating, AtmosphereRating, PriceRating,
                        HelpfulCount, NotHelpfulCount, ReviewPhoto, ReviewPhotoType)
    VALUES (@RestaurantID, @ReviewerID, GETDATE(), @VisitDate, @ReviewTitle, @ReviewText,
            @FoodRating, @ServiceRating, @AtmosphereRating, @PriceRating,
            0, 0, @ReviewPhoto, @ReviewPhotoType)
GO

-- SQL_STORED_PROCEDURE: [dbo].[UpdateReview]
CREATE PROCEDURE UpdateReview
    @ReviewID int,
    @ReviewerID int,
    @VisitDate datetime,
    @ReviewTitle varchar(100),
    @ReviewText varchar(4000),
    @FoodRating int,
    @ServiceRating int,
    @AtmosphereRating int,
    @PriceRating int
AS
    UPDATE Review
    SET VisitDate = @VisitDate,
        ReviewTitle = @ReviewTitle,
        ReviewText = @ReviewText,
        FoodRating = @FoodRating,
        ServiceRating = @ServiceRating,
        AtmosphereRating = @AtmosphereRating,
        PriceRating = @PriceRating
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[UpdateReviewPhoto]
-- =============================================
-- UpdateReviewPhoto
-- Changes the photo on a review. Pass NULL to remove the photo.
-- =============================================
CREATE PROCEDURE UpdateReviewPhoto
    @ReviewID int,
    @ReviewerID int,
    @ReviewPhoto varbinary(MAX),
    @ReviewPhotoType varchar(50)
AS
    UPDATE Review
    SET ReviewPhoto = @ReviewPhoto,
        ReviewPhotoType = @ReviewPhotoType
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[DeleteReview]
-- =============================================
-- DeleteReview
-- Deletes a review
-- =============================================
CREATE PROCEDURE DeleteReview
    @ReviewID int,
    @ReviewerID int
AS
    DELETE FROM Review
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddHelpfulVote]
CREATE PROCEDURE AddHelpfulVote
    @ReviewID int
AS
    UPDATE Review
    SET HelpfulCount = HelpfulCount + 1
    WHERE ReviewID = @ReviewID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddNotHelpfulVote]
CREATE PROCEDURE AddNotHelpfulVote
    @ReviewID int
AS
    UPDATE Review
    SET NotHelpfulCount = NotHelpfulCount + 1
    WHERE ReviewID = @ReviewID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddReviewResponse]
CREATE PROCEDURE AddReviewResponse
    @ReviewID int,
    @RestaurantID int,
    @ResponseText varchar(2000)
AS
    UPDATE Review
    SET ResponseText = @ResponseText,
        ResponseDate = GETDATE()
    WHERE ReviewID = @ReviewID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[CheckUsername]
CREATE PROCEDURE CheckUsername
    @Username varchar(50)
AS
    SELECT UserID
    FROM UserAccount
    WHERE Username = @Username
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddUserAccount]
CREATE PROCEDURE AddUserAccount
    @Username varchar(50),
    @Password varchar(200),
    @UserType varchar(20),
    @FirstName varchar(50),
    @LastName varchar(50),
    @Email varchar(100),
    @Phone varchar(20)
AS
    INSERT INTO UserAccount (Username, Password, UserType, FirstName, LastName, Email, Phone)
    VALUES (@Username, @Password, @UserType, @FirstName, @LastName, @Email, @Phone)

    RETURN SCOPE_IDENTITY()
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddReviewer]
CREATE PROCEDURE AddReviewer
    @UserID int,
    @FirstName varchar(50),
    @LastName varchar(50),
    @HomeCity varchar(60),
    @HomeState char(2)
AS
    INSERT INTO Reviewer (FirstName, LastName, HomeCity, HomeState, UserID)
    VALUES (@FirstName, @LastName, @HomeCity, @HomeState, @UserID)
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetUserLogin]
CREATE PROCEDURE GetUserLogin
    @Username varchar(50)
AS
    SELECT UserID, Username, Password, UserType, FirstName, LastName, Email, Phone
    FROM UserAccount
    WHERE Username = @Username
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetUserByID]
CREATE PROCEDURE GetUserByID
    @UserID int
AS
    SELECT UserID, Username, UserType, FirstName, LastName, Email, Phone
    FROM UserAccount
    WHERE UserID = @UserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetSocialMediaPlatforms]
CREATE PROCEDURE GetSocialMediaPlatforms
AS
    SELECT PlatformID, PlatformName
    FROM SocialMediaPlatform
    ORDER BY PlatformName
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetRestaurantByRep]
CREATE PROCEDURE TP_GetRestaurantByRep
    @RepUserID int
AS
    SELECT RestaurantID, RestaurantName, CuisineID, Description, StreetAddress, City, State,
           ZipCode, Phone, Email, WebsiteUrl, OwnerName, PriceRange
    FROM TP_Restaurants
    WHERE RepresentativeUserID = @RepUserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddRestaurant]
CREATE PROCEDURE TP_AddRestaurant
    @RepUserID int,
    @RestaurantName varchar(100),
    @CuisineID int,
    @Description varchar(2000),
    @StreetAddress varchar(150),
    @City varchar(60),
    @State char(2),
    @ZipCode varchar(10),
    @Phone varchar(20),
    @Email varchar(100),
    @WebsiteUrl varchar(200),
    @OwnerName varchar(100),
    @PriceRange int
AS
    INSERT INTO TP_Restaurants (RestaurantName, CuisineID, Description, StreetAddress, City, State, ZipCode,
                             Phone, Email, WebsiteUrl, OwnerName, PriceRange, DateAdded, RepresentativeUserID)
    VALUES (@RestaurantName, @CuisineID, @Description, @StreetAddress, @City, @State, @ZipCode,
            @Phone, @Email, @WebsiteUrl, @OwnerName, @PriceRange, GETDATE(), @RepUserID)

    RETURN SCOPE_IDENTITY()
GO

-- SQL_STORED_PROCEDURE: [dbo].[UpdateRestaurant]
CREATE PROCEDURE TP_UpdateRestaurant
    @RestaurantID int,
    @RepUserID int,
    @RestaurantName varchar(100),
    @CuisineID int,
    @Description varchar(2000),
    @StreetAddress varchar(150),
    @City varchar(60),
    @State char(2),
    @ZipCode varchar(10),
    @Phone varchar(20),
    @Email varchar(100),
    @WebsiteUrl varchar(200),
    @OwnerName varchar(100),
    @PriceRange int
AS
    UPDATE TP_Restaurants
    SET RestaurantName = @RestaurantName,
        CuisineID = @CuisineID,
        Description = @Description,
        StreetAddress = @StreetAddress,
        City = @City,
        State = @State,
        ZipCode = @ZipCode,
        Phone = @Phone,
        Email = @Email,
        WebsiteUrl = @WebsiteUrl,
        OwnerName = @OwnerName,
        PriceRange = @PriceRange
    WHERE RestaurantID = @RestaurantID AND RepresentativeUserID = @RepUserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[DeleteRestaurantHours]
CREATE PROCEDURE TP_DeleteRestaurantHours
    @RestaurantID int
AS
    DELETE FROM RestaurantHours
    WHERE RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddRestaurantHours]
CREATE PROCEDURE TP_AddRestaurantHours
    @RestaurantID int,
    @DayOfWeek int,
    @OpenTime time,
    @CloseTime time,
    @IsClosed bit
AS
    INSERT INTO RestaurantHours (RestaurantID, DayOfWeek, OpenTime, CloseTime, IsClosed)
    VALUES (@RestaurantID, @DayOfWeek, @OpenTime, @CloseTime, @IsClosed)
GO

-- SQL_STORED_PROCEDURE: [dbo].[DeleteSocialLinks]
CREATE PROCEDURE TP_DeleteSocialLinks
    @RestaurantID int
AS
    DELETE FROM TP_RestaurantSocialLink
    WHERE RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddSocialLink]
CREATE PROCEDURE TP_AddSocialLink
    @RestaurantID int,
    @PlatformID int,
    @LinkUrl varchar(200)
AS
    INSERT INTO TP_RestaurantSocialLink (RestaurantID, PlatformID, LinkUrl)
    VALUES (@RestaurantID, @PlatformID, @LinkUrl)
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddRestaurantPhoto]
CREATE PROCEDURE TP_AddRestaurantPhoto
    @RestaurantID int,
    @PhotoType varchar(20),
    @Caption varchar(200),
    @ImageData varbinary(MAX),
    @ImageType varchar(50),
    @ImageLength int
AS
    INSERT INTO RestaurantPhoto (RestaurantID, PhotoType, Caption, ImageData, ImageType, ImageLength)
    VALUES (@RestaurantID, @PhotoType, @Caption, @ImageData, @ImageType, @ImageLength)
GO

-- SQL_STORED_PROCEDURE: [dbo].[DeleteRestaurantPhoto]
CREATE PROCEDURE TP_DeleteRestaurantPhoto
    @PhotoID int,
    @RestaurantID int
AS
    DELETE FROM RestaurantPhoto
    WHERE PhotoID = @PhotoID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[AddReservation]
CREATE PROCEDURE AddReservation
    @RestaurantID int,
    @UserID int,
    @ContactName varchar(100),
    @Phone varchar(20),
    @Email varchar(100),
    @ReservationDate datetime,
    @PartySize int,
    @SpecialRequests varchar(1000)
AS
    INSERT INTO Reservation (RestaurantID, UserID, ContactName, Phone, Email,
                             ReservationDate, PartySize, SpecialRequests, Status)
    VALUES (@RestaurantID, @UserID, @ContactName, @Phone, @Email,
            @ReservationDate, @PartySize, @SpecialRequests, 'Pending')
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReservations]
CREATE PROCEDURE GetReservations
    @RestaurantID int
AS
    SELECT ReservationID, ContactName, Phone, Email, ReservationDate,
           PartySize, SpecialRequests, Status
    FROM Reservation
    WHERE RestaurantID = @RestaurantID
    ORDER BY ReservationDate
GO

-- SQL_STORED_PROCEDURE: [dbo].[GetReservationByID]
CREATE PROCEDURE GetReservationByID
    @ReservationID int,
    @RestaurantID int
AS
    SELECT ReservationID, ContactName, Phone, Email, ReservationDate,
           PartySize, SpecialRequests, Status
    FROM Reservation
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[UpdateReservation]
CREATE PROCEDURE UpdateReservation
    @ReservationID int,
    @RestaurantID int,
    @ContactName varchar(100),
    @Phone varchar(20),
    @Email varchar(100),
    @ReservationDate datetime,
    @PartySize int,
    @SpecialRequests varchar(1000)
AS
    UPDATE Reservation
    SET ContactName = @ContactName,
        Phone = @Phone,
        Email = @Email,
        ReservationDate = @ReservationDate,
        PartySize = @PartySize,
        SpecialRequests = @SpecialRequests
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[ConfirmReservation]
CREATE PROCEDURE ConfirmReservation
    @ReservationID int,
    @RestaurantID int
AS
    UPDATE Reservation
    SET Status = 'Confirmed'
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[DeleteReservation]
CREATE PROCEDURE DeleteReservation
    @ReservationID int,
    @RestaurantID int
AS
    DELETE FROM Reservation
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

