using Shop_KT1.models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddInventoryItemWindow.xaml
    /// </summary>
    public partial class AddInventoryItemWindow : Window
    {
        public Material SelectedMaterial { get; private set; }
        public AddInventoryItemWindow()
        {
            InitializeComponent();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string inputName = MaterialNameTextBox.Text?.Trim();
            string unit = UnitTextBox.Text?.Trim();
            string priceText = PriceTextBox.Text?.Trim();
            string stockText = StockTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(inputName))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }
            using (var db = new ApplicationContext())
            {
                var existing = db.Materials.FirstOrDefault(m => m.Name.ToLower() == inputName.ToLower());

                if (existing != null)
                {
                    SelectedMaterial = existing;
                }
                else
                {
                    decimal price = 0;
                    decimal stock = 0;
                    decimal.TryParse(priceText, out price);
                    decimal.TryParse(stockText, out stock);
                    var newMaterial = new Material
                    {
                        Name = inputName,
                        Unit = string.IsNullOrWhiteSpace(unit) ? "шт." : unit,
                        Price = price,
                        StockAmount = (double)stock
                    };
                    db.Materials.Add(newMaterial);
                    db.SaveChanges();
                    SelectedMaterial = newMaterial;
                }
            }
            DialogResult = true;
            Close();
        }
    }
}