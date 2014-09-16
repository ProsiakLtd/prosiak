using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Prosiak.Areas.Books.Helpers
{
    public class GoogleBookAPIResponse
    {
        public string Kind { get; set; }
        public string TotalItems { get; set; }
        public GoogleBookAPIItem[] Items { get; set; }
    }

    public class GoogleBookAPIItem
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public string SelfLink { get; set; }
        public GoogleBookApiVolumeInfo VolumeInfo { get; set; }
        public GoogleBookApiSaleInfo SaleInfo { get; set; }
        public GoogleBookApiAccessInfo AccessInfo { get; set; }
        public GoogleBookApiSearchInfo SearchInfo { get; set; }
    }

    public class GoogleBookApiVolumeInfo
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string[] Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string description { get; set; }
        public GoogleBookApiIndustryId[] IndustryIdentifiers { get; set; }
        public GoogleBookApiReadingModes ReadingModes { get; set; }
        public string PageCount { get; set; }
        public string PrintType { get; set; }
        public string[] Categories { get; set; }
        public string AverageRating { get; set; }
        public string RatingCount { get; set; }
        public string ContentVersion { get; set; }
        public GoogleBookApiImageLinks ImageLinks { get; set; }
        public string Language { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string CannonicalVolumeLink { get; set; }

    }

    public class GoogleBookApiIndustryId
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class GoogleBookApiReadingModes
    {
        public string Text { get; set; }
        public string Image { get; set; }
    }

    public class GoogleBookApiImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }

    public class GoogleBookApiSaleInfo
    {
        public string Country { get; set; }
        public string Saleability { get; set; }
        public bool IsEbook { get; set; }
    }

    public class GoogleBookApiAccessInfo
    {
        public string Country { get; set; }
        public string Vievability { get; set; }
        public bool Embeddable { get; set; }
        public bool PublicDomain { get; set; }
        public string textToSpeechPermission { get; set; }
        public GoogleBookApiEpub Epub { get; set; }
        public GoogleBookApiPdf Pdf { get; set; }
        public string webReaderLink { get; set; }
        public string accessViewStatus { get; set; }
        public bool quoteSharingAllowed { get; set; }


    }

    public class GoogleBookApiEpub
    {
        public bool isAvailable { get; set; }
    }

    public class GoogleBookApiPdf
    {
        public bool isAvailable { get; set; }
    }
    public class GoogleBookApiSearchInfo
    {
        public string textSnippet { get; set; }
    }

    public static class GoogleAPIResponse
    {
        public static GoogleBookAPIResponse parsedResponse;
        public static void GoogleJsonDecode(string response)
        {
            

            JavaScriptSerializer jss = new JavaScriptSerializer();
            parsedResponse = jss.Deserialize<GoogleBookAPIResponse>(response);
        }

        public static string GetThumbnailFromISBN(string ISBN)
        {
            string imageString = "http://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif";
            //Pass request to google api
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/books/v1/volumes?country=PL&q=isbn:" + ISBN);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    GoogleJsonDecode(result);
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


    [TestClass]
    public class TestClass
    {
        #region string
            string r = @"{
                 ""kind"": ""books#volumes"",
                 ""totalItems"": 1,
                 ""items"": [
                  {
                   ""kind"": ""books#volume"",
                   ""id"": ""0g1PdX4pdFkC"",
                   ""etag"": ""UCSDzpoLVPI"",
                   ""selfLink"": ""https://www.googleapis.com/books/v1/volumes/0g1PdX4pdFkC"",
                   ""volumeInfo"": {
                    ""title"": ""Under the Dome"",
                    ""subtitle"": ""A Novel"",
                    ""authors"": [
                     ""Stephen King""
                    ],
                    ""publisher"": ""Simon and Schuster"",
                    ""publishedDate"": ""2009-11-10"",
                    ""description"": ""After an invisible force field seals off Chester Mills, Maine, from the rest of the world, it is up to Dale Barbara, an Iraq veteran, and a select group of citizens to save the town, if they can get past Big Jim Rennie, a murderous politician, and his son, who hides a horrible secret in his dark pantry. By the best-selling author of Just After Sunset. Two million first printing."",
                    ""industryIdentifiers"": [
                     {
                      ""type"": ""ISBN_13"",
                      ""identifier"": ""9781439148501""
                     },
                     {
                      ""type"": ""ISBN_10"",
                      ""identifier"": ""1439148503""
                     }
                    ],
                    ""readingModes"": {
                     ""text"": false,
                     ""image"": false
                    },
                    ""pageCount"": 1074,
                    ""printType"": ""BOOK"",
                    ""categories"": [
                     ""Fiction""
                    ],
                    ""averageRating"": 4.0,
                    ""ratingsCount"": 167,
                    ""contentVersion"": ""1.0.0.0.preview.0"",
                    ""imageLinks"": {
                     ""smallThumbnail"": ""http://bks9.books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api"",
                     ""thumbnail"": ""http://bks9.books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api""
                    },
                    ""language"": ""en"",
                    ""previewLink"": ""http://books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&dq=isbn:9781439148501&hl=&cd=1&source=gbs_api"",
                    ""infoLink"": ""http://books.google.pl/books?id=0g1PdX4pdFkC&dq=isbn:9781439148501&hl=&source=gbs_api"",
                    ""canonicalVolumeLink"": ""http://books.google.pl/books/about/Under_the_Dome.html?hl=&id=0g1PdX4pdFkC""
                   },
                   ""saleInfo"": {
                    ""country"": ""PL"",
                    ""saleability"": ""NOT_FOR_SALE"",
                    ""isEbook"": false
                   },
                   ""accessInfo"": {
                    ""country"": ""PL"",
                    ""viewability"": ""PARTIAL"",
                    ""embeddable"": true,
                    ""publicDomain"": false,
                    ""textToSpeechPermission"": ""ALLOWED_FOR_ACCESSIBILITY"",
                    ""epub"": {
                     ""isAvailable"": false
                    },
                    ""pdf"": {
                     ""isAvailable"": false
                    },
                    ""webReaderLink"": ""http://books.google.pl/books/reader?id=0g1PdX4pdFkC&hl=&printsec=frontcover&output=reader&source=gbs_api"",
                    ""accessViewStatus"": ""SAMPLE"",
                    ""quoteSharingAllowed"": false
                   },
                   ""searchInfo"": {
                    ""textSnippet"": ""After an invisible force field seals off Chester Mills, Maine, from the rest of the world, it is up to Dale Barbara, an Iraq veteran, and a select group of citizens to save the town, if they can get past Big Jim Rennie, a murderous ...""
                   }
                  }
                 ]
                }
                ";
            #endregion
        [TestMethod]
        public void DoesItParseJSONCorrectly()
        {
            
            GoogleAPIResponse.GoogleJsonDecode(r);
            Assert.AreEqual("http://bks9.books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api", GoogleAPIResponse.parsedResponse.Items.First().VolumeInfo.ImageLinks.SmallThumbnail);
        }

        [TestMethod]
        public void DoesItProvideCorrectImageURL()
        {
            string s =  GoogleAPIResponse.GetThumbnailFromISBN("9781439148501");
            Assert.AreEqual("http://bks9.books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api", s);
        }

        [TestMethod]
        public void DoesGoogleReturnExpectedResult()
        {
        //Pass request to google api
        HttpWebRequest request =
            (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/books/v1/volumes?q=isbn:9781439148501");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();

            GoogleAPIResponse.GoogleJsonDecode(result);
            Assert.AreEqual("http://bks9.books.google.pl/books?id=0g1PdX4pdFkC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api", GoogleAPIResponse.parsedResponse.Items.First().VolumeInfo.ImageLinks.SmallThumbnail);
        }
        }
    }
}

/*

   


*/