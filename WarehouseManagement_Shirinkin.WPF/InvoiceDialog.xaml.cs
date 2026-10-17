using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.WPF.ViewModels;

namespace WarehouseManagement_Shirinkin.WPF
{
    public partial class InvoiceDialog : Window
    {
        public Invoice? Invoice { get; private set; }

        public InvoiceDialog(List<Product> products, InvoiceType type)
        {
            InitializeComponent();
            Invoice = new Invoice(type, 0);
            Title = type == InvoiceType.Incoming ? "Приходная накладная" : "Расходная накладная";
            CmbProducts.ItemsSource = products;
            if (products.Any()) CmbProducts.SelectedIndex = 0;
            UpdateTotal();
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProducts.SelectedItem is not Product product) return;
            FieldValidation.Reset(TxtQuantity);
            if (!int.TryParse(TxtQuantity.Text, out var quantity) || quantity <= 0)
            {
                FieldValidation.Fail(TxtQuantity, "Количество > 0");
                return;
            }

            var existing = Invoice!.Items.FirstOrDefault(i => i.ProductArticle == product.Article);
            if (existing != null) existing.Quantity += quantity;
            else Invoice.Items.Add(new InvoiceItem(product, quantity));

            LvItems.ItemsSource = Invoice.Items.ToList();
            UpdateTotal();
            TxtQuantity.Text = "1";
        }

        private void UpdateTotal()
        {
            TxtTotal.Text = $"{Invoice!.Items.Sum(i => i.Total):N0} ₽";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (Invoice!.Items.Count == 0) { MessageBox.Show("Добавьте товары"); return; }
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