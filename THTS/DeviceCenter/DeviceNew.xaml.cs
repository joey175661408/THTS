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
    /// DeviceNew.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceNew
    {
        DataAccess.Device _newDevice;
        public DataAccess.Device NewDevice
        {
            get { return _newDevice; }
        }

        public DeviceNew()
        {
            InitializeComponent();

            _newDevice = new DataAccess.Device();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _newDevice.ModuleName = "";
            _newDevice.Manufacture = this.tbManufactory.Text;
            _newDevice.FactoryNo = this.tbFactoryNo.Text;
            _newDevice.CertificateNo = this.tbCertificateNo.Text;
            _newDevice.CalibrateResult = this.rbPass.IsChecked.HasValue && this.rbPass.IsChecked.Value ? 1 : 0;
            _newDevice.CalibrateDate = this.dpCalDate.Text;
            _newDevice.ExpireDate = this.dpExpireDate.Text;
            _newDevice.Remark = this.tbRemark.Text;

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
