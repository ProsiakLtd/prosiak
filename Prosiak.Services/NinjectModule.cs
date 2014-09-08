using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Web.Common;

namespace Prosiak.Services
{
    public class NinjectModule
        : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IVisitorsManagerService>().To<VisitorsManagerService>().InSingletonScope();
        }
    }
}