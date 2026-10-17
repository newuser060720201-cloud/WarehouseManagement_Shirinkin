using NUnit.Framework;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;
using System.Linq;

namespace WarehouseManagement_Shirinkin.Tests.ServicesTests
{
    [TestFixture]
    public class InvoiceServiceTests
    {
        private InvoiceService _invoiceService;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            var orgService = new OrganizationService();
            var warehouseService = new WarehouseService(orgService);
            _productService = new ProductService(warehouseService);
            _invoiceService = new InvoiceService(_productService);
        }

        [Test]
        public void ConfirmIncomingInvoice_ShouldIncreaseStock()
        {
            var product = _productService.GetAllProducts().First();
            var initialStock = product.StockQuantity;
            var invoice = _invoiceService.CreateInvoice(InvoiceType.Incoming, 1);
            _invoiceService.AddItemToInvoice(invoice, product.Article, 10);

            var result = _invoiceService.ConfirmInvoice(invoice);

            Assert.That(result, Is.True);
            Assert.That(_productService.GetByArticle(product.Article).StockQuantity, Is.EqualTo(initialStock + 10));
        }
    }
}