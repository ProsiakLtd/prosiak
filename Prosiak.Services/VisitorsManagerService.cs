using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prosiak.Services
{
    class VisitorsManagerService
        : IVisitorsManagerService
    {
        static int _totalNumberOfVisitors;
        static DateTime _lastVisitDate;
        static string _mostRecentUserName;

        public int GetTotalNumberOfVisits()
        {
            lock (this)
            {
                return _totalNumberOfVisitors;
            }
        }

        public DateTime GetLastVisitedDate()
        {
            lock (this)
            {
                return _lastVisitDate;
            }
        }

        public string GetMostRecentUserName()
        {
            lock (this)
            {
                return _mostRecentUserName;
            }
        }

        public void RegisterVisitingUser(string userName)
        {
            lock (this)
            {
                _mostRecentUserName = userName;
                _totalNumberOfVisitors++;
                _lastVisitDate = DateTime.Now;
            }
        }
    }
}