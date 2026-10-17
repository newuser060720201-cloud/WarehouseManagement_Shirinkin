using NUnit.Framework;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;
using System.Linq;

namespace WarehouseManagement_Shirinkin.Tests.ServicesTests
{
    [TestFixture]
    public class WarehouseServiceTests
    {
        private WarehouseService _service;
        private OrganizationService _orgService;

        [SetUp]
        public void Setup()
        {
            _orgService = new OrganizationService();
            _service = new WarehouseService(_orgService);
        }

        [Test]
        public void GetAll_ShouldReturnWarehouses()
        {
            Assert.That(_service.GetAll().Count, Is.GreaterThan(0));
        }
    }
}