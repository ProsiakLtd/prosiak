using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prosiak.Areas.Books.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString BorrowButton(this HtmlHelper helper, int id, string reader)
        {
            var identity = helper.ViewContext.HttpContext.User.Identity;

            if (identity != null)
            {    
                var username = identity.Name;
                return GetButton(id, reader, username);
            }
            else
            {
                return null;
            }
        }

        public static string ThumbnailPath(this HtmlHelper helper, string isbn = "empty")
        {
            var path = Thumbnails.GetThumbnailDiskPath(isbn);
            if (System.IO.File.Exists(path))
            {
                return "~/Content/Thumbnails/" + isbn + ".jpg";
            }
            else
            {
                return "~/Content/Thumbnails/empty.jpg";
            }
        }

        public static FileContentResult Thumbnail(this HtmlHelper helper, string isbn)
        {
            var path = Thumbnails.GetThumbnailDiskPath(isbn);
            byte[] byteArray = System.IO.File.ReadAllBytes(path);
            return new FileContentResult(byteArray, "image/jpeg");
        }

        public static IHtmlString GetButton(int id, string reader, string username, bool conflict = false)
        {
            var input = new TagBuilder("input");
            input.Attributes["type"] = "submit";
            input.Attributes["class"] = "btn btn-default btn-sm";
            input.Attributes["data-prosiak-btn-id"] = id.ToString();
            if (string.IsNullOrEmpty(reader))
            {
                input.Attributes["value"] = "Borrow";
                return MvcHtmlString.Create(input.ToString(TagRenderMode.SelfClosing));
            }
            else if (reader == username)
            {
                input.Attributes["value"] = "Return";
                return MvcHtmlString.Create(input.ToString(TagRenderMode.SelfClosing));
            }
            else if (conflict)
            {
                return MvcHtmlString.Create(@"<p style=""color: red;"">Wypożyczone przez: <strong>" + reader + "</strong></p>");
            }
            else
            {
                return MvcHtmlString.Create(@"<p>Wypożyczone przez: <strong>" + reader + "</strong></p>");
            }
        }

        public static IEnumerable<SelectListItem> GetItems(this Type enumType, int? selectedValue)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException();
            }
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>();

            var items = names.Zip(values, (name,value) => 
            new SelectListItem{ 
                Text = name, 
                Value = value.ToString(), 
                Selected = value == selectedValue});
            return items;
        }

    }
}