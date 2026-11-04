using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace What_The_Fork.Navigation
{
    public class UiFooter
    {
        public static String GetFooter()
        {
            String html =
                "<footer class=\"site-footer\">" +
                "  <div class=\"container d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-2\">" +
                "    <div>" +
                "      <span class=\"brand-name\">What the <em>Fork</em></span>" +
                "      <div>Honest reviews for hungry people.</div>" +
                "    </div>" +
                "    <div class=\"text-md-end\">" +
                "      <div>CIS 3342 | Temple University</div>" +
                "      <a href=\"../index.html\">Course homepage</a>" +
                "    </div>" +
                "  </div>" +
                "</footer>";
            return html;
        }
    }
}