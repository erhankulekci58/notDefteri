using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.DataAccessLayer.EntityFreamwork
{
    public class RepositoryBase
    {
        protected static DatabaseContext _db;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        public static void CreateContext()
        {
            if (_db == null)
            {
                lock (_lockSync)
                {
                    if (_db == null)
                    {
                        _db = new DatabaseContext();
                    }
                }
            }
            
        }
    }
}
