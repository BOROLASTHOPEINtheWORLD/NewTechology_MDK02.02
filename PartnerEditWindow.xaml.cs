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

namespace NewTechn
{
    /// <summary>
    /// Логика взаимодействия для PartnerEditWindow.xaml
    /// </summary>
    public partial class PartnerEditWindow : Window
    {
        private Партнеры _partner;
        private bool _isEditMode;

        public PartnerEditWindow()
        {
            InitializeComponent();
            _partner = new Партнеры();
            _isEditMode = false;
            LoadTypes();
            DataContext = _partner;
            Title = "Добавление партнера";
        }

        public PartnerEditWindow(Партнеры partner) : this() 
        {
            _partner = partner;
            _isEditMode = true;
            Title = "Редактирование партнёра";
            DataContext = _partner;
        }

        private void LoadTypes()
        {
            using (var context = new NewTechologyEntities())
            {
                var types = context.ТипыПартнеров.ToList();
                TypeComboBox.ItemsSource = types;

                if (!_isEditMode && types.Any())
                {
                    TypeComboBox.SelectedIndex = 0;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация (оставляем без изменений)
            if (string.IsNullOrWhiteSpace(_partner.Наименование))
            {
                MessageBox.Show("Укажите наименование партнёра.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_partner.Рейтинг < 0)
            {
                MessageBox.Show("Рейтинг должен положительным.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new NewTechologyEntities())
                {
                    if (_isEditMode)
                    {
                        //редактирование партнера
                        var existingPartner = context.Партнеры.Find(_partner.Код_Партнера);
                        if (existingPartner != null)
                        {
                            existingPartner.Код_ТипаПартнера = _partner.Код_ТипаПартнера;
                            existingPartner.Наименование = _partner.Наименование;
                            existingPartner.Директор = _partner.Директор;
                            existingPartner.ЮридическийАдрес = _partner.ЮридическийАдрес;
                            existingPartner.Телефон = _partner.Телефон;
                            existingPartner.ЭлектроннаяПочта = _partner.ЭлектроннаяПочта;
                            existingPartner.Рейтинг = _partner.Рейтинг;
                        }
                    }
                    else
                    {
                        //добавление
                        context.Партнеры.Add(_partner);
                    }

                    context.SaveChanges();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}