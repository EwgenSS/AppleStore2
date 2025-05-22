using System;
using System.Collections.Generic;
using AppleStore.Models;

namespace AppleStore.Services
{
    public class OrderItemService
    {
        private List<OrderItem> _orderItems = new List<OrderItem>();
        private int _nextId = 1;
        private OrderService _orderService;
        private ProductService _productService;

        public OrderItemService(OrderService orderService, ProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public List<OrderItem> GetAllOrderItems()
        {
            return _orderItems;
        }

        public OrderItem GetOrderItemById(int id)
        {
            return _orderItems.Find(oi => oi.OrderItemId == id);
        }

        public List<OrderItem> GetOrderItemsByOrder(int orderId)
        {
            return _orderItems.FindAll(oi => oi.OrderId == orderId);
        }

        // Добавленный метод для добавления элемента заказа
        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem.OrderItemId == 0)
            {
                orderItem.OrderItemId = _nextId++;
            }
            
            _orderItems.Add(orderItem);
            
            // Добавляем элемент заказа в списки заказа и продукта
            if (orderItem.Order != null)
            {
                orderItem.Order.OrderItems.Add(orderItem);
            }
            
            if (orderItem.Product != null)
            {
                orderItem.Product.OrderItems.Add(orderItem);
            }
            
            Console.WriteLine($"Товар '{orderItem.Product?.Name ?? "Неизвестный"}' добавлен в заказ №{orderItem.OrderId}.");
        }

        public void AddItemToOrder(int orderId, int productId, int quantity)
        {
            var order = _orderService.GetOrderById(orderId);
            var product = _productService.GetProductById(productId);
            
            if (order != null && product != null)
            {
                // Проверяем наличие товара
                if (product.StockQuantity < quantity)
                {
                    Console.WriteLine($"Недостаточно товара '{product.Name}' на складе. Доступно: {product.StockQuantity}");
                    return;
                }
                
                // Проверяем, есть ли уже такой товар в заказе
                var existingItem = order.OrderItems.Find(oi => oi.ProductId == productId);
                if (existingItem != null)
                {
                    // Увеличиваем количество
                    existingItem.Quantity += quantity;
                    Console.WriteLine($"Количество товара '{product.Name}' в заказе №{orderId} увеличено до {existingItem.Quantity}.");
                }
                else
                {
                    // Создаем новый элемент заказа
                    var orderItem = new OrderItem
                    {
                        OrderItemId = _nextId++,
                        OrderId = orderId,
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price,
                        Order = order,
                        Product = product
                    };
                    
                    _orderItems.Add(orderItem);
                    order.OrderItems.Add(orderItem);
                    product.OrderItems.Add(orderItem);
                    
                    Console.WriteLine($"Товар '{product.Name}' добавлен в заказ №{orderId}.");
                }
                
                // Обновляем количество товара на складе
                product.UpdateStockQty(product.StockQuantity - quantity);
                
                // Пересчитываем общую сумму заказа
                _orderService.CalculateOrderTotal(orderId);
            }
            else
            {
                Console.WriteLine("Не удалось добавить товар в заказ. Проверьте ID заказа и товара.");
            }
        }

        public void RemoveItemFromOrder(int orderId, int productId, int quantity)
        {
            var order = _orderService.GetOrderById(orderId);
            var product = _productService.GetProductById(productId);
            
            if (order != null && product != null)
            {
                var orderItem = order.OrderItems.Find(oi => oi.ProductId == productId);
                if (orderItem != null)
                {
                    if (orderItem.Quantity <= quantity)
                    {
                        // Удаляем товар из заказа полностью
                        order.OrderItems.Remove(orderItem);
                        _orderItems.Remove(orderItem);
                        product.OrderItems.Remove(orderItem);
                        
                        Console.WriteLine($"Товар '{product.Name}' удален из заказа №{orderId}.");
                    }
                    else
                    {
                        // Уменьшаем количество
                        orderItem.Quantity -= quantity;
                        Console.WriteLine($"Количество товара '{product.Name}' в заказе №{orderId} уменьшено до {orderItem.Quantity}.");
                    }
                    
                    // Возвращаем товар на склад
                    product.UpdateStockQty(product.StockQuantity + quantity);
                    
                    // Пересчитываем общую сумму заказа
                    _orderService.CalculateOrderTotal(orderId);
                }
                else
                {
                    Console.WriteLine($"Товар '{product.Name}' не найден в заказе №{orderId}.");
                }
            }
            else
            {
                Console.WriteLine("Не удалось удалить товар из заказа. Проверьте ID заказа и товара.");
            }
        }

        public void UpdateItemQuantity(int orderId, int productId, int newQuantity)
        {
            var order = _orderService.GetOrderById(orderId);
            var product = _productService.GetProductById(productId);
            
            if (order != null && product != null)
            {
                var orderItem = order.OrderItems.Find(oi => oi.ProductId == productId);
                if (orderItem != null)
                {
                    int quantityDifference = newQuantity - orderItem.Quantity;
                    
                    // Проверяем наличие товара, если нужно увеличить количество
                    if (quantityDifference > 0 && product.StockQuantity < quantityDifference)
                    {
                        Console.WriteLine($"Недостаточно товара '{product.Name}' на складе. Доступно: {product.StockQuantity}");
                        return;
                    }
                    
                    // Обновляем количество товара в заказе
                    orderItem.Quantity = newQuantity;
                    
                    // Обновляем количество товара на складе
                    product.UpdateStockQty(product.StockQuantity - quantityDifference);
                    
                    // Пересчитываем общую сумму заказа
                    _orderService.CalculateOrderTotal(orderId);
                    
                    Console.WriteLine($"Количество товара '{product.Name}' в заказе №{orderId} изменено на {newQuantity}.");
                }
                else
                {
                    Console.WriteLine($"Товар '{product.Name}' не найден в заказе №{orderId}.");
                }
            }
            else
            {
                Console.WriteLine("Не удалось обновить количество товара в заказе. Проверьте ID заказа и товара.");
            }
        }

        public void DisplayOrderItems(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order != null)
            {
                Console.WriteLine($"\n=== Товары в заказе №{orderId} ===");
                if (order.OrderItems.Count > 0)
                {
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"- {item.Product?.Name ?? "Неизвестный"} x{item.Quantity} = {item.UnitPrice * item.Quantity:C}");
                    }
                }
                else
                {
                    Console.WriteLine("В заказе нет товаров.");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
            }
        }
    }
}
