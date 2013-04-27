using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeEntry.DL
{
    public static class TaskExtensions
    {
        private static TimeEntryDataContext db = new TimeEntryDataContext(DBSettings.ConnectionString);

        public static IQueryable<Task> GetAll()
        {
            return db.Tasks;
        }

        public static Task GetTask(this Task c, long id)
        {
            return db.Tasks.SingleOrDefault(t => t.ID == id);
        }

        public static long InsertTask(this Task c)
        {
            db.Tasks.InsertOnSubmit(c);
            db.SubmitChanges();

            return c.ID;
        }

        public static void UpdateTask(this Task c)
        {
            db.SubmitChanges();
        }

        public static void DeleteTask(this Task c)
        {
            db.Tasks.DeleteOnSubmit(c);
            db.SubmitChanges();
        }
    }
}
