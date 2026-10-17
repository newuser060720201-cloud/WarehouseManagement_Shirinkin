using NUnit.Framework;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;
using System.Linq;

namespace WarehouseManagement_Shirinkin.Tests.ServicesTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            var orgService = new OrganizationService();
            var warehouseService = new WarehouseService(orgService);
            _service = new ProductService(warehouseService);
        }

        [Test]
        public void GetAllProducts_ShouldReturnProducts()
        {
            Assert.That(_service.GetAllProducts().Count, Is.GreaterThan(0));
        }
    }
}