<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReviewDisplay.ascx.cs" Inherits="What_The_Fork.Web_Forms.ReviewDisplay" %>

<div class="review-card">
    <div class="review-header">
        <asp:Label ID="lblAvatar" runat="server" CssClass="review-avatar" />
        <div>
            <asp:Label ID="lblReviewerName" runat="server" CssClass="review-name" />
            <asp:Label ID="lblReviewerHome" runat="server" CssClass="review-home" />
            <div class="review-date">
                <asp:Label ID="lblReviewDate" runat="server" />
                <asp:Label ID="lblVisitDate" runat="server" CssClass="review-visit" />

            </div>
        </div>
        <div class="review-stars">
            <asp:Literal ID="litStars" runat="server" />
        </div>
    </div>

    <h3 class="review-title">
        <asp:Label ID="lblTitle" runat="server" />
    </h3>
    <p class="review-text">
        <asp:Label ID="lblText" runat="server" />
    </p>

    <asp:Image ID="imgReviewPhoto" runat="server" CssClass="review-card-photo" Visible="false" EnableViewState="false" />

    <div class="review-scores">
        Food
        <asp:Label ID="lblFood" runat="server" CssClass="review-score" />
        | Service
        <asp:Label ID="lblService" runat="server" CssClass="review-score" />
        | Atmosphere
       
        <asp:Label ID="lblAtmosphere" runat="server" CssClass="review-score" />
        | Price
        <asp:Label ID="lblPrice" runat="server" CssClass="review-score" />
    </div>

    <div class="review-footer">
        <span>Was this review helpful?</span>
        <asp:Button ID="btnHelpful" runat="server" CssClass="btn-vote" OnClick="btnHelpful_Click" />
        <asp:Button ID="btnNotHelpful" runat="server" CssClass="btn-vote" OnClick="btnNotHelpful_Click" />
        <asp:Label ID="lblVoteMessage" runat="server" CssClass="vote-message" />
    </div>

      <div id="divResponse" runat="server" class="review-response" visible="false">
      <div class="review-response-heading">
          Response from the restaurant
          <asp:Label ID="lblResponseDate" runat="server" CssClass="review-response-date" />
      </div>
      <p class="review-response-text">
          <asp:Label ID="lblResponseText" runat="server" />
      </p>
</div>
</div>
