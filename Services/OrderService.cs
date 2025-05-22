using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class OrderService
    {
        private List<Order> _orders = new List<Order>();
        private int _nextId = 1;
        private CustomerService _customerService;
        private EmployeeService _employeeService;
        private ProductService _productService;

        public OrderService(CustomerService customerService, EmployeeService employeeService, ProductService productService)
        {
            _customerService = customerService;
            _employeeService = employeeService;
            _productService = productService;
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        public Order GetOrderById(int id)
        {
            return _orders.Find(o => o.OrderId == id);
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _orders.FindAll(o => o.CustomerId == customerId);
        }

        public List<Order> GetOrdersByEmployee(int employeeId)
        {
            return _orders.FindAll(o => o.EmployeeId == employeeId);
        }

        // Добавленный метод для создания заказа
        public void AddOrder(Order order)
        {
            if (order.OrderId == 0)
            {
                order.OrderId = _nextId++;
            }
            
            _orders.Add(order);
            
            // Добавляем заказ в списки клиента и сотрудника
            if (order.Customer != null)
            {
                order.Customer.Orders.Add(order);
            }
            
            if (order.Employee != null)
            {
                order.Employee.Orders.Add(order);
            }
            
            Console.WriteLine($"Заказ №{order.OrderId} создан для клиента {order.Customer?.FullName ?? "Неизвестный"}.");
        }

        // Добавленный метод для обновления заказа
        public void UpdateOrder(Order order)
        {
            var existingOrder = GetOrderById(order.OrderId);
            if (existingOrder != null)
            {
                // Обновляем свойства заказа
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.EmployeeId = order.EmployeeId;
                existingOrder.Status = order.Status;
                existingOrder.TotalAmount = order.TotalAmount;
                
                Console.WriteLine($"Заказ №{order.OrderId} обновлен.");
            }
            else
            {
                Console.WriteLine($"Заказ с ID {order.OrderId} не найден.");
            }
        }

        // Добавленный метод для расчета общей суммы заказа
        public void CalculateTotal(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                double total = 0;
                foreach (var item in order.OrderItems)
                {
                    total += item.UnitPrice * item.Quantity;
                }
                
                order.TotalAmount = total;
                Console.WriteLine($"Общая сумма заказа №{orderId} рассчитана: {total:C}");
            }
            else
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
            }
        }

        public Order CreateOrder(int customerId, int employeeId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            var employee = _employeeService.GetEmployeeById(employeeId);
            
            if (customer != null && employee != null)
            {
                var order = new Order
                {
                    OrderId = _nextId++,
                    CustomerId = customerId,
                    EmployeeId = employeeId,
                    OrderDate = DateTime.Now,
                    Status = "Новый",
                    Customer = customer,
                    Employee = employee
                };
                
                _orders.Add(order);
                customer.Orders.Add(order);
                employee.Orders.Add(order);
                
                Console.WriteLine($"Заказ №{order.OrderId} создан для клиента {customer.FullName}.");
                return order;
            }
            
            Console.WriteLine("Не удалось создать заказ. Проверьте ID клиента и сотрудника.");
            return null;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = status;
                Console.WriteLine($"Статус заказа №{orderId} изменен на '{status}'.");
            }
        }

        public void CalculateOrderTotal(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.CalculateTotal();
            }
        }

        public void DisplayOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                Console.WriteLine($"\n=== Заказ №{order.OrderId} ===");
                Console.WriteLine($"Дата: {order.OrderDate}");
                Console.WriteLine($"Клиент: {order.Customer?.FullName ?? "Неизвестный"}");
                Console.WriteLine($"Сотрудник: {order.Employee?.FullName ?? "Неизвестный"}");
                Console.WriteLine($"Статус: {order.Status}");
                Console.WriteLine($"Сумма: {order.TotalAmount:C}");
                
                if (order.OrderItems.Count > 0)
                {
                    Console.WriteLine("\nТовары в заказе:");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"- {item.Product?.Name ?? "Неизвестный"} x{item.Quantity} = {item.UnitPrice * item.Quantity:C}");
                    }
                }
                else
                {
                    Console.WriteLine("\nВ заказе нет товаров.");
                }
                
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
            }
        }

        public void DisplayAllOrders()
        {
            Console.WriteLine("\n=== Все заказы ===");
            if (_orders.Count > 0)
            {
                foreach (var order in _orders)
                {
                    Console.WriteLine($"Заказ №{order.OrderId} - Клиент: {order.Customer?.FullName ?? "Неизвестный"} - Дата: {order.OrderDate.ToShortDateString()} - Статус: {order.Status} - Сумма: {order.TotalAmount:C}");
                }
            }
            else
            {
                Console.WriteLine("Заказов пока нет.");
            }
            Console.WriteLine();
        }
    }
}
