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
using System.Data.Entity;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PracticeProject
{
    /// <summary>
    /// Логика взаимодействия для DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        public Driver DriverWind { get; private set; }
        private ObservableCollection<Truck> _trucks;
        private Truck _selectedTruck;

        public Truck SelectedTruck
        {
            get { return _selectedTruck; }
            set { _selectedTruck = value; }
        }

        public ObservableCollection<Truck> Trucks
        {
            get { return _trucks; }
            set { _trucks = value; }
        }

        public DriverWindow(Driver d)
        {
            InitializeComponent();

            using (var db = new localDbContext())
            {
                db.Trucks.Load();
                Trucks = db.Trucks.Local;
            }

            DriverWind = d;
            comboBoxTrucks.ItemsSource = Trucks;
            DataContext = DriverWind;
            comboBoxTrucks.SelectedItem = SelectedTruck;

            Loaded += DriverWindow_Loaded;
        }

        private void DriverWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = DriverWind;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            var _item = comboBoxTrucks.SelectedItem as Truck;
            if (_item != null && DriverWind.DriverName != "")
            {
                DriverWind.TruckId = _item.TruckId;
                this.DialogResult = true;
            }
            else
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

      
    }
}
