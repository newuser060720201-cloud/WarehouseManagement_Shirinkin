using System.Windows;
using WarehouseManagement_Shirinkin.WPF.ViewModels;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class ConfirmDeleteDialog : Window
    {
        private string _orgName;

        public ConfirmDeleteDialog(string organizationName)
        {
            InitializeComponent();
            _orgName = organizationName;
            TxtOrgName.Text = organizationName;
            TxtConfirmName.Focus();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtConfirmName);
            if (TxtConfirmName.Text == _orgName)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                TxtConfirmName.Text = "";
                FieldValidation.Fail(TxtConfirmName, "Введённое название не совпадает с названием организации");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}