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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace NewTechn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                using (var context = new NewTechologyEntities())
                {
                    var requsts =  context.ЗаявкиПартнеров
                        .Include("Партнеры.ТипыПартнеров")
                        .Include("СоставЗаявки.Продукция")
                        .Include("Партнеры")

                        .ToList();

                    RequestListView.ItemsSource = requsts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
     
        }

        private void AddPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new PartnerEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                LoadData(); 
            }
        }

        private void RequestListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestListView.SelectedItem is ЗаявкиПартнеров req)
            {
                var editWindow = new PartnerEditWindow(req.Партнеры);
                if (editWindow.ShowDialog() == true)
                {   

                    LoadData(); 
                }
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            int result = CalculateMaterial(
                        productTypeId: 1,
                        materialTypeId: 1,
                        requiredProducts: 1000,
                        productsInStock: 40,
                        param1: 2.0,
                        param2: 1.0);
            MessageBox.Show($"Результат: {result}");


        }
        public static int CalculateMaterial(
                int productTypeId,      // идентификатор типа продукции
                int materialTypeId,     // идентификатор типа материала
                int requiredProducts,   // требуемое количество продукции
                int productsInStock,    // количество продукции на складе
                double param1,          
                double param2           
            )
        {
            if (requiredProducts < 0 || productsInStock < 0 || param1 <= 0 || param2 <= 0)
                return -1;
            try
            {
                using(var context = new NewTechologyEntities())
                {
                    var productType = context.ТипыПродукции.FirstOrDefault(t => t.Код_ТипаПродукции == productTypeId);
                    var materialType = context.ТипыМатериалов.FirstOrDefault(t => t.Код_ТипаМатериала == materialTypeId);
                    if (productType == null || materialType == null) //если не найдены
                        return -1;

                    int toProduce = Math.Max(0, requiredProducts - productsInStock); // сколько нужно произвести
                    double materialPerUnit = param1 * param2 * (double)productType.КоэффициентТипаПродукции; // материал на 1 штуку
                    double totalMaterial = toProduce * materialPerUnit;
  
                    double totalWithDefect = totalMaterial / (1.0 - (double)materialType.ПроцентБракаМатериала);

                    return (int)Math.Ceiling(totalWithDefect);
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}
