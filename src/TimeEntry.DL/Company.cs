using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeEntry.DL
{
    public static class CompanyExtensions
    {
        private static TimeEntryDataContext db = new TimeEntryDataContext(DBSettings.ConnectionString);

        public static IQueryable<Company> GetAll()
        {
            return db.Companies;
        }

        public static Company GetCompany(this Company c, long id)
        {
            return db.Companies.SingleOrDefault(t => t.ID == id);
        }

        public static IQueryable<Task> GetAllTasksForCompany(this Company c)
        {
            return from t in db.Tasks
                   where t.CompanyID == c.ID
                   select t;
        }

        public static long InsertCompany(this Company c)
        {
            db.Companies.InsertOnSubmit(c);
            db.SubmitChanges();

            return c.ID;
        }

        public static void UpdateCompany(this Company c)
        {
            db.SubmitChanges();
        }

        public static void DeleteCompany(this Company c)
        {
            db.Companies.DeleteOnSubmit(c);
            db.SubmitChanges();
        }
    }
}
