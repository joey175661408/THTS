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

namespace THTS.DeviceCenter
{
    /// <summary>
    /// DeviceCenter.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceCenter
    {
        public List<DataAccess.Device> deviceSelectList = null;

        public DeviceCenter(bool select) 
        {
            InitializeComponent();

            if (select)
            {
                this.Title = "选择传感器";
                this.WindowState = WindowState.Normal;
                this.btnSelect.Visibility = Visibility.Visible;
                deviceSelectList = new List<DataAccess.Device>();
            }
            else
            {
                this.btnSelect.Visibility = Visibility.Collapsed;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.flyoutSearch.IsOpen = !this.flyoutSearch.IsOpen;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DeviceNew newDevice = new DeviceNew();
            newDevice.ShowDialog();
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
