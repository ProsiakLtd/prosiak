using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Books.Helpers
{
    public static class HtmlHelpers
    {
        public static string GetThumbnailFromISBN(this HtmlHelper helper, string ISBN){
            return GoogleAPIResponse.GetThumbnailFromISBN(ISBN);
        }
    }
}