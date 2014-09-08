using Prosiak.Services;
using Prosiak.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Controllers
{
    public class HomeController : Controller
    {
        readonly IVisitorsManagerService _visitorsManagerService;

        public HomeController(IVisitorsManagerService visitorsManagerService)
        {
            _visitorsManagerService = visitorsManagerService;
        }

        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                NumberOfVisitors = _visitorsManagerService.GetTotalNumberOfVisits(),
                LastVisitor = _visitorsManagerService.GetMostRecentUserName(),
                LastVisitDate = _visitorsManagerService.GetLastVisitedDate()
            };

            _visitorsManagerService.RegisterVisitingUser(User.Identity.Name);

            return View(model);
        }
    }
}