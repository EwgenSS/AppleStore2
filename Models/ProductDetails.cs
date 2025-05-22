using System;

namespace AppleStore.Models
{
    public class ProductDetails
    {
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string ScreenSize { get; set; }
        
        // Связь один-к-одному с Product
        public Product Product { get; set; }
        
        // Методы из UML диаграммы
        public string GetColor()
        {
            return Color;
        }
        
        public void UpdateStorage()
        {
            Console.WriteLine("Информация о хранилище обновлена.");
        }
        
        public override string ToString()
        {
            return $"Цвет: {Color}, Размер экрана: {ScreenSize}";
        }
    }
}
