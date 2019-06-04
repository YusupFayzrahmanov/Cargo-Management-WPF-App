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

namespace PracticeProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {



       

        public MainWindow()
        {
            
            try
            {
                InitializeComponent();

                this.DataContext = new MainViewModel();
                Loaded += MainWindow_Loaded;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + "; " + e.Source, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainViewModel();
        }
    }
}
