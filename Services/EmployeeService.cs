using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class EmployeeService
    {
        private List<Employee> _employees = new List<Employee>();
        private int _nextId = 1;

        public EmployeeService()
        {
            // Инициализация некоторыми сотрудниками для демонстрации
            AddEmployee(new Employee 
            { 
                FullName = "Дмитрий Смирнов", 
                Position = "Менеджер", 
                Email = "dmitry@applestore.com", 
                Phone = "+7 (900) 111-22-33" 
            });
            
            AddEmployee(new Employee 
            { 
                FullName = "Елена Козлова", 
                Position = "Консультант", 
                Email = "elena@applestore.com", 
                Phone = "+7 (900) 222-33-44" 
            });
            
            AddEmployee(new Employee 
            { 
                FullName = "Сергей Николаев", 
                Position = "Техник", 
                Email = "sergey@applestore.com", 
                Phone = "+7 (900) 333-44-55" 
            });
        }

        public List<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee GetEmployeeById(int id)
        {
            return _employees.Find(e => e.EmployeeId == id);
        }

        public void AddEmployee(Employee employee)
        {
            employee.EmployeeId = _nextId++;
            _employees.Add(employee);
            Console.WriteLine($"Сотрудник '{employee.FullName}' добавлен.");
        }

        public void UpdateEmployee(Employee employee)
        {
            var existingEmployee = GetEmployeeById(employee.EmployeeId);
            if (existingEmployee != null)
            {
                existingEmployee.FullName = employee.FullName;
                existingEmployee.Position = employee.Position;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;
                Console.WriteLine($"Информация о сотруднике '{employee.FullName}' обновлена.");
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                _employees.Remove(employee);
                Console.WriteLine($"Сотрудник '{employee.FullName}' удален.");
            }
        }

        public void DisplayAllEmployees()
        {
            Console.WriteLine("\n=== Сотрудники ===");
            foreach (var employee in _employees)
            {
                Console.WriteLine($"ID: {employee.EmployeeId} - {employee.FullName} - {employee.Position} - {employee.Email}");
            }
            Console.WriteLine();
        }
    }
}
