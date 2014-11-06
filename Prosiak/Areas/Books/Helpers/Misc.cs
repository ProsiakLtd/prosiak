using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Prosiak.Areas.Books.Helpers
{
    public class GoogleBookAPIResponse
    {
        public GoogleBookAPIItem[] Items { get; set; }
    }

    public class GoogleBookAPIItem
    {
        public GoogleBookApiVolumeInfo VolumeInfo { get; set; }
    }

    public class GoogleBookApiVolumeInfo
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string[] Categories { get; set; }
        public GoogleBookApiImageLinks ImageLinks { get; set; }

    }

    public class GoogleBookApiImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }

    public static class GoogleAPIResponse
    {
        //public static GoogleBookAPIResponse parsedResponse;
        public static GoogleBookAPIResponse GoogleJsonDecode(string response)
        {
            

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<GoogleBookAPIResponse>(response);
        }
        

        
    }

    public static class Thumbnails
    {
        public static string GetThumbnailDiskPath(string ISBN)
        {
            var thumbnailFilename = ISBN + ".jpg";
            var thumbnailFolder = HostingEnvironment.MapPath(VirtualPathUtility.ToAbsolute("~/Content/Thumbnails"));
            return Path.Combine(thumbnailFolder, thumbnailFilename);
        }

        public static string GetDefaultThumbnailDiskPath()
        {
            var thumbnailFolder = HostingEnvironment.MapPath(VirtualPathUtility.ToAbsolute("~/Content/Thumbnails"));
            return Path.Combine(thumbnailFolder, "empty.jpg");
        }

        public static string TryGetThumbnailFromGoogle(string ISBN)
        {
            string imageString = null;
            //Pass request to google api
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/books/v1/volumes?country=PL&q=isbn:" + ISBN);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                GoogleBookAPIResponse parsedResponse;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    parsedResponse = GoogleAPIResponse.GoogleJsonDecode(result);
                }


                if (parsedResponse.Items != null &&
                    parsedResponse.Items.First().VolumeInfo != null &&
                    parsedResponse.Items.First().VolumeInfo.ImageLinks != null &&
                    parsedResponse.Items.First().VolumeInfo.ImageLinks.SmallThumbnail != null)
                    imageString = parsedResponse.Items.First().VolumeInfo.ImageLinks.SmallThumbnail;
            }
            catch (Exception e) { }
            return imageString;
        }
    }

    public static class Linq
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static T First<T>(this T[] input)
        {
            if (input.Length > 0)
            {
                return input[0];
            }
            else
            {
                return default(T);
            }
        }
    }
}