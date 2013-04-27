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
using System.Windows.Shapes;
using TimeEntry.Common;
using TimeEntry.BL;
using TimeEntry.DL;

namespace TimeEntry
{
    /// <summary>
    /// Interaction logic for frmTask.xaml
    /// </summary>
    public partial class frmTask : Window
    {
        private Enums.ChangeTypes changeType;

        public frmTask(Enums.ChangeTypes _type)
        {
            InitializeComponent();

            changeType = _type;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (changeType)
            {
                case Enums.ChangeTypes.Add:
                    lblTasks.Visibility = System.Windows.Visibility.Hidden;
                    cboTasks.Visibility = System.Windows.Visibility.Hidden;
                    cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
                    cboCompanies.DisplayMemberPath = "CompanyName";
                    cboCompanies.SelectedValuePath = "ID";
                    btnSave.Content = "Add";
                    this.Title = "Add Task";
                    break;
                case Enums.ChangeTypes.Delete:
                    lblTaskName.Visibility = System.Windows.Visibility.Hidden;
                    txtTaskName.Visibility = System.Windows.Visibility.Hidden;
                    cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
                    cboCompanies.DisplayMemberPath = "CompanyName";
                    cboCompanies.SelectedValuePath = "ID";
                    btnSave.Content = "Delete";
                    this.Title = "Delete Task";
                    break;
                case Enums.ChangeTypes.Edit:
                    cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
                    cboCompanies.DisplayMemberPath = "CompanyName";
                    cboCompanies.SelectedValuePath = "ID";
                    btnSave.Content = "Save";
                    this.Title = "Edit Task";
                    break;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (changeType == Enums.ChangeTypes.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                long id = long.Parse(cboTasks.SelectedValue.ToString());

                TaskManager.DeleteTask(id);
            }
            else if (changeType == Enums.ChangeTypes.Add)
            {
                if (string.IsNullOrWhiteSpace(txtTaskName.Text))
                {
                    MessageBox.Show("Task Name is required");
                    return;
                }

                long companyID = long.Parse(cboCompanies.SelectedValue.ToString());

                TaskManager.AddTask(txtTaskName.Text.Trim(), companyID);
            }
            else if (changeType == Enums.ChangeTypes.Edit)
            {
                if (string.IsNullOrWhiteSpace(txtTaskName.Text))
                {
                    MessageBox.Show("Task Name is required");
                    return;
                }

                long id = long.Parse(cboTasks.SelectedValue.ToString());

                TaskManager.UpdateTask(id, txtTaskName.Text.Trim());
            }
        }

        private void cboCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long id = long.Parse(cboCompanies.SelectedValue.ToString());

            cboTasks.ItemsSource = CompanyManager.GetTasks(id);
            cboTasks.DisplayMemberPath = "TaskName";
            cboTasks.SelectedValuePath = "ID";
            txtTaskName.Text = string.Empty;
        }

        private void cboTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long id = long.Parse(cboTasks.SelectedValue.ToString());

            Task t = TaskManager.GetTask(id);
            txtTaskName.Text = t.TaskName;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
