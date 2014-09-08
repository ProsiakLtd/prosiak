using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prosiak.ViewModels
{
    public class HomeViewModel
    {
        public int NumberOfVisitors { get; set; }
        public string LastVisitor { get; set; }
        public DateTime LastVisitDate { get; set; }
    }
}