using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using What_The_Fork.Classes;
using What_The_Fork.Navigation;
using What_The_Fork.Database;

namespace What_The_Fork.Web_Forms{
    public partial class Search : System.Web.UI.Page
    {
        RestaurantDB objRestruantsDB = new RestaurantDB();

            protected void Page_Load(object sender, EventArgs e)
            {
            litHeader.Text = UiHeader.GetHeader((UserAccounts)Session["User"]);
            litFooter.Text = UiFooter.GetFooter();

              
                if (!IsPostBack)
                {
                    try
                    {
                        LoadCuisines();
                        LoadLocations();

                        if (Session["SearchCriteria"] != null)
                        {
                            SearchCriteria objCriteria = (SearchCriteria)Session["SearchCriteria"];
                            RestoreSearch(objCriteria);
                            ShowResults(objCriteria);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError("An error occurred while loading the search page. Please try again later.");
                    }
                }
                else if (Session["SearchResults"] != null)
                {
                    gvResults.DataSource = (DataSet)Session["SearchResults"];
                    gvResults.DataBind();
                    gvResults.Visible = true;
                }
            }

            protected void btnSearch_Click(object sender, EventArgs e)
            {
                if (cblCuisines.SelectedIndex == -1)
                {
                    lblValidation.Text = "Please select at least one cuisine.";
                    lblValidation.Visible = true;
                    return;
                }
                lblValidation.Visible = false;

                SearchCriteria objCriteria = GetSearchCriteria();
                Session["SearchCriteria"] = objCriteria;

                gvResults.PageIndex = 0;
                ShowResults(objCriteria);
            }

            protected void btnSelectAll_Click(object sender, EventArgs e)
            {
                foreach (ListItem item in cblCuisines.Items)
                {
                    item.Selected = true;
                }
            }

            protected void btnClearAll_Click(object sender, EventArgs e)
            {
                cblCuisines.ClearSelection();
            }

            protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                if (Session["SearchResults"] != null)
                {
                    gvResults.PageIndex = e.NewPageIndex;
                    gvResults.DataSource = (DataSet)Session["SearchResults"];
                    gvResults.DataBind();
                }
            }

            protected void gvResults_RowCommand(object sender, GridViewCommandEventArgs e)
            {
                if (e.CommandName == "ViewDetails")
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    String restaurantID = gvResults.DataKeys[rowIndex].Value.ToString();
                    Response.Redirect("RestaurantProfile.aspx?id=" + restaurantID);
                }
            }

            private void LoadCuisines()
            {
                cblCuisines.DataSource = objRestruantsDB.GetCuisines();
                cblCuisines.DataTextField = "CuisineName";
                cblCuisines.DataValueField = "CuisineID";
                cblCuisines.DataBind();
            }

            private void LoadLocations()
            {
                DataSet dsLocations = objRestruantsDB.GetLocations();
                ddlLocation.Items.Clear();
                ddlLocation.Items.Add(new ListItem("All Locations", ""));

                foreach (DataRow row in dsLocations.Tables[0].Rows)
                {
                    String city = row["City"].ToString();
                    String state = row["State"].ToString();
                    String text = city + ", " + state + " (" + row["RestaurantCount"] + ")";
                    ddlLocation.Items.Add(new ListItem(text, city + "|" + state));
                }
            }

            private SearchCriteria GetSearchCriteria()
            {
                SearchCriteria objCriteria = new SearchCriteria();

                String cuisineList = "";
                foreach (ListItem item in cblCuisines.Items)
                {
                    int cuisineID;
                    if (item.Selected && int.TryParse(item.Value, out cuisineID))
                    {
                        if (cuisineList != "")
                            cuisineList += ",";
                        cuisineList += cuisineID;
                    }
                }
                objCriteria.CuisineList = cuisineList;

                String[] location = ddlLocation.SelectedValue.Split('|');
                if (location.Length == 2)
                {
                    objCriteria.City = location[0];
                    objCriteria.State = location[1];
                }

                return objCriteria;
            }

            private void RestoreSearch(SearchCriteria criteria)
            {
                foreach (ListItem item in cblCuisines.Items)
                {
                    item.Selected = IsSelectedCuisine(criteria, item.Value);
                }

                String location = criteria.City + "|" + criteria.State;
                if (!criteria.IsAllLocations && ddlLocation.Items.FindByValue(location) != null)
                {
                    ddlLocation.SelectedValue = location;
                }
            }

            private bool IsSelectedCuisine(SearchCriteria criteria, String cuisineID)
            {
                foreach (String id in criteria.CuisineList.Split(','))
                {
                    if (id == cuisineID)
                        return true;
                }
                return false;
            }

            private String GetCriteriaDescription(SearchCriteria criteria)
            {
                String cuisineNames = "";
                foreach (ListItem item in cblCuisines.Items)
                {
                    if (IsSelectedCuisine(criteria, item.Value))
                    {
                        if (cuisineNames != "")
                            cuisineNames += ", ";
                        cuisineNames += item.Text;
                    }
                }

                String location = "All Locations";
                if (!criteria.IsAllLocations)
                    location = criteria.City + ", " + criteria.State;

                return cuisineNames + " | " + location;
            }

            private void ShowResults(SearchCriteria criteria)
            {
                DataSet dsResults;
                try
                {
                    dsResults = objRestruantsDB.SearchRestraurants(criteria);
                }
                catch (Exception ex)
                {
                    ShowError("An error occurred while searching for restaurants. Please try again later.");
                    return;
                }

                Session["SearchResults"] = dsResults;

                int count = dsResults.Tables[0].Rows.Count;
                if (count == 1)
                    lblResultCount.Text = "1 restaurant found";
                else
                    lblResultCount.Text = count + " restaurants found";

                lblSearchCriteria.Text = GetCriteriaDescription(criteria);

                gvResults.DataSource = dsResults;
                gvResults.DataBind();
                gvResults.Visible = true;
                lblError.Visible = false;
            }

            private void ShowError(String theMessage)
            {
                lblError.Text = theMessage;
                lblError.Visible = true;
                gvResults.Visible = false;

            Session.Remove("SearchResults");
        }
        }
    }
