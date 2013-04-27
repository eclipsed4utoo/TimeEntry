using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeEntry.DL.Properties;
using System.Configuration;

namespace TimeEntry.DL
{
    public class DBSettings
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TimeEntryConnectionString"].ConnectionString; }
        }
    }
}
