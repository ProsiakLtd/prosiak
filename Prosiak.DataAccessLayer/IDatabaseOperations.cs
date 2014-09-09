using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosiak.DataAccessLayer
{
    /// <summary>
    /// this is the only one publicly available interfacve for database operations. 
    /// The class implementing it will use database context (not available publicly)
    /// to actually access database
    /// </summary>
    public interface IDatabaseOperations
    {
        //put here all operations we want to perform on the database
    }
}
