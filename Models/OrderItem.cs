using System;

namespace AppleStore.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        
        // Связи с Order и Product
        public Order Order { get; set; }
        public Product Product { get; set; }
        
        public override string ToString()
        {
            return $"Товар: {Product?.Name ?? "Неизвестный"}\nКоличество: {Quantity}\nЦена за единицу: {UnitPrice:C}\nСумма: {UnitPrice * Quantity:C}";
        }
    }
}
