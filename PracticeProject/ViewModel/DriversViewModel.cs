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
    class DriversViewModel:INotifyPropertyChanged
    {
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        localDbContext db;
        ICollectionView _items;
        string _filterText;
        // Свойство текста в поле фильтра
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

        // Коллекция элементов таблицы
        public ICollectionView Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }
        // Конструктор
        public DriversViewModel()
        {
                db = new localDbContext();
                db.Drivers.Load();
                Items = CollectionViewSource.GetDefaultView(db.Drivers.Local);
                Items.Filter += FilterDriver;
        }

        // Фильтр
        private bool FilterDriver(object obj)
        {
            bool result = true;
            Driver current = obj as Driver;
            if(!string.IsNullOrWhiteSpace(FilterText) && current != null && !current.DriverName.Contains(FilterText) && !current.DriverSurname.Contains(FilterText) && !current.TruckId.ToString().Contains(FilterText))
            {
                result = false;
            }

            return result;
        }
        // Команда добавления нового водителя
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      DriverWindow _driverWindow = new DriverWindow(new Driver());
                      if (_driverWindow.ShowDialog() == true)
                      {
                          Driver _newDriver = _driverWindow.DriverWind;
                          db.Drivers.Add(_newDriver);
                          db.SaveChanges();

                      }

                      Items.Refresh();
                  }));
            }
        }
        // команда редактирования водителя
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      Driver _selectedDriver = selectedItem as Driver;

                      Driver vm = new Driver()
                      {
                          TruckId = _selectedDriver.TruckId,
                          DriverId = _selectedDriver.DriverId,
                          DriverName = _selectedDriver.DriverName,
                          DriverSurname = _selectedDriver.DriverSurname,
                          DriverDateOfBirth = _selectedDriver.DriverDateOfBirth,
                          DriverDrivingLicense = _selectedDriver.DriverDrivingLicense
                          
                      };
                      DriverWindow _driverWindow = new DriverWindow(vm);


                      if (_driverWindow.ShowDialog() == true)
                      {
                          _selectedDriver = db.Drivers.SingleOrDefault(o => o.DriverId ==_selectedDriver.DriverId);
                          if (_selectedDriver != null)
                          {
                              _selectedDriver.DriverName = _driverWindow.DriverWind.DriverName;
                              _selectedDriver.DriverSurname = _driverWindow.DriverWind.DriverSurname;
                              _selectedDriver.DriverDrivingLicense = _driverWindow.DriverWind.DriverDrivingLicense;
                              _selectedDriver.DriverDateOfBirth = _driverWindow.DriverWind.DriverDateOfBirth;
                              _selectedDriver.TruckId = _driverWindow.DriverWind.TruckId;
                              db.Entry(_selectedDriver).State = EntityState.Modified;
                              db.SaveChanges();
                          }
                      }

                      Items.Refresh();
                  }));
            }
        }
        // команда удаления водителя
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      Driver _selectedDriver = selectedItem as Driver;

                      db.Drivers.Remove(_selectedDriver);
                      db.SaveChanges();
                  }));
            }
        }
        // Событие на изменения текста в поле ввода фильтра
        protected void OnFilterTextChanged(EventArgs e, object obj)
        {
            var current = obj as DriversViewModel;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterDriver;
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
