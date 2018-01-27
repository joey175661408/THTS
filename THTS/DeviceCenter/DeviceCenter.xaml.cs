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
        public DeviceCenter(bool select) 
        {
            InitializeComponent();

            DataContext = new DeviceCenterViewModel();

            this.btnSelect.Visibility = select ? Visibility.Visible : Visibility.Collapsed;
            this.dgDevice.SelectionMode = DataGridSelectionMode.Extended;
        }


        private ObservableCollection<DataAccess.Device> _deviceSelectList = new ObservableCollection<DataAccess.Device>();
        /// <summary>
        /// 当前选中的传感器列表
        /// </summary>
        public ObservableCollection<DataAccess.Device> DeviceSelectList
        {
            get { return _deviceSelectList; }
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < this.dgDevice.SelectedItems.Count; i++)
            {
                _deviceSelectList.Add((DataAccess.Device)this.dgDevice.SelectedItems[i]);
            }
            this.DialogResult = true;
        }
    }
}
