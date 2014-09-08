using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Shopping.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping/Shopping
        public ActionResult Index()
        {
            return View();
        }
    }
}