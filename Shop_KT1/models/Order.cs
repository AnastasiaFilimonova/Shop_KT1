using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime CreatedOrder { get; set; }
        public int CustomerId { get; set; }
        public User Customer { get; set; }
        public int? ManagerId { get; set; }
        public User Manager { get; set; }
        public string Status { get; set; } 
        public ICollection<OrderItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
