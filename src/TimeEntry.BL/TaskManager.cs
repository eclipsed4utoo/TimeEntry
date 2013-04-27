using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeEntry.DL;

namespace TimeEntry.BL
{
    //TODO:  total time doesn't update when clocking out.
    public class TaskManager
    {
        public static IQueryable<Task> GetAllTasks()
        {
            return TaskExtensions.GetAll();
        }

        public static Task GetTask(long id)
        {
            Task c = new Task();
            c = c.GetTask(id);
            return c;
        }

        public static long AddTask(string name, long companyID)
        {
            Task c = new Task();
            c.TaskName = name;
            c.CompanyID = companyID;
            return c.InsertTask();
        }

        public static void UpdateTask(long id, string name)
        {
            Task c = GetTask(id);
            c.TaskName = name;
            c.UpdateTask();
        }

        public static void DeleteTask(long id)
        {
            Task c = GetTask(id);
            c.DeleteTask();
        }

        public static TimeEntry.DL.TimeEntry GetLatestTimeEntry(long taskID)
        {
            Task c = GetTask(taskID);
            return c.GetLatestTimeEntry();
        }
    }
}
