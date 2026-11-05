-- What the Fork, Project 3: every table and stored procedure renamed with the TP_ prefix.
-- Creates new TP_ objects next to the existing ones; nothing existing is changed or dropped.
-- Backup of [fa26_3342_tus45674] on cis-mssql1.temple.edu, 2026-11-04 17:12
-- Run this after 01_Tables.sql.

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetCuisines]
-- =============================================
-- What the Fork - Project 2
-- Script 04: Stored procedures for the Search page

CREATE PROCEDURE TP_GetCuisines
AS
    SELECT CuisineID, CuisineName
    FROM TP_Cuisines
    ORDER BY CuisineName
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetLocations]
CREATE PROCEDURE TP_GetLocations
AS
    SELECT City, State, COUNT(*) AS RestaurantCount
    FROM TP_Restaurants
    GROUP BY City, State
    ORDER BY City, State
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_SearchRestaurants]
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
    INNER JOIN TP_Cuisines c ON r.CuisineID = c.CuisineID
    LEFT JOIN TP_Review rv ON r.RestaurantID = rv.RestaurantID
    WHERE ',' + @CuisineList + ',' LIKE '%,' + CAST(r.CuisineID AS varchar) + ',%'
      AND (@City = '' OR r.City = @City)
      AND (@State = '' OR r.State = @State)
    GROUP BY r.RestaurantID, r.RestaurantName, c.CuisineName, r.LogoImagePath, r.PriceRange, r.City, r.State
    ORDER BY OverallRating DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantLogo]
CREATE PROCEDURE TP_GetRestaurantLogo
    @RestaurantID int
AS
    SELECT ImageData, ImageType
    FROM TP_RestaurantPhoto
    WHERE RestaurantID = @RestaurantID AND PhotoType = 'Logo'
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantByID]
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
    INNER JOIN TP_Cuisines c ON r.CuisineID = c.CuisineID
    LEFT JOIN TP_Review rv ON r.RestaurantID = rv.RestaurantID
    WHERE r.RestaurantID = @RestaurantID
    GROUP BY r.RestaurantID, r.RestaurantName, r.CuisineID, c.CuisineName, r.Description,
             r.StreetAddress, r.City, r.State, r.ZipCode, r.Phone, r.Email, r.WebsiteUrl,
             r.OwnerName, r.PriceRange, r.LogoImagePath, r.CoverImagePath, r.RepresentativeUserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantHours]
CREATE PROCEDURE TP_GetRestaurantHours
    @RestaurantID int
AS
    SELECT DayOfWeek, OpenTime, CloseTime, IsClosed
    FROM TP_RestaurantHours
    WHERE RestaurantID = @RestaurantID
    ORDER BY DayOfWeek
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantSocialLinks]
-- =============================================
-- GetRestaurantSocialLinks
-- Retrieves the restaurant's social media links
-- =============================================
CREATE PROCEDURE TP_GetRestaurantSocialLinks
    @RestaurantID int
AS
    SELECT s.PlatformID, p.PlatformName, s.LinkUrl
    FROM TP_RestaurantSocialLink s
    INNER JOIN TP_SocialMediaPlatform p ON s.PlatformID = p.PlatformID
    WHERE s.RestaurantID = @RestaurantID
    ORDER BY p.PlatformName
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantPhotos]
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

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRatingCounts]
-- =============================================
-- GetRatingCounts
-- Counts how many 1, 2, 3, 4 and 5 star food ratings a restaurant has.
-- Used to show the rating bars on the profile page.
-- =============================================
CREATE PROCEDURE TP_GetRatingCounts
    @RestaurantID int
AS
    SELECT FoodRating AS Stars, COUNT(*) AS RatingCount
    FROM TP_Review
    WHERE RestaurantID = @RestaurantID
    GROUP BY FoodRating
    ORDER BY FoodRating DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReviewsByRestaurantID]
CREATE PROCEDURE TP_GetReviewsByRestaurantID
    @RestaurantID int
