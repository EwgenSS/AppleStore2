using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class ProductService
    {
        private List<Product> _products = new List<Product>();
        private int _nextId = 1;
        private CategoryService _categoryService;

        public ProductService(CategoryService categoryService)
        {
            _categoryService = categoryService;
            
            // Инициализация некоторыми продуктами для демонстрации
            var iPhoneCategory = _categoryService.GetCategoryById(1); // iPhone
            var iPadCategory = _categoryService.GetCategoryById(2); // iPad
            var macCategory = _categoryService.GetCategoryById(3); // Mac
            
            AddProduct(new Product 
            { 
                CategoryId = iPhoneCategory.CategoryId, 
                Name = "iPhone 15 Pro", 
                Description = "Флагманский смартфон Apple", 
                Price = 999.99, 
                StockQuantity = 50,
                Category = iPhoneCategory
            });
            
            AddProduct(new Product 
            { 
                CategoryId = iPhoneCategory.CategoryId, 
                Name = "iPhone 15", 
                Description = "Новый смартфон Apple", 
                Price = 799.99, 
                StockQuantity = 75,
                Category = iPhoneCategory
            });
            
            AddProduct(new Product 
            { 
                CategoryId = iPadCategory.CategoryId, 
                Name = "iPad Pro", 
                Description = "Мощный планшет для профессионалов", 
                Price = 1099.99, 
                StockQuantity = 30,
                Category = iPadCategory
            });
            
            AddProduct(new Product 
            { 
                CategoryId = macCategory.CategoryId, 
                Name = "MacBook Air", 
                Description = "Легкий и мощный ноутбук", 
                Price = 1299.99, 
                StockQuantity = 25,
                Category = macCategory
            });
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product GetProductById(int id)
        {
            return _products.Find(p => p.ProductId == id);
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _products.FindAll(p => p.CategoryId == categoryId);
        }

        public void AddProduct(Product product)
        {
            product.ProductId = _nextId++;
            _products.Add(product);
            
            // Добавляем продукт в список продуктов категории
            var category = _categoryService.GetCategoryById(product.CategoryId);
            if (category != null)
            {
                category.Products.Add(product);
            }
            
            Console.WriteLine($"Продукт '{product.Name}' добавлен.");
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = GetProductById(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                
                Console.WriteLine($"Продукт '{product.Name}' обновлен.");
            }
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _products.Remove(product);
                
                // Удаляем продукт из списка продуктов категории
                var category = _categoryService.GetCategoryById(product.CategoryId);
                if (category != null)
                {
                    category.Products.Remove(product);
                }
                
                Console.WriteLine($"Продукт '{product.Name}' удален.");
            }
        }

        public void UpdateStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product.UpdateStockQty(quantity);
            }
        }

        public void DisplayAllProducts()
        {
            Console.WriteLine("\n=== Продукты ===");
            foreach (var product in _products)
            {
                Console.WriteLine($"ID: {product.ProductId} - {product.Name} - {product.Price:C} - В наличии: {product.StockQuantity}");
            }
            Console.WriteLine();
        }

        public void DisplayProductsByCategory(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            if (category != null)
            {
                Console.WriteLine($"\n=== Продукты в категории '{category.Name}' ===");
                var products = GetProductsByCategory(categoryId);
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId} - {product.Name} - {product.Price:C} - В наличии: {product.StockQuantity}");
                }
                Console.WriteLine();
            }
        }
    }
}
