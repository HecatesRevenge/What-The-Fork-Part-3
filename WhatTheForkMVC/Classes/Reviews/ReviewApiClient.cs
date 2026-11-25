using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
namespace WhatTheForkMVC.Classes.Reviews
{
    public class ReviewApiClient
    {
        String ApiUrl = "";
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();

        public ReviewApiClient(HttpRequest request)
        {
            ApiUrl = request.Scheme + "://" + request.Host.Value + "/api/reviews/";
            jsonOptions.PropertyNameCaseInsensitive = true;
        }

        //GET
        //All reviews for a specific restaurant by all users
        public List<Review>GetReviewsForRestaurant(int restaurantID)
        {
            return GetReviewList("Restaurant/" + restaurantID);

        }

        //GET
        //All reviews for a specific restaurant by all users
        public List<Review> GetReviewsForUser(int userID)
        {
            return GetReviewList("User/" + userID);
        }

        //GET
        //Single review for a restaurant by a specific user
        public Review GetReview (int reviewID, int reviewerID) {
            HttpClient client = new HttpClient();
            HttpResponseMessage response=client.GetAsync(ApiUrl  + reviewID + "/" + reviewerID).Result;

            if (response.StatusCode == HttpStatusCode.OK || response.IsSuccessStatusCode)
            {
                String content=response.Content.ReadAsStringAsync().Result;


                if (content=="")
                {
                    return null;
                }

                return JsonSerializer.Deserialize<Review>(content, jsonOptions);
            }

            return null;
        }


        //POST
        //Adds new review using API as JSON
        public Boolean AddReview(Review review)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PostAsync(ApiUrl, MakeJsonBody(review)).Result;
            return ReadBoolean(response);
        }

        //POST
        //Adding new review using API as form key/value pairs
        public Boolean AddReviewForm(Review review)
        {
            HttpClient client=new HttpClient();
            HttpResponseMessage response=client.PostAsync(ApiUrl+"Form", MakeFormBody(review)).Result;
            return ReadBoolean(response);
           
        }


        //PUT
        //Updates existing review using API as JSON
        public Boolean UpdateReview(Review review)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PutAsync(ApiUrl + review.ReviewID, MakeJsonBody(review)).Result;
            return ReadBoolean(response);
        }

        //PUT
        //updates existing review using API as form key/value pairs
        public Boolean UpdateReviewForm(Review review)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PutAsync(ApiUrl + "Form/" + review.ReviewID, MakeFormBody(review)).Result;
            return ReadBoolean(response);
        }
        //DELETE
        //Deletes existing review by reviewID and reviewerID

        public Boolean DeleteReview(int reviewID, int reviewerID)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync(ApiUrl + reviewID + "/" + reviewerID).Result;
            return ReadBoolean(response);
        }



        private List<Review> GetReviewList(String route)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiUrl + route).Result;

            if (response.StatusCode == HttpStatusCode.OK || response.IsSuccessStatusCode)
            {
                String content=response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Review>>(content, jsonOptions);
            }
            return new List<Review>();
        }

        private FormUrlEncodedContent MakeFormBody(Review review)
        {
            Dictionary<String, String> fields = new Dictionary<String, String>();
            fields.Add("ReviewID", review.ReviewID.ToString());
            fields.Add("RestaurantID", review.RestaurantID.ToString());
            fields.Add("ReviewerID", review.ReviewerID.ToString());
            fields.Add("VisitDate", review.VisitDate.ToString("yyyy-MM-ddTHH:mm"));
            fields.Add("ReviewTitle", review.ReviewTitle);
            fields.Add("ReviewText", review.ReviewText);
            fields.Add("FoodRating", review.FoodRating.ToString());
            fields.Add("ServiceRating", review.ServiceRating.ToString());
            fields.Add("AtmosphereRating", review.AtmosphereRating.ToString());
            fields.Add("PriceRating", review.PriceRating.ToString());
            return new FormUrlEncodedContent(fields);
        }

        private StringContent MakeJsonBody(Review review)
        {

            String jsonReview = JsonSerializer.Serialize(review);
            StringContent requestBody = new StringContent(jsonReview);
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestBody;
        }
        private Boolean ReadBoolean(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.IsSuccessStatusCode)
            {
                String content = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<Boolean>(content, jsonOptions);
            }
            return false;
        }



    }
}
