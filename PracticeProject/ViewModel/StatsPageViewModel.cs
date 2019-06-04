using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace PracticeProject
{
    public class StatsPageViewModel:INotifyPropertyChanged
    {
        
        ObservableCollection<TruckRevenue> _trucks;
        IEnumerable<Order> orders;
        DateTime _startDate;
        DateTime _endDate;
        int _totalRevenue;
        localDbContext db;
        // Коллекция грузовиков
        public ObservableCollection<TruckRevenue> Trucks
        {
            get { return _trucks; }
            set
            {
                _trucks = value;
                OnPropertyChanged("Trucks");
            }
        }
        //Фильтр даты: начало
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        //Фильтр даты: конец
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (EndDate < StartDate)
                    _endDate = StartDate;
                else
                    _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        //Общий доход
        public int TotalRevenue
        {
            get { return _totalRevenue; }
            set
            {
                _totalRevenue = value;
                OnPropertyChanged("TotalRevenue");
            }
        }

        //Конструктор
        public StatsPageViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            db = new localDbContext();
            db.Trucks.Load();
            db.Orders.Load();
            orders = db.Orders.Local.ToBindingList();
            Trucks = new ObservableCollection<TruckRevenue>();
            //Получение статистики по грузовиками за весь период
            foreach (var truck in db.Trucks.Local)
            {
                var orderList = from order in orders
                                where (order.TruckId == truck.TruckId && order.OrderState == "Завершен")
                                select order;
                Trucks.Add(new TruckRevenue { TruckId = truck.TruckId, TruckName = truck.TruckName, TruckModel = truck.TruckModel, TruckTotalRevenues = (int)orderList.Sum(s => s.OrderPrice) });
            }
           

            

            var totalPrice = from order in orders
                             where (order.OrderState == "Завершен")
                             select order;
            //Общая прибыль за все время
            TotalRevenue = (int)totalPrice.Sum(o => o.OrderPrice);


            Trucks = new ObservableCollection<TruckRevenue>(Trucks.OrderByDescending(o => o.TruckTotalRevenues));

            


        }

        //Фильтрация статистики по даходам по указанному периоду
        RelayCommand _showStatsByDateCommand;
        public RelayCommand ShowStatsByDateCommand
        {
            get
            {
                return _showStatsByDateCommand ??
                  (_showStatsByDateCommand = new RelayCommand((o) =>
                  {

                      var ordersByDate = from order in orders
                                     where (order.OrderState == "Завершен" && 
                                     Convert.ToDateTime(order.OrderEndDate) > StartDate && 
                                     Convert.ToDateTime(order.OrderEndDate) < EndDate)
                                     select order;

                      foreach (var truck in Trucks)
                      {
                          var orderList = from order in ordersByDate
                                          where (order.TruckId == truck.TruckId)
                                          select order;
                          truck.TruckTotalRevenues = (int)orderList.Sum(obj => obj.OrderPrice);
                          

                      }
                      Trucks = new ObservableCollection<TruckRevenue>(Trucks.OrderByDescending(s => s.TruckTotalRevenues));
                      TotalRevenue = (int)ordersByDate.Sum(obj => obj.OrderPrice);
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
    //Клаас содержащий информацию о прибыле каждого грузовика
    public class TruckRevenue:INotifyPropertyChanged
    {
        private double _truckTotalRevenues;
        public int TruckId { get; set; }
        public string TruckName { get; set; }
        public string TruckModel { get; set; }
        public string TruckLicState { get; set; }
        public double TruckTotalRevenues
        {
            get { return _truckTotalRevenues; }
            set
            {
                _truckTotalRevenues = value;
                OnPropertyChanged("TruckTotalRevenues");
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
