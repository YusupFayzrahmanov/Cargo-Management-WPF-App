using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace PracticeProject
{
    public class MainViewModel: INotifyPropertyChanged
    {
        
        private TrucksPage Trucks;
        private DriversPage Drivers;
        private OrdersPage Orders;
        private StatsPage Stats;
        private string _waitingTrucksText;
        private string _openOrdersText;
        private string _faultTrucksText;

        // Количество ожидающих грузовиков
        public string WaitingTrucksText
        {
            get { return _waitingTrucksText; }
            set
            {
                _waitingTrucksText = value;
                OnPropertyChanged("WaitingTrucksText");
            }
        }

        // Количество открытых заказов
        public string OpenOrderText
        {
            get { return _openOrdersText; }
            set
            {
                _openOrdersText = value;
                OnPropertyChanged("OpenOrderText");
            }
        }
        // Количество неисправных автомобилей
        public string FaultTrucksText
        {
            get { return _faultTrucksText; }
            set
            {
                _faultTrucksText = value;
                OnPropertyChanged("FaultTrucksText");
            }
        }
       // Подсчет открытых заказов
        private string GetOpenOrders
        {
            get
            {
                using (var db = new localDbContext())
                {
                    db.Orders.Load();
                    var orders = from order in db.Orders.Local
                                 where (order.OrderState == "Выполняется")
                                 select order;
                    return orders.Count().ToString();
                }
            }

            set { }
        }
        // Подсчет ожидающих грузовиков
        private static string GetWaitingTrucks
        {
            get
            {
                using (var db = new localDbContext())
                {
                    db.Trucks.Load();
                    var trucks = from truck in db.Trucks.Local
                                 where (truck.TruckState == "Ожидает")
                                 select truck;
                    return trucks.Count().ToString();
                }
            }

            set { }
        }
        // Подсчет неисправных грузовиков
        private static string GetFaultyTruck
        {
            get
            {
                using (var db = new localDbContext())
                {
                    db.Trucks.Load();
                    var trucks = from truck in db.Trucks.Local
                                 where (truck.TruckCondition == "Неисправен")
                                 select truck;
                    return trucks.Count().ToString();
                }
            }

            set { }
        }



        private Page _currentPage;
        //Текущая страница
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        //Конструктор
        public MainViewModel()
        {

                Trucks = new TrucksPage();
                Drivers = new DriversPage();
                Orders = new OrdersPage();
                Stats = new StatsPage();
                CurrentPage = Trucks;
                WaitingTrucksText = GetWaitingTrucks;
                FaultTrucksText = GetFaultyTruck;
                OpenOrderText = GetOpenOrders;
        }


        //Обновить текст в боковом поле
        private void RefreshText()
        {
            WaitingTrucksText = GetWaitingTrucks;
            FaultTrucksText = GetFaultyTruck;
            OpenOrderText = GetOpenOrders;
        }
        // Команда перехода на страницу с заказами
        private RelayCommand ordersPageCommand;
        public RelayCommand OrdersPageCommand
        {
            get
            {
                return ordersPageCommand ??
                  (ordersPageCommand = new RelayCommand(obj =>
                  {
                       
                      CurrentPage = Orders;
                      Orders.OrdersPageLoad();
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу со статистикой
        private RelayCommand statsPageCommand;
        public RelayCommand StatsPageCommand
        {
            get
            {
                return statsPageCommand ??
                  (statsPageCommand = new RelayCommand(obj =>
                  {

                      CurrentPage = Stats;
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу с грузовиками
        private RelayCommand trucksPageCommand;
        public RelayCommand TrucksPageCommand
        {
            get
            {
                return trucksPageCommand ??
                  (trucksPageCommand = new RelayCommand(obj =>
                  {
                      CurrentPage = Trucks;
                      Trucks.TrucksPageLoad();
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу с ожидающими грузовиками
        private RelayCommand showTruckCommand;
        public RelayCommand ShowTruckCommand
        {
            get
            {
                return showTruckCommand ??
                  (showTruckCommand = new RelayCommand(obj =>
                  {

                      CurrentPage = Trucks;
                      Trucks.ShowWaitingTrucks();
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу с открытыми заказами
        private RelayCommand showOrdersCommand;
        public RelayCommand ShowOrdersCommand
        {
            get
            {
                return showOrdersCommand ??
                  (showOrdersCommand = new RelayCommand(obj =>
                  {

                      CurrentPage = Orders;
                      Orders.ShowOpenOrders();
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу с неисправными грузовиками
        private RelayCommand showFaultTrucksCommand;
        public RelayCommand ShowFaultTrucksCommand
        {
            get
            {
                return showFaultTrucksCommand ??
                  (showFaultTrucksCommand = new RelayCommand(obj =>
                  {

                      CurrentPage = Trucks;
                      Trucks.ShowFaultTrucks();
                      this.RefreshText();
                  }));
            }
        }
        // Команда перехода на страницу с водителями
        private RelayCommand driversPageCommand;
        public RelayCommand DriversPageCommand
        {
            get
            {
                return driversPageCommand ??
                  (driversPageCommand = new RelayCommand(obj =>
                  {
                      CurrentPage = Drivers;
                      this.RefreshText();
                  }));
            }
        }

        

       

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
