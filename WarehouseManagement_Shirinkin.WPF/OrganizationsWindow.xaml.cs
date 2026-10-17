using System.Windows;
using System.Windows.Controls;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class OrganizationsWindow : Window
    {
        private OrganizationService _orgService = new OrganizationService();
        private Organization? _selectedOrganization;

        public OrganizationsWindow()
        {
            InitializeComponent();
            LoadOrganizations();
        }

        private void LoadOrganizations() => LvOrganizations.ItemsSource = _orgService.GetAll();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OrganizationDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrganizationName))
            {
                _orgService.Add(new Organization(dialog.OrganizationName));
                LoadOrganizations();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrganization == null) { MessageBox.Show("Выберите организацию"); return; }
            var dialog = new OrganizationDialog(_selectedOrganization.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrganizationName))
            {
                _selectedOrganization.Name = dialog.OrganizationName;
                _orgService.Update(_selectedOrganization);
                LoadOrganizations();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrganization == null) { MessageBox.Show("Выберите организацию"); return; }
            var dialog = new ConfirmDeleteDialog(_selectedOrganization.Name);
            if (dialog.ShowDialog() == true)
            {
                _orgService.Delete(_selectedOrganization.Id);
                _selectedOrganization = null;
                LoadOrganizations();
            }
        }

        private void LvOrganizations_SelectionChanged(object sender, SelectionChangedEventArgs e) => _selectedOrganization = LvOrganizations.SelectedItem as Organization;
        private void LvOrganizations_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => OpenMainWorkWindow();
        private void BtnSelect_Click(object sender, RoutedEventArgs e) => OpenMainWorkWindow();

        private void OpenMainWorkWindow()
        {
            if (_selectedOrganization == null) { MessageBox.Show("Выберите организацию"); return; }
            new MainWorkWindow(_selectedOrganization).Show();
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}