using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosiak.DataAccessLayer
{
    class DatabaseOperations
        : IDatabaseOperations
    {
        readonly DatabaseContext _context;

        /// <summary>
        /// Constructor which takes http context. the context 
        /// will be disposed once http request is completed. 
        /// See ninject registration dor the context
        /// </summary>
        /// <param name="context">The database context which will be disposed once Http request is completed</param>
        public DatabaseOperations( DatabaseContext context)
        {
            _context = context;
        }

        //add implementation.
    }
}
