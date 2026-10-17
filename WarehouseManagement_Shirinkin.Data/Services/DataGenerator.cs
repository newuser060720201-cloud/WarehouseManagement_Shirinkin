using System.Collections.Generic;
using WarehouseManagement_Shirinkin.Data.Models;

namespace WarehouseManagement_Shirinkin.Data.Services
{
    public static class DataGenerator
    {
        public static List<Organization> GenerateOrganizations()
        {
            var names = new[] { "ООО ТехноСфера", "АО УралМаш", "ИП Смирнов А.А.", "ООО СпортЛэнд", "ЗАО АгроПром" };
            var result = new List<Organization>();
            for (int i = 0; i < names.Length; i++) result.Add(new Organization(names[i]) { Id = i + 1 });
            return result;
        }

        public static List<Category> GenerateCategories()
        {
            var names = new[] { "Электроинструмент", "Ручной инструмент", "Спорттовары", "Садовая техника", "Крепёж" };
            var result = new List<Category>();
            for (int i = 0; i < names.Length; i++) result.Add(new Category(names[i]) { Id = i + 1 });
            return result;
        }

        public static List<Manufacturer> GenerateManufacturers()
        {
            var names = new[] { "Bosch", "Makita", "DeWalt", "Stihl", "Gardena", "Stels" };
            var result = new List<Manufacturer>();
            for (int i = 0; i < names.Length; i++) result.Add(new Manufacturer(names[i]) { Id = i + 1 });
            return result;
        }

        public static List<Supplier> GenerateSuppliers()
        {
            var names = new[] { "ООО ОптТорг", "Урал-Снаб", "МастерЛайн", "СпортМаркет", "ГринТрейд" };
            var result = new List<Supplier>();
            for (int i = 0; i < names.Length; i++) result.Add(new Supplier(names[i]) { Id = i + 1 });
            return result;
        }

        public static List<Warehouse> GenerateWarehouses(List<Organization> organizations)
        {
            var data = new (int OrgIndex, string Name, string Address)[]
            {
                (0, "Склад Юг-1", "г. Краснодар, ул. Красная, 15"),
                (0, "Склад Юг-2", "г. Краснодар, ул. Северная, 200"),
                (1, "Главный цех", "г. Екатеринбург, ул. Машиностроителей, 10"),
                (2, "Склад на Заводской", "г. Пермь, ул. Заводская, 5"),
                (3, "Спорт-База", "г. Челябинск, ул. Ленина, 50")
            };
            var warehouses = new List<Warehouse>();
            int whId = 1;
            foreach (var item in data)
            {
                if (item.OrgIndex >= organizations.Count) continue;
                warehouses.Add(new Warehouse(item.Name, item.Address, organizations[item.OrgIndex].Id) { Id = whId++ });
            }
            return warehouses;
        }

        public static List<Product> GenerateProducts(List<Category> categories, List<Manufacturer> manufacturers, List<Supplier> suppliers)
        {
            var data = new (string Name, decimal Price, decimal Discount, int Stock, string Unit, string Category, string Manufacturer, string Supplier)[]
            {
                ("Дрель ударная Bosch GSB 13 RE", 9190, 5, 25, "шт", "Электроинструмент", "Bosch", "ООО ОптТорг"),
                ("Перфоратор Makita HR2470", 14990, 0, 15, "шт", "Электроинструмент", "Makita", "Урал-Снаб"),
                ("УШМ DeWalt DWE4157", 8290, 10, 30, "шт", "Электроинструмент", "DeWalt", "ООО ОптТорг"),
                ("Набор ключей Stanley 12 предметов", 1890, 0, 50, "наб", "Ручной инструмент", "DeWalt", "МастерЛайн"),
                ("Велосипед Stels Navigator 700", 24990, 15, 8, "шт", "Спорттовары", "Stels", "СпортМаркет"),
                ("Бензопила Stihl MS 180", 16990, 8, 12, "шт", "Садовая техника", "Stihl", "ГринТрейд"),
                ("Газонокосилка Gardena PowerMax 32", 21990, 5, 5, "шт", "Садовая техника", "Gardena", "ГринТрейд"),
                ("Саморезы по дереву 4×50 (500 шт)", 450, 0, 200, "уп", "Крепёж", "Bosch", "Урал-Снаб")
            };

            var products = new List<Product>();
            int article = 1;
            foreach (var item in data)
            {
                var category = categories.Find(c => c.Name == item.Category) ?? categories[0];
                var manufacturer = manufacturers.Find(m => m.Name == item.Manufacturer) ?? manufacturers[0];
                var supplier = suppliers.Find(s => s.Name == item.Supplier) ?? suppliers[0];

                products.Add(new Product(item.Name, item.Price)
                {
                    Article = (article++).ToString("D000"),
                    Unit = item.Unit,
                    DiscountPercent = item.Discount,
                    StockQuantity = item.Stock,
                    CategoryId = category.Id,
                    ManufacturerId = manufacturer.Id,
                    SupplierId = supplier.Id,
                    Category = category,
                    Manufacturer = manufacturer,
                    Supplier = supplier,
                    SupplierName = supplier.Name
                });
            }
            return products;
        }
    }
}