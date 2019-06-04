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
    public class TrucksPageViewModel:DependencyObject, INotifyPropertyChanged
    {

        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        RelayCommand addNewOrderCommand;
        ICollectionView _items;
        string _filterText;
        localDbContext db;
        //Текст поля фильтра
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged("FilterText");
                OnFilterTextChanged(EventArgs.Empty, this);

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
        public TrucksPageViewModel()
        {

                db = new localDbContext();
                db.Trucks.Load();
                Items = CollectionViewSource.GetDefaultView(db.Trucks.Local);
                Items.Filter += FilterTruck;

           
            
        }

       //Фильтр
        private bool FilterTruck(object obj)
        {
            bool result = true;
            Truck current = obj as Truck;
            if (!string.IsNullOrWhiteSpace(FilterText) && current != null && !current.TruckName.Contains(FilterText) && !current.TruckModel.Contains(FilterText) && !current.TruckId.ToString().Contains(FilterText) && !current.TruckState.ToString().Contains(FilterText))
            {
                result = false;
            }

            return result;
        }
        //Применения фильтра для ожидающих грузовиков
        public void ItemsWaitingTrucks()
        {
            Items.Filter += WaitingTrucks;
        }
        //Фильтр для несиправных грузовиков
        public void ItemsFaultTrucks()
        {
            Items.Filter += FaultTrucks;
        }
        //Фильтр несправных грузовиков
        private bool FaultTrucks(object obj)
        {
            bool result = true;
            Truck current = obj as Truck;
            if (current.TruckCondition != "Неисправен")
                result = false;
            return result;
        }
        //Фильтр ожидающих
        private bool WaitingTrucks(object obj)
        {
            bool result = true;
            Truck current = obj as Truck;
            if (current.TruckState !="Ожидает")
                result = false;
            return result;
        }
        //Команда изменения роботоспособности грузовика
        private RelayCommand _changeTruckCondition;
        public RelayCommand ChangeTruckCondition
        {
            get
            {
                return _changeTruckCondition ??
                  (_changeTruckCondition = new RelayCommand((selectedItem) =>
                  {
                      Truck _selectedTruck = selectedItem as Truck;
                      if (selectedItem == null || _selectedTruck.TruckState == "Выполняет заказ") return;
                      // получаем выделенный объект
                      _selectedTruck.TruckCondition = _selectedTruck.TruckCondition == "Исправен" ? "Неисправен":"Исправен";
                      db.Entry(_selectedTruck).State = EntityState.Modified;
                      db.SaveChanges();
                      Items.Refresh();
                  }));
            }
        }
        //Команда добавления нового грузовика
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      TruckWindow truckWindow = new TruckWindow(new Truck());
                      if (truckWindow.ShowDialog() == true)
                      {
                          Truck newTruck = truckWindow.TruckWind;
                          newTruck.TruckState = "Ожидает";
                          newTruck.TruckCondition = "Исправен";
                          db.Trucks.Add(newTruck);
                          db.SaveChanges();
                          Items.Refresh();
                      }
                  }));
            }
        }
        //Команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      
                      Truck _selectedTruck = selectedItem as Truck;
                      if (selectedItem == null || _selectedTruck.TruckState == "Выполняет заказ") return;
                      // получаем выделенный объект

                      db.Trucks.Remove(_selectedTruck);
                      db.SaveChanges();
                      Items.Refresh();
                  }));
            }
        }
        //Добавления нового заказа для выбранного грузовика
        public RelayCommand AddNewOrderCommand
        {
            get
            {
                return addNewOrderCommand ??
                  (addNewOrderCommand = new RelayCommand((selectedItem) =>
                  {
                      var _selectedTruck = selectedItem as Truck;
                      if (_selectedTruck.TruckState == "Выполняет заказ" || _selectedTruck.TruckCondition == "Неисправен") return;

                      OrderWindow orderWindow = new OrderWindow(new Order());
                      if (orderWindow.ShowDialog() == true)
                      {
                          Order newOrder = orderWindow.OrderWind;
                          _selectedTruck.TruckState = "Выполняет заказ";
                          db.Entry(_selectedTruck).State = EntityState.Modified;
                          newOrder.OrderStartDate = DateTime.Now.ToString();
                          newOrder.OrderState = "Выполняется";
                          newOrder.TruckId = _selectedTruck.TruckId;
                          db.Orders.Add(newOrder);
                          db.SaveChanges();
                          
                      }

                      Items.Refresh();


                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      Truck _selectedTruck = selectedItem as Truck;

                      Truck vm = new Truck()
                      {
                          TruckId = _selectedTruck.TruckId,
                          TruckName = _selectedTruck.TruckName,
                          TruckModel = _selectedTruck.TruckModel,
                          TruckType = _selectedTruck.TruckType,
                          TruckLicState = _selectedTruck.TruckLicState,
                          TruckMaxSize = _selectedTruck.TruckMaxSize,
                          TruckWeightLimit = _selectedTruck.TruckWeightLimit,
                          TruckCondition = _selectedTruck.TruckCondition,
                          TruckState = _selectedTruck.TruckState,
                          TruckYearOfIssue = _selectedTruck.TruckYearOfIssue,

                      };
                      TruckWindow truckWindow = new TruckWindow(vm);


                      if (truckWindow.ShowDialog() == true)
                      {
                          _selectedTruck = db.Trucks.Find(truckWindow.TruckWind.TruckId);
                          if (_selectedTruck != null)
                          {
                              _selectedTruck.TruckName = truckWindow.TruckWind.TruckName;
                              _selectedTruck.TruckModel = truckWindow.TruckWind.TruckModel;
                              _selectedTruck.TruckType = truckWindow.TruckWind.TruckType;
                              _selectedTruck.TruckYearOfIssue = truckWindow.TruckWind.TruckYearOfIssue;
                              _selectedTruck.TruckWeightLimit = truckWindow.TruckWind.TruckWeightLimit;
                              _selectedTruck.TruckMaxSize = truckWindow.TruckWind.TruckMaxSize;
                              _selectedTruck.TruckLicState = truckWindow.TruckWind.TruckLicState;
                              db.Entry(_selectedTruck).State = EntityState.Modified;
                              db.SaveChanges();
                          }
                         
                      }
                      Items.Refresh();
                  }));
            }
        }
        
        //Событие на изменение текста в поле фильтра
        protected void OnFilterTextChanged(EventArgs e, object obj)
        {
            var current = obj as TrucksPageViewModel;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterTruck;
            }
            EventHandler handler = FilterTextChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler FilterTextChanged;

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