AS
    SELECT rv.ReviewID, rv.ReviewDate, rv.VisitDate, rv.ReviewTitle, rv.ReviewText,
           rv.FoodRating, rv.ServiceRating, rv.AtmosphereRating, rv.PriceRating,
           rv.HelpfulCount, rv.NotHelpfulCount, rv.ReviewPhoto, rv.ReviewPhotoType,
           rv.ResponseText, rv.ResponseDate,
           p.FirstName, p.LastName, p.HomeCity, p.HomeState
    FROM TP_Review rv
    INNER JOIN TP_Reviewer p ON rv.ReviewerID = p.ReviewerID
    WHERE rv.RestaurantID = @RestaurantID
    ORDER BY rv.ReviewDate DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReviewerID]
CREATE PROCEDURE TP_GetReviewerID
    @UserID int
AS
    SELECT ReviewerID
    FROM TP_Reviewer
    WHERE UserID = @UserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReviewsByReviewer]
CREATE PROCEDURE TP_GetReviewsByReviewer
    @ReviewerID int
AS
    SELECT rv.ReviewID, rv.RestaurantID, r.RestaurantName, rv.ReviewDate, rv.VisitDate,
           rv.ReviewTitle, rv.FoodRating, rv.ServiceRating, rv.AtmosphereRating, rv.PriceRating
    FROM TP_Review rv
    INNER JOIN TP_Restaurants r ON rv.RestaurantID = r.RestaurantID
    WHERE rv.ReviewerID = @ReviewerID
    ORDER BY rv.ReviewDate DESC
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReviewByID]
CREATE PROCEDURE TP_GetReviewByID
    @ReviewID int,
    @ReviewerID int
AS
    SELECT ReviewID, RestaurantID, VisitDate, ReviewTitle, ReviewText,
           FoodRating, ServiceRating, AtmosphereRating, PriceRating,
           ReviewPhoto, ReviewPhotoType
    FROM TP_Review
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddReview]
CREATE PROCEDURE TP_AddReview
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
    INSERT INTO TP_Review (RestaurantID, ReviewerID, ReviewDate, VisitDate, ReviewTitle, ReviewText,
                        FoodRating, ServiceRating, AtmosphereRating, PriceRating,
                        HelpfulCount, NotHelpfulCount, ReviewPhoto, ReviewPhotoType)
    VALUES (@RestaurantID, @ReviewerID, GETDATE(), @VisitDate, @ReviewTitle, @ReviewText,
            @FoodRating, @ServiceRating, @AtmosphereRating, @PriceRating,
            0, 0, @ReviewPhoto, @ReviewPhotoType)
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_UpdateReview]
CREATE PROCEDURE TP_UpdateReview
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
    UPDATE TP_Review
    SET VisitDate = @VisitDate,
        ReviewTitle = @ReviewTitle,
        ReviewText = @ReviewText,
        FoodRating = @FoodRating,
        ServiceRating = @ServiceRating,
        AtmosphereRating = @AtmosphereRating,
        PriceRating = @PriceRating
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_UpdateReviewPhoto]
-- =============================================
-- UpdateReviewPhoto
-- Changes the photo on a review. Pass NULL to remove the photo.
-- =============================================
CREATE PROCEDURE TP_UpdateReviewPhoto
    @ReviewID int,
    @ReviewerID int,
    @ReviewPhoto varbinary(MAX),
    @ReviewPhotoType varchar(50)
AS
    UPDATE TP_Review
    SET ReviewPhoto = @ReviewPhoto,
        ReviewPhotoType = @ReviewPhotoType
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_DeleteReview]
-- =============================================
-- DeleteReview
-- Deletes a review
-- =============================================
CREATE PROCEDURE TP_DeleteReview
    @ReviewID int,
    @ReviewerID int
AS
    DELETE FROM TP_Review
    WHERE ReviewID = @ReviewID AND ReviewerID = @ReviewerID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddHelpfulVote]
