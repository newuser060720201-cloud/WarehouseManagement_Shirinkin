using System.Windows;
using WarehouseManagement_Shirinkin.WPF.ViewModels;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class WarehouseDialog : Window
    {
        public string WarehouseName => TxtWarehouseName.Text.Trim();
        public string WarehouseAddress => TxtWarehouseAddress.Text.Trim();

        public WarehouseDialog(string initialName = "", string initialAddress = "")
        {
            InitializeComponent();
            TxtWarehouseName.Text = initialName;
            TxtWarehouseAddress.Text = initialAddress;
            if (!string.IsNullOrEmpty(initialName)) Title = "Редактирование склада";
            TxtWarehouseName.Focus();
        }

        // Метод для перетаскивания окна
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtWarehouseName, TxtWarehouseAddress);
            if (string.IsNullOrWhiteSpace(TxtWarehouseName.Text))
            {
                FieldValidation.Fail(TxtWarehouseName, "Введите название склада");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtWarehouseAddress.Text))
            {
                FieldValidation.Fail(TxtWarehouseAddress, "Введите адрес склада");
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