using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using TimeEntry.DL.Properties;

namespace TimeEntry.DL
{
    public static class DBUtilities
    {
        public static void CreateDatabase()
        {
            using (TimeEntryDataContext db = new TimeEntryDataContext())
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                }
            }
        }
    }
}
