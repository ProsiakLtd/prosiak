using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prosiak.Services
{
    public interface IVisitorsManagerService
    {
        int GetTotalNumberOfVisits();
        DateTime GetLastVisitedDate();
        string GetMostRecentUserName();
        void RegisterVisitingUser(string userName);
    }
}
