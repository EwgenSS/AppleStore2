using System;
using System.Collections.Generic;

namespace AppleStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
        
        // Связи с Customer и Employee
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        
        // Связь один-ко-многим с OrderItem
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        // Метод из UML диаграммы
        public void CalculateTotal()
        {
            double total = 0;
            foreach (var item in OrderItems)
            {
                total += item.UnitPrice * item.Quantity;
            }
            TotalAmount = total;
            Console.WriteLine($"Общая сумма заказа: {TotalAmount:C}");
        }
        
        public override string ToString()
        {
            return $"Заказ №{OrderId}\nДата: {OrderDate}\nСтатус: {Status}\nСумма: {TotalAmount:C}";
        }
    }
}
