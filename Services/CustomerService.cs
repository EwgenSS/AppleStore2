using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class CustomerService
    {
        private List<Customer> _customers = new List<Customer>();
        private int _nextId = 1;

        public CustomerService()
        {
            // Инициализация некоторыми клиентами для демонстрации
            AddCustomer(new Customer 
            { 
                FullName = "Иван Петров", 
                Email = "ivan@example.com", 
                Phone = "+7 (900) 123-45-67", 
                Address = "г. Москва, ул. Ленина, 10" 
            });
            
            AddCustomer(new Customer 
            { 
                FullName = "Анна Сидорова", 
                Email = "anna@example.com", 
                Phone = "+7 (900) 987-65-43", 
                Address = "г. Санкт-Петербург, пр. Невский, 20" 
            });
            
            AddCustomer(new Customer 
            { 
                FullName = "Алексей Иванов", 
                Email = "alexey@example.com", 
                Phone = "+7 (900) 555-55-55", 
                Address = "г. Казань, ул. Баумана, 5" 
            });
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _customers.Find(c => c.CustomerId == id);
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerId = _nextId++;
            _customers.Add(customer);
            Console.WriteLine($"Клиент '{customer.FullName}' добавлен.");
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = GetCustomerById(customer.CustomerId);
            if (existingCustomer != null)
            {
                existingCustomer.FullName = customer.FullName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;
                Console.WriteLine($"Информация о клиенте '{customer.FullName}' обновлена.");
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
                Console.WriteLine($"Клиент '{customer.FullName}' удален.");
            }
        }

        public void DisplayAllCustomers()
        {
            Console.WriteLine("\n=== Клиенты ===");
            foreach (var customer in _customers)
            {
                Console.WriteLine($"ID: {customer.CustomerId} - {customer.FullName} - {customer.Email} - {customer.Phone}");
            }
            Console.WriteLine();
        }
    }
}
