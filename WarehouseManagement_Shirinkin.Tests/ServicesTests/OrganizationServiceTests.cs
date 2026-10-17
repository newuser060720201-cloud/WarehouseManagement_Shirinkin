using NUnit.Framework;
using WarehouseManagement_Shirinkin.Data.Models;
using WarehouseManagement_Shirinkin.Data.Services;
using System.Linq;

namespace WarehouseManagement_Shirinkin.Tests.ServicesTests
{
    [TestFixture]
    public class OrganizationServiceTests
    {
        private OrganizationService _service;

        [SetUp]
        public void Setup() => _service = new OrganizationService();

        [Test]
        public void GetAll_ShouldReturnOrganizations()
        {
            var result = _service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}