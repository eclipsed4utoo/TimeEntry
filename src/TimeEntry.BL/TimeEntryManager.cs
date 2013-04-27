using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeEntry.DL;

namespace TimeEntry.BL
{
    public class TimeEntryManager
    {
        #region Public Methods

        public static void AddTimeEntry(long taskID, DateTime date)
        {
            var timeEntry = GetLatestTimeEntry(taskID);

            if (timeEntry == null)
            {
                TimeEntry.DL.TimeEntry te = new DL.TimeEntry();
                te.TaskID = taskID;
                te.StartDate = date;
                te.CreatedDate = DateTime.Now;
                te.InsertTimeEntry();
            }
            else
            {
                if (timeEntry.EndDate.HasValue)
                {
                    TimeEntry.DL.TimeEntry t = new DL.TimeEntry();
                    t.TaskID = taskID;
                    t.StartDate = date;
                    t.CreatedDate = DateTime.Now;
                    t.InsertTimeEntry();
                }
                else
                {
                    timeEntry.EndDate = date;
                    timeEntry.UpdateTimeEntry();
                }
            }
        }

        public static bool IsClockIn(long taskID, out DateTime startDate)
        {
            startDate = DateTime.MinValue;

            var timeEntry = TaskManager.GetLatestTimeEntry(taskID);

            if (timeEntry == null)
                return true;

            if (timeEntry.EndDate.HasValue)
                return true;

            startDate = timeEntry.StartDate;

            return false;
        }

        public static void GetMonthlyHourTotal(long taskID, out double hours, out double minutes)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime endDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);

            double totalMinutes = 0;

            var timeEntries = TimeEntryExtensions.GetTimeEntriesForRange(taskID, startDate, endDate);

            foreach (var t in timeEntries)
            {
                totalMinutes += t.EndDate.Value.Subtract(t.StartDate).TotalMinutes;
            }

            hours = 0;
            minutes = 0;

            CalculateHoursAndMinutes(Math.Truncate(totalMinutes), out hours, out minutes);
        }

        public static void GetWeeklyHourTotal(long taskID, out double hours, out double minutes)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = now.AddDays(0 - (int)now.DayOfWeek);
            DateTime endDate = now.AddDays(6 - (int)now.DayOfWeek);

            double totalMinutes = 0;

            var timeEntries = TimeEntryExtensions.GetTimeEntriesForRange(taskID, startDate, endDate);

            foreach (var t in timeEntries)
            {
                totalMinutes += t.EndDate.Value.Subtract(t.StartDate).TotalMinutes;
            }

            hours = 0;
            minutes = 0;

            CalculateHoursAndMinutes(Math.Truncate(totalMinutes), out hours, out minutes);
        }

        public static void GetDailyHourTotal(long taskID, out double hours, out double minutes)
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date.AddDays(1);

            double totalMinutes = 0;

            var timeEntries = TimeEntryExtensions.GetTimeEntriesForRange(taskID, startDate, endDate);

            foreach (var t in timeEntries)
            {
                totalMinutes += t.EndDate.Value.Subtract(t.StartDate).TotalMinutes;
            }

            hours = 0;
            minutes = 0;

            CalculateHoursAndMinutes(Math.Truncate(totalMinutes), out hours, out minutes);
        }

        public static void GetTaskHourTotal(long taskID, out double hours, out double minutes)
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date.AddDays(1);

            double totalMinutes = 0;

            var timeEntries = TimeEntryExtensions.GetAllTimeEntriesForTask(taskID);

            foreach (var t in timeEntries)
            {
                totalMinutes += t.EndDate.Value.Subtract(t.StartDate).TotalMinutes;
            }

            hours = 0;
            minutes = 0;

            CalculateHoursAndMinutes(Math.Truncate(totalMinutes), out hours, out minutes);
        }

        #endregion

        #region Private Methods

        private static TimeEntry.DL.TimeEntry GetLatestTimeEntry(long taskID)
        {
            Task c = TaskManager.GetTask(taskID);
            return c.GetLatestTimeEntry();
        }

        private static void CalculateHoursAndMinutes(double totalMinutes, out double hours, out double minutes)
        {
            hours = 0;
            minutes = 0;

            hours = Math.Truncate(totalMinutes / 60);
            minutes = Math.Truncate(totalMinutes % 60);
        }

        #endregion
    }
}
