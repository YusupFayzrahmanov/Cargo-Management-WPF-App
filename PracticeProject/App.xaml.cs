using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PracticeProject
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}




//public static string GetOpenOrders
//{
//    get
//    {
//        using (var db = new localDbContext())
//        {
//            db.Orders.Load();
//            var orders = from order in db.Orders.Local
//                         where (order.OrderState == true)
//                         select order;
//            return orders.Count().ToString();
//        }
//    }

//    private set { }
//}

//public string OrderStateMessage
//{
//    get
//    {
//        if (this.OrderState == false)
//            return "Завершен";
//        else
//            return "Выполняется";
//    }
//    private set { }
//}

//namespace PracticeProject
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data.Entity;
//    using System.Linq;
//    using System.ComponentModel;

//    public partial class Truck : INotifyPropertyChanged
//    {
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
//        public Truck()
//        {
//            this.Drivers = new HashSet<Driver>();
//            this.Orders = new HashSet<Order>();
//        }

//        public int TruckId { get; set; }
//        public string TruckName { get; set; }
//        public string TruckModel { get; set; }
//        public string TruckType { get; set; }
//        public Nullable<int> TruckWeightLimit { get; set; }
//        public Nullable<int> TruckMaxSize { get; set; }
//        public Nullable<bool> TruckCondition { get; set; }
//        public Nullable<System.DateTime> TruckYearOfIssue { get; set; }
//        public Nullable<bool> TruckState { get; set; }
//        public string TruckLicState { get; set; }

//        private int _totalTruckRevenue;

//        public Nullable<bool> NullTruck
//        { get; set; }

//        public static string GetWaitingTrucks
//        {
//            get
//            {
//                using (var db = new localDbContext())
//                {
//                    db.Trucks.Load();
//                    var trucks = from truck in db.Trucks.Local
//                                 where (truck.TruckState == false)
//                                 select truck;
//                    return trucks.Count().ToString();
//                }
//            }

//            private set { }
//        }

//        public static string GetFaultyTruck
//        {
//            get
//            {
//                using (var db = new localDbContext())
//                {
//                    db.Trucks.Load();
//                    var trucks = from truck in db.Trucks.Local
//                                 where (truck.TruckCondition == false)
//                                 select truck;
//                    return trucks.Count().ToString();
//                }
//            }

//            private set { }
//        }

//        public string TruckStateMessage
//        {
//            get
//            {
//                if (this.TruckState == false)
//                    return "Ожидает";
//                else
//                    return "Выполняет заказ";
//            }
//            private set { }
//        }

//        public string TruckConditionMessage
//        {
//            get
//            {
//                if (this.TruckCondition == true)
//                    return "Исправен";
//                else
//                    return "Неисправен";
//            }
//            private set { }
//        }


//        public int GetTotalTruckRevenue
//        {
//            get { return _totalTruckRevenue; }
//            set
//            {
//                _totalTruckRevenue = value;
//                OnPropertyChanged("GetTotalTruckRevenue");
//            }
//        }



//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
//        public virtual ICollection<Driver> Drivers { get; set; }
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
//        public virtual ICollection<Order> Orders { get; set; }

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void OnPropertyChanged(PropertyChangedEventArgs e)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null)
//                handler(this, e);
//        }

//        protected void OnPropertyChanged(string propertyName)
//        {
//            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}