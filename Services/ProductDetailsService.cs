using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class ProductDetailsService
    {
        private List<ProductDetails> _productDetails = new List<ProductDetails>();
        private ProductService _productService;

        public ProductDetailsService(ProductService productService)
        {
            _productService = productService;
            
            // Инициализация деталями продуктов для демонстрации
            AddProductDetails(new ProductDetails 
            { 
                ProductId = 1, 
                Color = "Титановый черный", 
                ScreenSize = "6.1 дюйма",
                Product = _productService.GetProductById(1)
            });
            
            AddProductDetails(new ProductDetails 
            { 
                ProductId = 2, 
                Color = "Синий", 
                ScreenSize = "6.1 дюйма",
                Product = _productService.GetProductById(2)
            });
            
            AddProductDetails(new ProductDetails 
            { 
                ProductId = 3, 
                Color = "Серебристый", 
                ScreenSize = "12.9 дюйма",
                Product = _productService.GetProductById(3)
            });
            
            AddProductDetails(new ProductDetails 
            { 
                ProductId = 4, 
                Color = "Серый космос", 
                ScreenSize = "13.6 дюйма",
                Product = _productService.GetProductById(4)
            });
        }

        public ProductDetails GetProductDetailsById(int productId)
        {
            return _productDetails.Find(pd => pd.ProductId == productId);
        }

        public void AddProductDetails(ProductDetails details)
        {
            var product = _productService.GetProductById(details.ProductId);
            if (product != null)
            {
                // Проверяем, существуют ли уже детали для этого продукта
                var existingDetails = GetProductDetailsById(details.ProductId);
                if (existingDetails != null)
                {
                    _productDetails.Remove(existingDetails);
                }
                
                _productDetails.Add(details);
                product.Details = details;
                
                Console.WriteLine($"Детали для продукта '{product.Name}' добавлены.");
            }
        }

        public void UpdateProductDetails(ProductDetails details)
        {
            var existingDetails = GetProductDetailsById(details.ProductId);
            if (existingDetails != null)
            {
                existingDetails.Color = details.Color;
                existingDetails.ScreenSize = details.ScreenSize;
                existingDetails.UpdateStorage();
            }
        }

        public void DisplayProductDetails(int productId)
        {
            var details = GetProductDetailsById(productId);
            var product = _productService.GetProductById(productId);
            
            if (details != null && product != null)
            {
                Console.WriteLine($"\n=== Детали продукта '{product.Name}' ===");
                Console.WriteLine($"Цвет: {details.Color}");
                Console.WriteLine($"Размер экрана: {details.ScreenSize}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Детали для продукта с ID {productId} не найдены.");
            }
        }
    }
}