CREATE PROCEDURE TP_AddHelpfulVote
    @ReviewID int
AS
    UPDATE TP_Review
    SET HelpfulCount = HelpfulCount + 1
    WHERE ReviewID = @ReviewID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddNotHelpfulVote]
CREATE PROCEDURE TP_AddNotHelpfulVote
    @ReviewID int
AS
    UPDATE TP_Review
    SET NotHelpfulCount = NotHelpfulCount + 1
    WHERE ReviewID = @ReviewID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddReviewResponse]
CREATE PROCEDURE TP_AddReviewResponse
    @ReviewID int,
    @RestaurantID int,
    @ResponseText varchar(2000)
AS
    UPDATE TP_Review
    SET ResponseText = @ResponseText,
        ResponseDate = GETDATE()
    WHERE ReviewID = @ReviewID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_CheckUsername]
CREATE PROCEDURE TP_CheckUsername
    @Username varchar(50)
AS
    SELECT UserID
    FROM TP_UserAccount
    WHERE Username = @Username
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddUserAccount]
CREATE PROCEDURE TP_AddUserAccount
    @Username varchar(50),
    @Password varchar(200),
    @UserType varchar(20),
    @FirstName varchar(50),
    @LastName varchar(50),
    @Email varchar(100),
    @Phone varchar(20)
AS
    INSERT INTO TP_UserAccount (Username, Password, UserType, FirstName, LastName, Email, Phone)
    VALUES (@Username, @Password, @UserType, @FirstName, @LastName, @Email, @Phone)

    RETURN SCOPE_IDENTITY()
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddReviewer]
CREATE PROCEDURE TP_AddReviewer
    @UserID int,
    @FirstName varchar(50),
    @LastName varchar(50),
    @HomeCity varchar(60),
    @HomeState char(2)
AS
    INSERT INTO TP_Reviewer (FirstName, LastName, HomeCity, HomeState, UserID)
    VALUES (@FirstName, @LastName, @HomeCity, @HomeState, @UserID)
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetUserLogin]
CREATE PROCEDURE TP_GetUserLogin
    @Username varchar(50)
AS
    SELECT UserID, Username, Password, UserType, FirstName, LastName, Email, Phone
    FROM TP_UserAccount
    WHERE Username = @Username
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetUserByID]
CREATE PROCEDURE TP_GetUserByID
    @UserID int
AS
    SELECT UserID, Username, UserType, FirstName, LastName, Email, Phone
    FROM TP_UserAccount
    WHERE UserID = @UserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetSocialMediaPlatforms]
CREATE PROCEDURE TP_GetSocialMediaPlatforms
AS
    SELECT PlatformID, PlatformName
    FROM TP_SocialMediaPlatform
    ORDER BY PlatformName
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetRestaurantByRep]
CREATE PROCEDURE TP_GetRestaurantByRep
    @RepUserID int
AS
    SELECT RestaurantID, RestaurantName, CuisineID, Description, StreetAddress, City, State,
           ZipCode, Phone, Email, WebsiteUrl, OwnerName, PriceRange
    FROM TP_Restaurants
    WHERE RepresentativeUserID = @RepUserID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddRestaurant]
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

-- SQL_STORED_PROCEDURE: [dbo].[TP_UpdateRestaurant]
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

-- SQL_STORED_PROCEDURE: [dbo].[TP_DeleteRestaurantHours]
CREATE PROCEDURE TP_DeleteRestaurantHours
    @RestaurantID int
AS
    DELETE FROM TP_RestaurantHours
    WHERE RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddRestaurantHours]
CREATE PROCEDURE TP_AddRestaurantHours
    @RestaurantID int,
    @DayOfWeek int,
    @OpenTime time,
    @CloseTime time,
    @IsClosed bit
