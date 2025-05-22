using System;
using System.Collections.Generic;

namespace AppleStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        
        // Связь с Category
        public Category Category { get; set; }
        
        // Связь один-к-одному с ProductDetails
        public ProductDetails Details { get; set; }
        
        // Связь с OrderItem
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        // Методы из UML диаграммы
        public void GetProduct(int id)
        {
            Console.WriteLine($"Получен продукт с ID: {id}");
        }
        
        public string GetProductInfo()
        {
            return $"Продукт: {Name}\nОписание: {Description}\nЦена: {Price:C}\nВ наличии: {StockQuantity}";
        }
        
        public void UpdateStockQty(int qty)
        {
            StockQuantity = qty;
            Console.WriteLine($"Количество товара '{Name}' обновлено до {StockQuantity}");
        }
        
        public double CalculateDiscount()
        {
            // Простая логика расчета скидки (например, 10% если товара больше 10 штук)
            if (StockQuantity > 10)
                return Price * 0.1;
            return 0;
        }
        
        public override string ToString()
        {
            return $"{Name} - {Price:C}";
        }
    }
}
