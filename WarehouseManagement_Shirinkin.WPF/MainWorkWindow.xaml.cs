using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class MainWorkWindow : Window
    {
        private Organization _organization;
        private WarehouseService _warehouseService;
        private ProductService _productService;
        private InvoiceService _invoiceService;
        private Warehouse _selectedWarehouse;
        private List<Product> _allProducts;

        public MainWorkWindow(Organization organization)
        {
            InitializeComponent();
            _organization = organization;
            var orgService = new OrganizationService();
            _warehouseService = new WarehouseService(orgService);
            _productService = new ProductService(_warehouseService);
            _invoiceService = new InvoiceService(_productService);

            TxtOrgName.Text = _organization.Name;
            LoadWarehouses();
        }

        private void LoadWarehouses() => LvWarehouses.ItemsSource = _warehouseService.GetByOrganizationId(_organization.Id);

        private void LoadProducts()
        {
            if (_selectedWarehouse != null)
            {
                _allProducts = _productService.GetProductsByWarehouse(_selectedWarehouse.Id).ToList();
                ApplyFilterAndSort();
                TxtWarehouseName.Text = _selectedWarehouse.Name;
            }
            else
            {
                LvProducts.ItemsSource = null;
                TxtWarehouseName.Text = "[не выбран]";
            }
        }

        private void ApplyFilterAndSort()
        {
            if (_allProducts == null) return;
            var query = _allProducts.AsEnumerable();

            string searchText = TxtSearch.Text;
            if (!string.IsNullOrEmpty(searchText) && searchText != "Поиск по названию или артикулу...")
            {
                query = query.Where(p => p.Name.ToLower().Contains(searchText.ToLower())
                                      || p.Article.ToLower().Contains(searchText.ToLower()));
            }

            var sortItem = (CmbSort.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (sortItem)
            {
                case "По цене (возр.)": query = query.OrderBy(p => p.Price); break;
                case "По цене (убыв.)": query = query.OrderByDescending(p => p.Price); break;
                case "По остатку": query = query.OrderByDescending(p => p.StockQuantity); break;
                default: query = query.OrderBy(p => p.Name); break;
            }
            LvProducts.ItemsSource = query.ToList();
        }

        private void LvWarehouses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedWarehouse = LvWarehouses.SelectedItem as Warehouse;
            LoadProducts();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilterAndSort();
        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilterAndSort();

        private void BtnAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WarehouseDialog();
            if (dialog.ShowDialog() == true)
            {
                _warehouseService.Add(new Warehouse(dialog.WarehouseName, dialog.WarehouseAddress, _organization.Id));
                LoadWarehouses();
            }
        }

        private void BtnEditWarehouse_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse == null) return;
            var dialog = new WarehouseDialog(_selectedWarehouse.Name, _selectedWarehouse.Address);
            if (dialog.ShowDialog() == true)
            {
                _selectedWarehouse.Name = dialog.WarehouseName;
                _selectedWarehouse.Address = dialog.WarehouseAddress;
                _warehouseService.Update(_selectedWarehouse);
                LoadWarehouses();
            }
        }

        private void BtnDeleteWarehouse_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse == null) return;
            if (MessageBox.Show("Удалить склад?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _warehouseService.Delete(_selectedWarehouse.Id);
                _selectedWarehouse = null;
                LoadWarehouses();
                LoadProducts();
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse == null) { MessageBox.Show("Выберите склад"); return; }
            var dialog = new ProductDialog(_productService.GetAllCategories(), _productService.GetAllManufacturers(), _productService.GetAllSuppliers());
            if (dialog.ShowDialog() == true && dialog.Product != null)
            {
                _productService.Add(dialog.Product);
                LoadProducts();
            }
        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (LvProducts.SelectedItem is not Product selected) return;
            var dialog = new ProductDialog(_productService.GetAllCategories(), _productService.GetAllManufacturers(), _productService.GetAllSuppliers(), selected);
            if (dialog.ShowDialog() == true)
            {
                _productService.Update(dialog.Product);
                LoadProducts();
            }
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (LvProducts.SelectedItem is not Product selected) return;
            if (MessageBox.Show($"Удалить {selected.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _productService.Delete(selected.Article);
                LoadProducts();
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var lines = System.IO.File.ReadAllLines(dialog.FileName);
                    int imported = 0;
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var parts = lines[i].Split(',');
                        if (parts.Length >= 3)
                        {
                            _productService.Add(new Product { Name = parts[0].Trim(), Price = decimal.TryParse(parts[1], out var p) ? p : 0, StockQuantity = int.TryParse(parts[2], out var s) ? s : 0 });
                            imported++;
                        }
                    }
                    LoadProducts();
                    MessageBox.Show($"Импортировано: {imported}");
                }
                catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
            }
        }

        private void BtnInvoiceIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse == null) { MessageBox.Show("Выберите склад"); return; }
            var dialog = new InvoiceDialog(_productService.GetAllProducts(), InvoiceType.Incoming);
            if (dialog.ShowDialog() == true && dialog.Invoice != null)
            {
                dialog.Invoice.WarehouseId = _selectedWarehouse.Id;
                if (_invoiceService.ConfirmInvoice(dialog.Invoice)) { LoadProducts(); MessageBox.Show("Приход проведен"); }
            }
        }

        private void BtnInvoiceOut_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse == null) { MessageBox.Show("Выберите склад"); return; }
            var dialog = new InvoiceDialog(_productService.GetAllProducts(), InvoiceType.Outgoing);
            if (dialog.ShowDialog() == true && dialog.Invoice != null)
            {
                dialog.Invoice.WarehouseId = _selectedWarehouse.Id;
                if (_invoiceService.ConfirmInvoice(dialog.Invoice)) { LoadProducts(); MessageBox.Show("Расход проведен"); }
                else MessageBox.Show("Недостаточно товара");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new OrganizationsWindow().Show();
            this.Close();
        }
    }
}