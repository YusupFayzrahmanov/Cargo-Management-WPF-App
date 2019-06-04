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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        OrdersViewModel page;

        public OrdersPage()
        {
            InitializeComponent();
            
            DataContext = new OrdersViewModel();
        }

        

        public void OrdersPageLoad()
        {
            DataContext = new OrdersViewModel();
        }

        public void ShowOpenOrders()
        {
            page = new OrdersViewModel();
            DataContext = page;
            page.ItemsOpenOrder();
        }

       
    }
}
