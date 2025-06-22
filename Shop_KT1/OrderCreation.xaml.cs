using Microsoft.EntityFrameworkCore;
using Shop_KT1.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop_KT1
{
    /// <summary>
    /// Логика взаимодействия для OrderCreation.xaml
    /// </summary>
    public partial class OrderCreation : Window
    {
        private ObservableCollection<OrderItemDisplay> OrderItems { get; set; } = new();
        private List<Product> Products;
        private readonly int currentCustomerId;
        private readonly ApplicationContext db = new();
        public OrderCreation(int customerId)
        {
            InitializeComponent();
            currentCustomerId = customerId;
            LoadProducts();
            OrderItemsGrid.ItemsSource = OrderItems;
        }
        private void LoadProducts()
        {
            Products = db.Products
                         .Include(p => p.Materials)
                             .ThenInclude(pm => pm.Material)
                         .ToList();

            ProductComboBox.ItemsSource = Products;
        }
        private decimal CalculateProductCost(Product product)
        {
            decimal totalCost = 0;

            foreach (var pm in product.Materials)
            {
                if (pm.Material == null)
                    db.Entry(pm).Reference(x => x.Material).Load();

                totalCost += (decimal)pm.Quantity * pm.Material.Price;
            }
            return totalCost;
        }
        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Выберите изделие.");
                return;
            }
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество.");
                return;
            }
            decimal calculatedCost = CalculateProductCost(selectedProduct);
            var existingItem = OrderItems.FirstOrDefault(i => i.ProductId == selectedProduct.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Total = existingItem.Quantity * existingItem.Price;
            }
            else
            {
                OrderItems.Add(new OrderItemDisplay
                {
                    ProductId = selectedProduct.ProductID,
                    ProductName = selectedProduct.Name,
                    Quantity = quantity,
                    Price = calculatedCost,
                    Total = calculatedCost * quantity
                });
            }
            UpdateTotalPrice();
        }
        private void UpdateTotalPrice()
        {
            decimal total = OrderItems.Sum(i => i.Total);
            TotalTextBlock.Text = $"Итоговая стоимость: {total:C}";
        }
        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!OrderItems.Any())
            {
                MessageBox.Show("Добавьте хотя бы одно изделие в заказ.");
                return;
            }
            var newOrder = new Order
            {
                CreatedOrder = DateTime.Now,
                CustomerId = currentCustomerId,
                Status = "Новый",
                Items = new List<OrderItem>(),
                TotalPrice = OrderItems.Sum(i => i.Total)
            };
            foreach (var item in OrderItems)
            {
                newOrder.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ItemPrice = item.Price
                });
            }
            db.Orders.Add(newOrder);
            db.SaveChanges();
            MessageBox.Show("Заказ успешно оформлен!");
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
    public class OrderItemDisplay
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}