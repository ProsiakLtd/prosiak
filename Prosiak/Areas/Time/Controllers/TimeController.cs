using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Time.Controllers
{
    public class TimeController : Controller
    {
        // GET: Time/Time
        public ActionResult Index()
        {
            return View();
        }
    }
}