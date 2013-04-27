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
    /// Interaction logic for frmCompany.xaml
    /// </summary>
    public partial class frmCompany : Window
    {
        private Enums.ChangeTypes changeType;

        public frmCompany(Enums.ChangeTypes _type)
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(frmCompany_Loaded);

            changeType = _type;
        }

        void frmCompany_Loaded(object sender, RoutedEventArgs e)
        {
            switch (changeType)
            {
                case Enums.ChangeTypes.Add:
                    this.Title = "Add Company";
                    lblCompany.Visibility = Visibility.Hidden;
                    cboCompanies.Visibility = Visibility.Hidden;
                    btnSave.Content = "Add";
                    break;
                case Enums.ChangeTypes.Delete:
                    this.Title = "Delete Company";
                    lblCompanyName.Visibility = System.Windows.Visibility.Hidden;
                    txtCompanyName.Visibility = System.Windows.Visibility.Hidden;
                    btnSave.Content = "Delete";
                    cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
                    cboCompanies.SelectedValuePath = "ID";
                    cboCompanies.DisplayMemberPath = "CompanyName";
                    break;
                case Enums.ChangeTypes.Edit:
                    this.Title = "Edit Company";
                    btnSave.Content = "Save";
                    cboCompanies.ItemsSource = CompanyManager.GetAllCompanies();
                    cboCompanies.SelectedValuePath = "ID";
                    cboCompanies.DisplayMemberPath = "CompanyName";
                    break;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (changeType == Enums.ChangeTypes.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this company?", "Delete Company?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                long id = long.Parse(cboCompanies.SelectedValue.ToString());

                CompanyManager.DeleteCompany(id);
            }
            else if (changeType == Enums.ChangeTypes.Add)
            {
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    MessageBox.Show("Company Name is required");
                    return;
                }

                CompanyManager.AddCompany(txtCompanyName.Text.Trim());
            }
            else if (changeType == Enums.ChangeTypes.Edit)
            {
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    MessageBox.Show("Company Name is required");
                    return;
                }

                long id = long.Parse(cboCompanies.SelectedValue.ToString());

                CompanyManager.UpdateCompany(id, txtCompanyName.Text.Trim());
            }
        }

        private void cboCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCompanies.SelectedIndex < 0)
                return;

            long id = long.Parse(cboCompanies.SelectedValue.ToString());

            Company c = CompanyManager.GetCompany(id);
            txtCompanyName.Text = c.CompanyName;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
