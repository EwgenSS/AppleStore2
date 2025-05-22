using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class CategoryService
    {
        private List<Category> _categories = new List<Category>();
        private int _nextId = 1;

        public CategoryService()
        {
            // Инициализация некоторыми категориями для демонстрации
            AddCategory(new Category { Name = "iPhone", Description = "Смартфоны Apple" });
            AddCategory(new Category { Name = "iPad", Description = "Планшеты Apple" });
            AddCategory(new Category { Name = "Mac", Description = "Компьютеры Apple" });
            AddCategory(new Category { Name = "Watch", Description = "Умные часы Apple" });
            AddCategory(new Category { Name = "AirPods", Description = "Беспроводные наушники Apple" });
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(int id)
        {
            return _categories.Find(c => c.CategoryId == id);
        }

        public void AddCategory(Category category)
        {
            category.CategoryId = _nextId++;
            _categories.Add(category);
            category.AddCategory();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = GetCategoryById(category.CategoryId);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.UpdateCategory();
            }
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                _categories.Remove(category);
                category.DeleteCategory();
            }
        }

        public void DisplayAllCategories()
        {
            Console.WriteLine("\n=== Категории ===");
            foreach (var category in _categories)
            {
                Console.WriteLine($"ID: {category.CategoryId} - {category.Name}");
            }
            Console.WriteLine();
        }
    }
}