AS
    INSERT INTO TP_RestaurantHours (RestaurantID, DayOfWeek, OpenTime, CloseTime, IsClosed)
    VALUES (@RestaurantID, @DayOfWeek, @OpenTime, @CloseTime, @IsClosed)
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_DeleteSocialLinks]
CREATE PROCEDURE TP_DeleteSocialLinks
    @RestaurantID int
AS
    DELETE FROM TP_RestaurantSocialLink
    WHERE RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddSocialLink]
CREATE PROCEDURE TP_AddSocialLink
    @RestaurantID int,
    @PlatformID int,
    @LinkUrl varchar(200)
AS
    INSERT INTO TP_RestaurantSocialLink (RestaurantID, PlatformID, LinkUrl)
    VALUES (@RestaurantID, @PlatformID, @LinkUrl)
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddRestaurantPhoto]
CREATE PROCEDURE TP_AddRestaurantPhoto
    @RestaurantID int,
    @PhotoType varchar(20),
    @Caption varchar(200),
    @ImageData varbinary(MAX),
    @ImageType varchar(50),
    @ImageLength int
AS
    INSERT INTO TP_RestaurantPhoto (RestaurantID, PhotoType, Caption, ImageData, ImageType, ImageLength)
    VALUES (@RestaurantID, @PhotoType, @Caption, @ImageData, @ImageType, @ImageLength)
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_DeleteRestaurantPhoto]
CREATE PROCEDURE TP_DeleteRestaurantPhoto
    @PhotoID int,
    @RestaurantID int
AS
    DELETE FROM TP_RestaurantPhoto
    WHERE PhotoID = @PhotoID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_AddReservation]
CREATE PROCEDURE TP_AddReservation
    @RestaurantID int,
    @UserID int,
    @ContactName varchar(100),
    @Phone varchar(20),
    @Email varchar(100),
    @ReservationDate datetime,
    @PartySize int,
    @SpecialRequests varchar(1000)
AS
    INSERT INTO TP_Reservation (RestaurantID, UserID, ContactName, Phone, Email,
                             ReservationDate, PartySize, SpecialRequests, Status)
    VALUES (@RestaurantID, @UserID, @ContactName, @Phone, @Email,
            @ReservationDate, @PartySize, @SpecialRequests, 'Pending')
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReservations]
CREATE PROCEDURE TP_GetReservations
    @RestaurantID int
AS
    SELECT ReservationID, ContactName, Phone, Email, ReservationDate,
           PartySize, SpecialRequests, Status
    FROM TP_Reservation
    WHERE RestaurantID = @RestaurantID
    ORDER BY ReservationDate
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_GetReservationByID]
CREATE PROCEDURE TP_GetReservationByID
    @ReservationID int,
    @RestaurantID int
AS
    SELECT ReservationID, ContactName, Phone, Email, ReservationDate,
           PartySize, SpecialRequests, Status
    FROM TP_Reservation
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_UpdateReservation]
CREATE PROCEDURE TP_UpdateReservation
    @ReservationID int,
    @RestaurantID int,
    @ContactName varchar(100),
    @Phone varchar(20),
    @Email varchar(100),
    @ReservationDate datetime,
    @PartySize int,
    @SpecialRequests varchar(1000)
AS
    UPDATE TP_Reservation
    SET ContactName = @ContactName,
        Phone = @Phone,
        Email = @Email,
        ReservationDate = @ReservationDate,
        PartySize = @PartySize,
        SpecialRequests = @SpecialRequests
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_ConfirmReservation]
CREATE PROCEDURE TP_ConfirmReservation
    @ReservationID int,
    @RestaurantID int
AS
    UPDATE TP_Reservation
    SET Status = 'Confirmed'
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

-- SQL_STORED_PROCEDURE: [dbo].[TP_DeleteReservation]
CREATE PROCEDURE TP_DeleteReservation
    @ReservationID int,
    @RestaurantID int
AS
    DELETE FROM TP_Reservation
    WHERE ReservationID = @ReservationID AND RestaurantID = @RestaurantID
GO

