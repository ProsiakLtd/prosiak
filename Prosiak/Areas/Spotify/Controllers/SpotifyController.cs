using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Spotify.Controllers
{
    public class SpotifyController : Controller
    {
        // GET: Spotify/Spotify
        public ActionResult Index()
        {
            return View();
        }
    }
}