using System.Windows;
using WarehouseManagement_Shirinkin.WPF.ViewModels;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class OrganizationDialog : Window
    {
        public string OrganizationName => TxtOrganizationName.Text.Trim();

        public OrganizationDialog(string initialName = "")
        {
            InitializeComponent();
            TxtOrganizationName.Text = initialName;
            if (!string.IsNullOrEmpty(initialName)) Title = "Редактирование организации";
            TxtOrganizationName.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtOrganizationName);
            if (string.IsNullOrWhiteSpace(TxtOrganizationName.Text))
            {
                FieldValidation.Fail(TxtOrganizationName, "Введите наименование организации");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}