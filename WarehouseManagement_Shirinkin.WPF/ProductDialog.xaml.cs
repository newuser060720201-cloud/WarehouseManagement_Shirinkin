using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.WPF.ViewModels;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class ProductDialog : Window
    {
        public Product? Product { get; private set; }

        public ProductDialog(List<Category> categories, List<Manufacturer> manufacturers, List<Supplier> suppliers, Product? existingProduct = null)
        {
            InitializeComponent();
            CmbCategory.ItemsSource = categories;
            CmbManufacturer.ItemsSource = manufacturers;
            CmbSupplier.ItemsSource = suppliers;

            if (existingProduct != null)
            {
                Product = existingProduct;
                TxtName.Text = existingProduct.Name;
                TxtUnit.Text = existingProduct.Unit;
                TxtPrice.Text = existingProduct.Price.ToString();
                TxtDiscount.Text = existingProduct.DiscountPercent.ToString();
                TxtStockQuantity.Text = existingProduct.StockQuantity.ToString();
                CmbCategory.SelectedValue = existingProduct.CategoryId;
                CmbManufacturer.SelectedValue = existingProduct.ManufacturerId;
                CmbSupplier.SelectedValue = existingProduct.SupplierId;
            }
            else
            {
                Product = new Product();
                if (categories.Any()) CmbCategory.SelectedIndex = 0;
                if (manufacturers.Any()) CmbManufacturer.SelectedIndex = 0;
                if (suppliers.Any()) CmbSupplier.SelectedIndex = 0;
            }
        }

        // Этот метод позволяет перетаскивать окно мышкой за любое место
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtName, TxtPrice);
            if (string.IsNullOrWhiteSpace(TxtName.Text)) { FieldValidation.Fail(TxtName, "Введите наименование"); return; }
            if (!decimal.TryParse(TxtPrice.Text, out var price) || price <= 0) { FieldValidation.Fail(TxtPrice, "Цена должна быть > 0"); return; }
            if (!decimal.TryParse(TxtDiscount.Text, out var discount) || discount < 0 || discount > 100) { MessageBox.Show("Скидка от 0 до 100"); return; }

            Product!.Name = TxtName.Text;
            Product.Unit = TxtUnit.Text;
            Product.Price = price;
            Product.DiscountPercent = discount;
            Product.StockQuantity = int.TryParse(TxtStockQuantity.Text, out var stock) ? stock : 0;

            if (CmbCategory.SelectedItem is Category cat) { Product.CategoryId = cat.Id; Product.Category = cat; }
            if (CmbManufacturer.SelectedItem is Manufacturer man) { Product.ManufacturerId = man.Id; Product.Manufacturer = man; }
            if (CmbSupplier.SelectedItem is Supplier sup) { Product.SupplierId = sup.Id; Product.Supplier = sup; Product.SupplierName = sup.Name; }

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