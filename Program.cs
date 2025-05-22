using System;
using AppleStore.Models;
using AppleStore.Services;
using AppleStore.UI;

namespace AppleStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Инициализация сервисов
            var categoryService = new CategoryService();
            var productService = new ProductService(categoryService);
            var productDetailsService = new ProductDetailsService(productService);
            var customerService = new CustomerService();
            var employeeService = new EmployeeService();
            var orderService = new OrderService(customerService, employeeService, productService);
            var orderItemService = new OrderItemService(orderService, productService);
            
            // Инициализация пользовательского интерфейса
            var consoleUI = new ConsoleUI(
                categoryService,
                productService,
                productDetailsService,
                customerService,
                employeeService,
                orderService,
                orderItemService
            );
            
            // Запуск приложения
            Console.WriteLine("Запуск Apple Store...");
            consoleUI.Start();
        }
    }
}
