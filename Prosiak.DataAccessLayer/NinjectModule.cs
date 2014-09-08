using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prosiak.DataAccessLayer
{
    public class NinjectModule
        : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //Bind<IVisitorsManagerService>().To<VisitorsManagerService>().InSingletonScope();
        }
    }
}