using System;
using System.ComponentModel;
using What_The_Fork.Classes;
using What_The_Fork.Styling.Reviews;
using What_The_Fork.Database;

namespace What_The_Fork.Web_Forms
{

    public partial class ReviewDisplay : System.Web.UI.UserControl
    {
        private int reviewID;
        private String reviewerName;
        private String reviewerHome;
        private DateTime reviewDate;
        private String reviewTitle;
        private String reviewText;
        private int foodRating;
        private int serviceRating;
        private int atmosphereRating;
        private int priceRating;
        private int helpfulCount;
        private int notHelpfulCount;
        private DateTime visitDate;
        private byte[] reviewPhoto;
        private String reviewPhotoType;
        private String responseText;
        private DateTime responseDate;




        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [Category("Misc")]
        public int ReviewID
        {
            get { return reviewID; }
            set { reviewID = value; }
        }

        [Category("Misc")]
        public String ReviewerName
        {
            get { return reviewerName; }
            set { reviewerName = value; }
        }

        [Category("Misc")]
        public String ReviewerHome
        {
            get { return reviewerHome; }
            set { reviewerHome = value; }
        }

        [Category("Misc")]
        public DateTime ReviewDate
        {
            get { return reviewDate; }
            set { reviewDate = value; }
        }

        [Category("Misc")]
        public String ReviewTitle
        {
            get { return reviewTitle; }
            set { reviewTitle = value; }
        }

        [Category("Misc")]
        public String ReviewText
        {
            get { return reviewText; }
            set { reviewText = value; }
        }

        [Category("Misc")]
        public int FoodRating
        {
            get { return foodRating; }
            set { foodRating = value; }
        }

        [Category("Misc")]
        public int ServiceRating
        {
            get { return serviceRating; }
            set { serviceRating = value; }
        }

        [Category("Misc")]
        public int AtmosphereRating
        {
            get { return atmosphereRating; }
            set { atmosphereRating = value; }
        }

        [Category("Misc")]
        public int PriceRating
        {
            get { return priceRating; }
            set { priceRating = value; }
        }

        [Category("Misc")]
        public int HelpfulCount
        {
            get { return helpfulCount; }
            set { helpfulCount = value; }
        }

        [Category("Misc")]
        public int NotHelpfulCount
        {
            get { return notHelpfulCount; }
            set { notHelpfulCount = value; }
        }


        [Category("Misc")]
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        [Category("Misc")]
        public byte[] ReviewPhoto
        {
            get { return reviewPhoto; }
            set { reviewPhoto = value; }

        }

        [Category("Misc")]
        public String ReviewPhotoType
        {
            get { return reviewPhotoType; }
            set { reviewPhotoType = value; }
        }

        [Category("Misc")]
        public String ResponseText
        {
            get { return responseText; }
            set { responseText = value; }
        }

        [Category("Misc")]
        public DateTime ResponseDate
        {
            get { return responseDate; }
            set { responseDate = value; }
        }


        private String VoteSessionKey
        {
            get { return "ReviewVote" + reviewID; }
        }


        public override void DataBind()
        {
            //Make sure intials fit in the circle and are not too long
            lblAvatar.Text = GetIntials(reviewerName);
            lblReviewerName.Text = Server.HtmlEncode(reviewerName);
            if (!String.IsNullOrEmpty(reviewerHome))
            {
                lblReviewerHome.Text = "from " + Server.HtmlEncode(reviewerHome);
            }


            lblReviewDate.Text = reviewDate.ToString("MMMM d, yyyy 'at' h:mm tt");

            if (visitDate != DateTime.MinValue)
            {
                lblVisitDate.Text = "Visited on " + visitDate.ToString("MMMM d, yyyy");
            }

            lblTitle.Text = Server.HtmlEncode(reviewTitle);
            lblText.Text = Server.HtmlEncode(reviewText);

            if (reviewPhoto != null && reviewPhoto.Length > 0)
            {
                imgReviewPhoto.ImageUrl = "data:" + reviewPhotoType + ";base64," + Convert.ToBase64String(reviewPhoto);
                imgReviewPhoto.AlternateText = "Photo from " + reviewerName;
                imgReviewPhoto.Visible = true;
            }

            if (!String.IsNullOrEmpty(responseText))
            {
                lblResponseText.Text = Server.HtmlEncode(responseText);

                if (responseDate != DateTime.MinValue)
                {
                    lblResponseDate.Text = responseDate.ToString("MMMM d, yyyy");


                }
                divResponse.Visible = true;
            }

            lblFood.Text = foodRating.ToString();
            lblService.Text = serviceRating.ToString();
            lblPrice.Text = RestaurantDB.GetDollarSigns(priceRating);
            double overall;

            if (atmosphereRating == 0)
            {
                lblAtmosphere.Text = "-";
                overall = (foodRating + serviceRating) / 2.0;
            }

            else
            {
                lblAtmosphere.Text = atmosphereRating.ToString();
                overall = (foodRating + serviceRating + atmosphereRating) / 3.0;
            }

            //Changing of stars is now handled nby reviewDiv
            litStars.Text = ReviewDivs.GetStarsHtml(overall);


            ShowVoteCounts();


            if (Session[VoteSessionKey] != null)
            {
                DisableVoting("You rated this review. Thanks!");

            }
        }

        protected void btnHelpful_Click(object sender, EventArgs e)
        {
            RecordVote(true);
        }

        protected void btnNotHelpful_Click(object sender, EventArgs e)
        {
            RecordVote(false);
        }

        private void RecordVote(bool isHelpful)
        {
            if (Session[VoteSessionKey] != null)
            {
                DisableVoting("You already rated this review.");
                return;
            }

            ReviewDB objReviewDB = new ReviewDB();
            int rowsUpdated = objReviewDB.AddReviewVote(reviewID, isHelpful);

            if (rowsUpdated == 1)
            {
                if (isHelpful)
                {
                    helpfulCount = helpfulCount + 1;

                }
                else
                {
                    notHelpfulCount = notHelpfulCount + 1;
                }


                Session[VoteSessionKey] = true;
                ShowVoteCounts();
                DisableVoting("Thanks for your feedback!");
            }
            else
            {
                lblVoteMessage.Text = "Sorry, your vote couldn't be saved. Please try again later.";
            }
        }

        private void ShowVoteCounts()
        {
            btnHelpful.Text = "Helpful (" + helpfulCount + ")";
            btnNotHelpful.Text = "Not helpful (" + notHelpfulCount + ")";
        }

        private void DisableVoting(String theMessage)
        {
            btnHelpful.Enabled = false;
            btnNotHelpful.Enabled = false;
            lblVoteMessage.Text = theMessage;
        }


        private String GetIntials(String name)
        {
            String initials = "";

            foreach (String part in name.Split(' '))
            {
                if (part.Length > 0)
                    initials += part.Substring(0, 1).ToUpper();
            }
            return Server.HtmlEncode(initials.ToUpper());
        }

    }
}
