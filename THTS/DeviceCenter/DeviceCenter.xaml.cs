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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace THTS.DeviceCenter
{
    /// <summary>
    /// DeviceCenter.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceCenter
    {
        private ObservableCollection<DataAccess.Device> _deviceList = new ObservableCollection<DataAccess.Device>();
        public ObservableCollection<DataAccess.Device> DeviceList
        {
            get { return _deviceList; }
        }

        private ObservableCollection<DataAccess.Device> _deviceSelectList = new ObservableCollection<DataAccess.Device>();
        public ObservableCollection<DataAccess.Device> DeviceSelectList
        {
            get { return _deviceSelectList; }
        }

        public DeviceCenter(bool select) 
        {
            InitializeComponent();

            DataContext = new DeviceCenterViewModel();

            if (select)
            {
                this.Title = "选择传感器";
                this.WindowState = WindowState.Normal;
                this.btnSelect.Visibility = Visibility.Visible;
            }
            else
            {
                this.btnSelect.Visibility = Visibility.Collapsed;

                //_deviceList = DataAccess.EntityDAO.DeviceDAO.GetAllData();
                //dgDevice.ItemsSource = _deviceList;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.flyoutSearch.IsOpen = !this.flyoutSearch.IsOpen;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DeviceNew newDevice = new DeviceNew();
            bool? result = newDevice.ShowDialog();

            if(result.HasValue && result.Value)
            {
                bool save = DataAccess.EntityDAO.DeviceDAO.Save(newDevice.NewDevice);
                if (save)
                {
                    _deviceList = DataAccess.EntityDAO.DeviceDAO.GetAllData();
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DeviceNew newDevice = new DeviceNew();
            newDevice.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
