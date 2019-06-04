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
using System.Data;
using System.Data.Entity;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace PracticeProject
{
    /// <summary>
    /// Логика взаимодействия для DriversPage.xaml
    /// </summary>
    public partial class DriversPage : Page
    {

        public DriversPage()
        {
            InitializeComponent();
           
            Loaded += DriversPage_Loaded;
            DataContext = new DriversViewModel();


        }

      

        private void DriversPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new DriversViewModel();
        }

      
    }
}
