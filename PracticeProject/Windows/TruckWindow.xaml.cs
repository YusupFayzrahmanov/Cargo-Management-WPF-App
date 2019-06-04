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

namespace PracticeProject
{
    /// <summary>
    /// Логика взаимодействия для TruckWindow.xaml
    /// </summary>
    public partial class TruckWindow : Window
    {
        public Truck TruckWind { get; private set; }

        
        public TruckWindow(Truck t)
        {
            InitializeComponent();
            TruckWind = t;
            this.DataContext = TruckWind;
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
        }

        private void ComboBoxType_MouseLeave(object sender, MouseEventArgs e)
        {
            TruckWind.TruckType = ComboBoxType.Text;

        }
        
    }
}
