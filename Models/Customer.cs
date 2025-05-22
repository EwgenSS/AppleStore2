using System;
using System.Collections.Generic;

namespace AppleStore.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        
        // Связь один-ко-многим с Order
        public List<Order> Orders { get; set; } = new List<Order>();
        
        // Метод из UML диаграммы
        public Order PlaceOrder()
        {
            var order = new Order
            {
                CustomerId = this.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Новый"
            };
            
            Orders.Add(order);
            Console.WriteLine($"Заказ создан для клиента {FullName}");
            return order;
        }
        
        public override string ToString()
        {
            return $"Клиент: {FullName}\nEmail: {Email}\nТелефон: {Phone}\nАдрес: {Address}";
        }
    }
}
