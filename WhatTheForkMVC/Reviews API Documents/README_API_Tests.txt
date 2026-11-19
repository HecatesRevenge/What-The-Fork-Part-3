What the Fork - Reviews API Tests (Requestly)
=============================================

These tests check every call in the Reviews Web API (ReviewsApiController).
They were made in Requestly and exported to a file you can import.


1. Download Requestly
---------------------
1. Go to https://requestly.com/downloads and download the desktop app
   for your computer (Windows or Mac).
2. Install it, open it, and sign in or create a free account.
   (The web version at app.requestly.io can't reach localhost unless
   the Requestly browser extension is installed, so use the desktop app.)


2. Import the tests
-------------------
1. In Requestly, open the API Client.
2. Click Import, choose the Requestly collection option, and pick the
   exported tests file that comes with this project.
3. The collection appears in the sidebar with one request per API call.


3. Run the app first
--------------------
1. Open the solution in Visual Studio and run WhatTheForkMVC (F5).
2. Leave it running while you test. The tests call:
      http://localhost:5262/api/Reviews
   If your app uses a different port, change it in each request's URL
   (the port is in Properties\launchSettings.json).


4. Before you test
------------------
Some calls need a ReviewerID and a ReviewID from the database:
- ReviewerID: in SSMS, run
      SELECT ReviewerID FROM TP_Reviewer WHERE UserID = 3
- ReviewID of a new test review: run the "Get reviews by user" call after
  adding one, and read the reviewID of the entry with your test title.
Put these numbers into the URL or body where the request needs them.


5. What each call does
----------------------
GET  /api/Reviews/Restaurant/{restaurantID}
     Returns every review for one restaurant as a JSON list, newest first,
     with the reviewer's name and home city.
     Expected: a list of reviews, or [] if the restaurant has none.

GET  /api/Reviews/User/{userID}
     Returns every review written by one logged-in user (by UserID).
     Expected: a list of that user's reviews, or [] if they have none.

GET  /api/Reviews/{reviewID}/{reviewerID}
     Returns one review with its full text. It only works when the review
     belongs to that reviewer.
     Expected: one review, or an empty response if it isn't theirs.

POST /api/Reviews
     Adds a new review sent as JSON (body type: raw JSON).
     Expected: true. A review that fails validation (blank title,
     a rating of 0, a future visit date) returns false.

POST /api/Reviews/Form
     Adds a new review sent as form data (body type:
     x-www-form-urlencoded).
     Expected: true, or false if it fails validation.

PUT  /api/Reviews/{reviewID}
     Changes an existing review, sent as JSON.
     Expected: true. Run "Get reviews by user" to see the change.

PUT  /api/Reviews/Form/{reviewID}
     Changes an existing review, sent as form data.
     Expected: true.

DELETE /api/Reviews/{reviewID}/{reviewerID}
     Deletes a review. It only deletes it when the reviewer owns it.
     Expected: true for the owner, false for any other reviewer ID.


6. Suggested order
------------------
1. Get reviews by restaurant   (check the API is running)
2. Add review (JSON)           -> true
3. Add review (form data)      -> true
4. Get reviews by user         -> find the two new ReviewIDs
5. Update review (JSON)        -> true
6. Update review (form data)   -> true
7. Get one review              -> shows the edited review
8. Add a bad review            -> false (validation works)
9. Delete with the wrong reviewer -> false
10. Delete both test reviews   -> true, true
