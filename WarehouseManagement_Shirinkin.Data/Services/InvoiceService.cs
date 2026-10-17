using System.Collections.Generic;
using System.Linq;
using WarehouseManagement_Shirinkin.Data.Models;

namespace WarehouseManagement_Shirinkin.Data.Services
{
    public class InvoiceService
    {
        private List<Invoice> _invoices;
        private ProductService _productService;
        private int _nextId = 1;

        public InvoiceService(ProductService productService)
        {
            _productService = productService;
            _invoices = new List<Invoice>();
        }

        public Invoice CreateInvoice(InvoiceType type, long warehouseId)
        {
            var invoice = new Invoice(type, warehouseId) { Id = _nextId++ };
            return invoice;
        }

        public void AddItemToInvoice(Invoice invoice, string productArticle, int quantity)
        {
            var product = _productService.GetByArticle(productArticle);
            if (product == null) return;
            var existing = invoice.Items.FirstOrDefault(i => i.ProductArticle == productArticle);
            if (existing != null) existing.Quantity += quantity;
            else invoice.Items.Add(new InvoiceItem(product, quantity));
        }

        public void RemoveItemFromInvoice(Invoice invoice, string productArticle)
        {
            var item = invoice.Items.FirstOrDefault(i => i.ProductArticle == productArticle);
            if (item != null) invoice.Items.Remove(item);
        }

        public bool ConfirmInvoice(Invoice invoice)
        {
            if (invoice.IsConfirmed) return false;
            if (invoice.Type == InvoiceType.Outgoing)
            {
                foreach (var item in invoice.Items)
                {
                    var product = _productService.GetByArticle(item.ProductArticle);
                    if (product == null || product.StockQuantity < item.Quantity) return false;
                }
            }

            foreach (var item in invoice.Items)
            {
                int change = invoice.Type == InvoiceType.Incoming ? item.Quantity : -item.Quantity;
                _productService.UpdateStock(item.ProductArticle, change);
            }

            invoice.IsConfirmed = true;
            _invoices.Add(invoice);
            return true;
        }

        public List<Invoice> GetAllInvoices() => _invoices.ToList();
        public List<Invoice> GetInvoicesByWarehouse(long warehouseId) => _invoices.Where(i => i.WarehouseId == warehouseId).ToList();
    }
}