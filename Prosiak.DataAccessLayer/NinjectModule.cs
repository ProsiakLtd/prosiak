using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Web.Common;

namespace Prosiak.DataAccessLayer
{
    public class NinjectModule
        : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<DatabaseContext>().ToSelf().InRequestScope(); //it's going to be disposed once  http request is completed
            Bind<IDatabaseOperations>().To<DatabaseOperations>();
        }
    }
}