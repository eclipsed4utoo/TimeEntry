using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeEntry.DL
{
    public static class TimeEntryExtensions
    {
        private static TimeEntryDataContext db = new TimeEntryDataContext(DBSettings.ConnectionString);

        public static void InsertTimeEntry(this TimeEntry t)
        {
            CheckDB();
            db.TimeEntries.InsertOnSubmit(t);
            db.SubmitChanges();
            db = null;
        }

        public static void UpdateTimeEntry(this TimeEntry t)
        {
            CheckDB();
            db.SubmitChanges();
            db = null;
        }

        public static TimeEntry GetLatestTimeEntry(this Task c)
        {
            CheckDB();

            var query = from t in db.TimeEntries
                        where t.TaskID == c.ID
                        orderby t.CreatedDate descending
                        select t;

            return (query.Count() > 0 ? query.First() : null);
        }

        public static IQueryable<TimeEntry> GetTimeEntriesForRange(long taskID, DateTime startDate, DateTime endDate)
        {
            CheckDB();

            return from t in db.TimeEntries
                   where t.CreatedDate >= startDate
                   && t.CreatedDate < endDate
                   && t.EndDate.HasValue
                   && t.TaskID == taskID
                   select t;
        }

        public static IQueryable<TimeEntry> GetAllTimeEntriesForTask(long taskID)
        {
            CheckDB();

            return from t in db.TimeEntries
                   where t.EndDate.HasValue
                   && t.TaskID == taskID
                   select t;
        }

        private static void CheckDB()
        {
            if (db == null)
                db = new TimeEntryDataContext();
        }
    }
}
