using System;
using System.Collections.Generic;

namespace AppleStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Связь один-ко-многим с Product
        public List<Product> Products { get; set; } = new List<Product>();
        
        // Методы из UML диаграммы
        public void AddCategory()
        {
            Console.WriteLine($"Категория '{Name}' добавлена.");
        }
        
        public void UpdateCategory()
        {
            Console.WriteLine($"Категория '{Name}' обновлена.");
        }
        
        public void DeleteCategory()
        {
            Console.WriteLine($"Категория '{Name}' удалена.");
        }
        
        public void GetCategoryById(int id)
        {
            Console.WriteLine($"Получена категория с ID: {id}");
        }
        
        public override string ToString()
        {
            return $"Категория: {Name}\nОписание: {Description}";
        }
    }
}
