using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Web.Helpers;
using Syncfusion.EJ2.FileManager.Base;
using System.Data;
using System.Data.Common;
using WhatTheForkMVC.Classes;

namespace WhatTheForkMVC.Controllers
{
    public class ImageUploadController : Controller
    {
        SqlCommand objCommand = new SqlCommand();
        DBConnect objDB = new DBConnect();

        public IActionResult Submit(string title, string caption, int restid, IFormFile file, int photoid)
        {
            int result = 0;
            string strSQL = "";
            int imagesize = Convert.ToInt32(file.Length);
          // string fileExtension, imageType, imageName, imageTitle, strSQL;
            byte[] imageData = new byte[imagesize];

         //   file.InputStream.Read(imageData, 0, imagesize);

         string imageName = file.FileName;
            string imageType = file.ContentType;
          string  fileExtension = imageName.Substring(imageName.LastIndexOf("."));
            fileExtension = fileExtension.ToLower();
            if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".gif")

            {

                // INSERT an image (BLOB) into the database using a stored procedure 'storeProductImage'

                strSQL = "TP_AddRestaurantPhoto";

                objCommand.CommandText = strSQL;

                objCommand.CommandType = CommandType.StoredProcedure;

                objCommand.Parameters.AddWithValue("@ImageData", imageData);
                objCommand.Parameters.AddWithValue("@Caption", caption);
                objCommand.Parameters.AddWithValue("@ImageType", imageType);
                objCommand.Parameters.AddWithValue("@ImageLength", imageData.Length);
                objCommand.Parameters.AddWithValue("@RestaurantID", restid);

                result = objDB.DoUpdateUsingCmdObj(objCommand);


                ViewData["Status"] = "Image was successully uploaded.";

            }

            return View();
        }
    }
}
