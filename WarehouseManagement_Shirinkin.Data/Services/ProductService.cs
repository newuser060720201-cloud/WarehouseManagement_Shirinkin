using System.Collections.Generic;
using System.Linq;
using WarehouseManagement_Shirinkin.Data.Models;

namespace WarehouseManagement_Shirinkin.Data.Services
{
    public class ProductService
    {
        private List<Product> _products;
        private List<Category> _categories;
        private List<Manufacturer> _manufacturers;
        private List<Supplier> _suppliers;

        public ProductService(WarehouseService warehouseService)
        {
            _categories = DataGenerator.GenerateCategories();
            _manufacturers = DataGenerator.GenerateManufacturers();
            _suppliers = DataGenerator.GenerateSuppliers();
            _products = DataGenerator.GenerateProducts(_categories, _manufacturers, _suppliers);
        }

        public List<Product> GetAllProducts() => _products.ToList();
        public List<Product> GetProductsByWarehouse(long warehouseId) => _products.Where(p => p.StockQuantity > 0).ToList();
        public Product? GetByArticle(string article) => _products.FirstOrDefault(p => p.Article == article);

        public void Add(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Article))
            {
                int max = _products.Select(p => int.TryParse(p.Article, out var n) ? n : 0).DefaultIfEmpty(0).Max();
                product.Article = (max + 1).ToString("D000");
            }
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Article == product.Article);
            if (index != -1) _products[index] = product;
        }

        public void Delete(string article)
        {
            var product = GetByArticle(article);
            if (product != null) _products.Remove(product);
        }

        public List<Category> GetAllCategories() => _categories.ToList();
        public List<Manufacturer> GetAllManufacturers() => _manufacturers.ToList();
        public List<Supplier> GetAllSuppliers() => _suppliers.ToList();

        public void UpdateStock(string article, int quantityChange)
        {
            var product = GetByArticle(article);
            if (product != null) product.StockQuantity += quantityChange;
        }
    }
}