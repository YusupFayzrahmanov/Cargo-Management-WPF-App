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
    class OrdersViewModel:DependencyObject
    {

        RelayCommand deleteCommand;
        localDbContext db;
        ICollectionView _items;
        DateTime _startDate;
        DateTime _endDate;

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
                if (value < StartDate)
                    _endDate = StartDate;
                else
                    _endDate = value;
               
                OnPropertyChanged("EndDate");
            }
        }
        //Коллекция
        public ICollectionView Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        
        //Конструктор
        public OrdersViewModel()
        {
                db = new localDbContext();
                db.Orders.Load();
                Items = CollectionViewSource.GetDefaultView(db.Orders.Local.OrderByDescending(s => s.OrderId));
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
           

        }

        //Применение фильтра для открытых сделок
        public void ItemsOpenOrder()
        {
            Items.Filter += FilterOpenOrder;
        }
        //Фильтр открытых сделок
        private bool FilterOpenOrder(object obj)
        {
            Order current = obj as Order;
            bool result = true;
            if (current.OrderState != "Выполняется")
                result = false;
            return result;
        }
        //Фильтр даты
        private bool FilterDateOrder(object obj)
        {
            Order current = obj as Order;
            bool result = false;
            if (Convert.ToDateTime(current.OrderEndDate) > StartDate && Convert.ToDateTime(current.OrderEndDate) < EndDate)
            {
                result = true;
            }
            return result;
        }
        //Команда фильтрации данных
        private RelayCommand _filterDateCommand;

        public RelayCommand FilterDateCommand
        {
            get
            {
                return _filterDateCommand ??
                    (_filterDateCommand = new RelayCommand((o) =>
                    {
                        Items.Filter += FilterDateOrder;
                    }));
            }
        }
        //Команда завершение заказа
        private RelayCommand _closeOrderCommand;

        public RelayCommand CloseOrderCommand
        {
            get
            {
                return _closeOrderCommand ??
                    (_closeOrderCommand = new RelayCommand((selectedItem) =>
                    {
                        Order _selectedOrder = selectedItem as Order;
                        var truck = db.Trucks.Find(_selectedOrder.TruckId);
                        var order = db.Orders.SingleOrDefault(o => o.OrderId == _selectedOrder.OrderId);
                        truck.TruckState = "Ожидает";
                        order.OrderState = "Завершен";
                        order.OrderEndDate = DateTime.Now.ToString();
                        db.Entry(truck).State = EntityState.Modified;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        Items.Refresh();
                    }));
            }
        }
        //
        //public RelayCommand DeleteCommand
        //{
        //    get
        //    {
        //        return deleteCommand ??
        //          (deleteCommand = new RelayCommand((selectedItem) =>
        //          {
        //              if (selectedItem == null) return;
        //              // получаем выделенный объект
        //              Order _selectedOrder = selectedItem as Order;
        //              db.Orders.Remove(_selectedOrder);
        //              db.SaveChanges();
        //              Items.Refresh();
        //          }));
        //    }
        //}





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

        public OrderWindow OrderWindow
        {
            get => default(OrderWindow);
            set
            {
            }
        }
    }
}
