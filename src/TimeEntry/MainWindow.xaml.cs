using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeEntry.Common;
using TimeEntry.BL;
using TimeEntry.DL;

namespace TimeEntry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        bool isClockIn = true;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        #endregion

        #region Window Events

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBUtilities.CreateDatabase();

            LoadCompanies();   
        }

        #endregion
        
        #region Menu Button Clicks

        private void mmuAddCompany_Click(object sender, RoutedEventArgs e)
        {
            frmCompany f = new frmCompany(Enums.ChangeTypes.Add);
            f.ShowDialog();

            LoadCompanies();
        }

        private void mmuEditCompany_Click(object sender, RoutedEventArgs e)
        {
            frmCompany f = new frmCompany(Enums.ChangeTypes.Edit);
            f.ShowDialog();

            LoadCompanies();
        }

        private void mmuDeleteCompany_Click(object sender, RoutedEventArgs e)
        {
            frmCompany f = new frmCompany(Enums.ChangeTypes.Delete);
            f.ShowDialog();

            LoadCompanies();
        }

        private void mmuAddTask_Click(object sender, RoutedEventArgs e)
        {
            frmTask f = new frmTask(Enums.ChangeTypes.Add);
            f.ShowDialog();
        }

        private void mmuEditTask_Click(object sender, RoutedEventArgs e)
        {
            frmTask f = new frmTask(Enums.ChangeTypes.Edit);
            f.ShowDialog();
        }

        private void mmuDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            frmTask f = new frmTask(Enums.ChangeTypes.Delete);
            f.ShowDialog();
        }

        #endregion

        #region ComboBox Events

        private void cboCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long companyID = long.Parse(cboCompanies.SelectedValue.ToString());

            cboTasks.ItemsSource = CompanyManager.GetTasks(companyID);
            cboTasks.DisplayMemberPath = "TaskName";
            cboTasks.SelectedValuePath = "ID";
        }

        private void cboTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long taskID = long.Parse(cboTasks.SelectedValue.ToString());
            DateTime startDate = DateTime.MinValue;

            isClockIn = TimeEntryManager.IsClockIn(taskID, out startDate);

            if (!isClockIn)
            {
                btnClock.Content = "Clock Out";
                lblStartDate.Text = startDate.ToString();
            }

            CalculateTotalMonthlyHours(taskID);
            CalculateTotalWeeklyHours(taskID);
            CalculateTotalDailyHours(taskID);
            CalculateTotalTaskHours(taskID);
        }

        #endregion

        #region Button Click Events

        private void btnClock_Click(object sender, RoutedEventArgs e)
        {
            if (cboCompanies.SelectedIndex < 0)
            {
                MessageBox.Show("Must select a Company");
                return;
            }

            if (cboTasks.SelectedIndex < 0)
            {
                MessageBox.Show("Must select a Task");
                return;
            }

            long taskID = long.Parse(cboTasks.SelectedValue.ToString());
            DateTime startDate = DateTime.Now;

            TimeEntryManager.AddTimeEntry(taskID, startDate);

            if (isClockIn)
            {
                lblStartDate.Text = startDate.ToString();
                lblEndDate.Text = string.Empty;
                btnClock.Content = "Clock Out";
                isClockIn = false;
            }
            else
            {
                lblEndDate.Text = startDate.ToString();
                btnClock.Content = "Clock In";
                isClockIn = true;

                CalculateTotalDailyHours(taskID);
                CalculateTotalMonthlyHours(taskID);
                CalculateTotalWeeklyHours(taskID);
            }
        }

        #endregion

        #region Custom Methods

        private void LoadCompanies()
        {
            cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
            cboCompanies.DisplayMemberPath = "CompanyName";
            cboCompanies.SelectedValuePath = "ID";

            cboCompanies.SelectedIndex = -1;
        }

        private void CalculateTotalTaskHours(long taskID)
        {
            double totalTaskHours = 0;
            double totalTaskMinutes = 0;
            TimeEntryManager.GetTaskHourTotal(taskID, out totalTaskHours, out totalTaskMinutes);
            lblTotalTaskHours.Text = string.Format("{0}h {1}m", totalTaskHours, totalTaskMinutes);
        }

        private void CalculateTotalDailyHours(long taskID)
        {
            double totalDailyHours = 0;
            double totalDailyMinutes = 0;
            TimeEntryManager.GetDailyHourTotal(taskID, out totalDailyHours, out totalDailyMinutes);
            lblTotalDayHours.Text = string.Format("{0}h {1}m", totalDailyHours, totalDailyMinutes);
        }

        private void CalculateTotalWeeklyHours(long taskID)
        {
            double totalWeeklyHours = 0;
            double totalWeeklyMinutes = 0;
            TimeEntryManager.GetWeeklyHourTotal(taskID, out totalWeeklyHours, out totalWeeklyMinutes);
            lblTotalWeekHours.Text = string.Format("{0}h {1}m", totalWeeklyHours, totalWeeklyMinutes);
        }

        private void CalculateTotalMonthlyHours(long taskID)
        {
            double totalMonthHours = 0;
            double totalMonthMinutes = 0;
            TimeEntryManager.GetMonthlyHourTotal(taskID, out totalMonthHours, out totalMonthMinutes);
            lblTotalMonthHours.Text = string.Format("{0}h {1}m", totalMonthHours, totalMonthMinutes);
        }

        #endregion
    }
}
