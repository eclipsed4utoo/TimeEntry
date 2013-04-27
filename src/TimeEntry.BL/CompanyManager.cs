using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeEntry.DL;

namespace TimeEntry.BL
{
    public class CompanyManager
    {
        public static IQueryable<Company> GetAllCompanies()
        {
            return CompanyExtensions.GetAll();
        }

        public static Company GetCompany(long id)
        {
            Company c = new Company();
            c = c.GetCompany(id);
            return c;
        }

        public static long AddCompany(string name)
        {
            Company c = new Company();
            c.CompanyName = name;
            return c.InsertCompany();
        }

        public static void UpdateCompany(long id, string name)
        {
            Company c = GetCompany(id);
            c.CompanyName = name;
            c.UpdateCompany();
        }

        public static void DeleteCompany(long id)
        {
            Company c = GetCompany(id);
            c.DeleteCompany();
        }

        public static IQueryable<Task> GetTasks(long id)
        {
            Company c = GetCompany(id);
            return c.GetAllTasksForCompany();
        }
    }
}
