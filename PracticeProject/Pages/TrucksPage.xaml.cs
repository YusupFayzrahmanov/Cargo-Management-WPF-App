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
    /// Логика взаимодействия для TrucksPage.xaml
    /// </summary>
    public partial class TrucksPage : Page
    {

        TrucksPageViewModel _modelObject;
        
        
        
        public TrucksPage()
        {
            InitializeComponent();
            DataContext = new TrucksPageViewModel();
        }

        

        public void TrucksPageLoad()
        {
            DataContext = new TrucksPageViewModel();
        }

        public void ShowWaitingTrucks()
        {
            _modelObject = new TrucksPageViewModel();
            DataContext = _modelObject;
            _modelObject.ItemsWaitingTrucks();
        }

        public void ShowFaultTrucks()
        {
            _modelObject = new TrucksPageViewModel();
            DataContext = _modelObject;
            _modelObject.ItemsFaultTrucks();
        }
    }
}
