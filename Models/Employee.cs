using System;
using System.Collections.Generic;

namespace AppleStore.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        // Связь один-ко-многим с Order
        public List<Order> Orders { get; set; } = new List<Order>();
        
        public override string ToString()
        {
            return $"Сотрудник: {FullName}\nДолжность: {Position}\nEmail: {Email}\nТелефон: {Phone}";
        }
    }
}
