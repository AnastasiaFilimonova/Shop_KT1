using Shop_KT1.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        public ObservableCollection<InventoryItemViewModel> Items { get; set; }
        public Inventory()
        {
            InitializeComponent();
            Items = new ObservableCollection<InventoryItemViewModel>();
            DataContext = this;
            LoadData();
        }
        private void LoadData()
        {
            using (var db = new ApplicationContext())
            {
                var materials = db.Materials.ToList();
                Items.Clear();

                foreach (var material in materials)
                {
                    Items.Add(new InventoryItemViewModel
                    {
                        MaterialId = material.MaterialID,
                        Name = material.Name,
                        SystemAmount = material.StockAmount,
                        ActualAmount = material.StockAmount,
                        Price = material.Price
                    });
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                bool hasLargeDeviation = Items.Any(i => i.DeviationPercent > 20);
                bool approved = DirectorApprovedCheckbox.IsChecked ?? false;

                if (hasLargeDeviation && !approved)
                {
                    MessageBox.Show("Есть расхождения более 20%. Требуется утверждение директором.");
                    return;
                }
                var audit = new InventoryAudit
                {
                    AuditDate = DateTime.Now,
                    ApprovedByDirector = approved,
                    Items = Items.Select(vm => new InventoryItem
                    {
                        MaterialID = vm.MaterialId,
                        SystemAmount = vm.SystemAmount,
                        ActualAmount = vm.ActualAmount,
                        DifferenceValue = vm.ActualValue - vm.SystemValue
                    }).ToList()
                };
                foreach (var vm in Items)
                {
                    var material = db.Materials.FirstOrDefault(m => m.MaterialID == vm.MaterialId);
                    if (material != null)
                    {
                        material.StockAmount = vm.ActualAmount;
                    }
                }

                db.InventoryAudits.Add(audit);
                db.SaveChanges();
            }
            MessageBox.Show("Инвентаризация сохранена успешно.");
            Close();
        }
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddInventoryItemWindow();
            if (addWindow.ShowDialog() == true)
            {
                var selectedMaterial = addWindow.SelectedMaterial;
                if (selectedMaterial != null && !Items.Any(i => i.MaterialId == selectedMaterial.MaterialID))
                {
                    Items.Add(new InventoryItemViewModel
                    {
                        MaterialId = selectedMaterial.MaterialID,
                        Name = selectedMaterial.Name,
                        SystemAmount = selectedMaterial.StockAmount,
                        ActualAmount = selectedMaterial.StockAmount,
                        Price = selectedMaterial.Price
                    });
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
    public class InventoryItemViewModel : INotifyPropertyChanged
    {
        private double actualAmount;
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public double SystemAmount { get; set; }
        public double ActualAmount
        {
            get => actualAmount;
            set
            {
                if (actualAmount != value)
                {
                    actualAmount = value;
                    OnPropertyChanged(nameof(ActualAmount));
                    OnPropertyChanged(nameof(ActualValue));
                    OnPropertyChanged(nameof(DeviationPercent));
                    OnPropertyChanged(nameof(DeviationStatus));
                }
            }
        }
        public decimal Price { get; set; }
        public decimal SystemValue => (decimal)SystemAmount * Price;
        public decimal ActualValue => (decimal)ActualAmount * Price;
        public decimal DeviationPercent => SystemValue == 0 ? 0 : Math.Abs(ActualValue - SystemValue) / SystemValue * 100;
        public string DeviationStatus => DeviationPercent > 20 ? "Отклонение!" : "В норме";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}