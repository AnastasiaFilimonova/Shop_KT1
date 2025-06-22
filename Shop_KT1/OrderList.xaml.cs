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
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        public ObservableCollection<OrderViewModel> Orders { get; set; }
        public OrderList()
        {
            InitializeComponent();
            Orders = new ObservableCollection<OrderViewModel>();
            DataContext = this;
            LoadOrders();
            OrdersGrid.ItemsSource = Orders;
        }
        private void LoadOrders()
        {
            using (var db = new ApplicationContext())
            {
                var orders = db.Orders
                    .Select(order => new OrderViewModel
                    {
                        OrderId = order.OrderID,
                        CreatedDate = order.CreatedOrder,
                        TotalProductCount = order.Items.Sum(i => i.Quantity),
                        Status = order.Status,
                        CustomerLogin = order.Customer.Login,
                        ManagerLogin = order.Manager != null ? order.Manager.Login : "—"
                    }).ToList();

                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalProductCount { get; set; }
        public string Status { get; set; }
        public string CustomerLogin { get; set; }
        public string ManagerLogin { get; set; }
    }
}