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
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private readonly ApplicationContext db = new();
        public Report()
        {
            InitializeComponent();
            LoadMaterials();
        }
        private void LoadMaterials()
        {
            var allMaterials = db.Materials.ToList();
            var allProducts = db.Products.ToList();
            var combinedList = allMaterials
                .Select(m => new ReportItem { Id = m.MaterialID, Name = m.Name, IsProduct = false })
                .Concat(allProducts.Select(p => new ReportItem { Id = p.ProductID, Name = p.Name, IsProduct = true }))
                .ToList();

            MaterialListBox.ItemsSource = combinedList;
        }
        private void ShowStockReport_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = MaterialListBox.SelectedItems.Cast<ReportItem>().ToList();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Выберите материалы или изделия.");
                return;
            }
            var report = new List<StockRow>();

        foreach (var item in selectedItems)
            {
                if (item.IsProduct)
                {
                    var product = db.Products
                    .Where(p => p.ProductID == item.Id)
                    .Select(p => new
                    {
                        p.Name,
                        p.ProductID,
                        Materials = p.Materials.Select(pm => new
                        {
                            MaterialPrice = pm.Material.Price,
                            QuantityPerUnit = pm.Quantity
                        }).ToList()
                    })
                .FirstOrDefault();
            if (product != null)
            {
                double stock = db.Productions
                    .Where(pr => pr.ProductId == product.ProductID)
                    .Sum(pr => pr.Quantity);
                decimal unitCost = product.Materials
                    .Sum(m => (decimal)m.MaterialPrice * (decimal)m.QuantityPerUnit);
                decimal totalCost = unitCost * (decimal)stock;
                report.Add(new StockRow
                {
                    Name = product.Name,
                    Amount = stock,
                    Price = totalCost
                });
            }
        }
        else
        {
            var material = db.Materials.FirstOrDefault(m => m.MaterialID == item.Id);
            if (material != null)
            {
                report.Add(new StockRow
                {
                    Name = material.Name,
                    Amount = material.StockAmount,
                    Price = (decimal)material.StockAmount * material.Price
                });
            }
        }
    }

    StockReportGrid.ItemsSource = report;
}
        private int GetProductStock(int productId)
        {
            return (int)db.Productions
                .Where(p => p.ProductId == productId)
                .Sum(p => p.Quantity);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();
                doc.Blocks.Add(new Paragraph(new Run("Отчёт по остаткам")));
                var table = new Table();
                table.Columns.Add(new TableColumn());
                table.Columns.Add(new TableColumn());
                table.Columns.Add(new TableColumn());
                var header = new TableRow();
                header.Cells.Add(new TableCell(new Paragraph(new Run("Наименование"))));
                header.Cells.Add(new TableCell(new Paragraph(new Run("Остаток (шт.)"))));
                header.Cells.Add(new TableCell(new Paragraph(new Run("Стоимость (руб.)"))));
                var rowGroup = new TableRowGroup();
                rowGroup.Rows.Add(header);
                foreach (StockRow row in StockReportGrid.ItemsSource)
                {
                    var dataRow = new TableRow();
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(row.Name))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(row.Amount.ToString()))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run($"{row.Price:C}"))));
                    rowGroup.Rows.Add(dataRow);
                }
                table.RowGroups.Add(rowGroup);
                doc.Blocks.Add(table);
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Отчёт");
            }
        }
        public class ReportItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsProduct { get; set; }
        }
        public class StockRow
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public decimal Price { get; set; }
        }
    }
}
    