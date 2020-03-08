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
using THTS.DataAccess;
using THTS.DataAccess.EntityDAO;

namespace THTS.DeviceCenter
{
    /// <summary>
    /// DeviceNew.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceNew
    {
        private bool isEdit = false;
        public Device NewDevice = new Device();

        public DeviceNew()
        {
            InitializeComponent();

            Device tempDevice = DeviceDAO.GetLastDevice();
            if (tempDevice != null)
            {
                NewDevice = tempDevice;
                this.tbModule.Text = NewDevice.ModuleName;
                this.tbManufactory.Text = NewDevice.Manufacture;
                this.tbFactoryNo.Text = NewDevice.FactoryNo;
                this.tbCertificateNo.Text = NewDevice.CertificateNo;
                this.rbPass.IsChecked = NewDevice.CalibrateResult == 1;
                this.rbFail.IsChecked = NewDevice.CalibrateResult != 1;
                this.dpCalDate.Text = NewDevice.CalibrateDate;
                this.dpExpireDate.Text = NewDevice.ExpireDate;
                this.tbRemark.Text = NewDevice.Remark;
            }
        }

        public DeviceNew(Device editDevice)
        {
            InitializeComponent();

            isEdit = true;
            NewDevice = editDevice;
            this.tbID.IsEnabled = false;
            this.tbID.Text = NewDevice.SensorId;
            this.tbModule.Text = NewDevice.ModuleName;
            this.tbManufactory.Text = NewDevice.Manufacture;
            this.tbFactoryNo.Text = NewDevice.FactoryNo;
            this.tbCertificateNo.Text = NewDevice.CertificateNo;
            this.rbPass.IsChecked = NewDevice.CalibrateResult == 1;
            this.rbFail.IsChecked = NewDevice.CalibrateResult != 1;
            this.dpCalDate.Text = NewDevice.CalibrateDate;
            this.dpExpireDate.Text = NewDevice.ExpireDate;
            this.tbRemark.Text = NewDevice.Remark;


            if (this.tbModule.SelectedItem.ToString().Contains("PT100"))
            {
                this.cbP.IsChecked = NewDevice.Extra4.Equals("True") ? true : false;
                this.txP1.Text = NewDevice.Extra1;
                this.txP2.Text = NewDevice.Extra2;
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K"))
            {
                this.cbK.IsChecked = NewDevice.Extra4.Equals("True") ? true : false;
                this.txK1.Text = NewDevice.Extra1;
                this.txK2.Text = NewDevice.Extra2;
                this.txK3.Text = NewDevice.Extra3;
            }
            else
            {
                this.cbH.IsChecked = NewDevice.Extra4.Equals("True") ? true : false;
                this.txH1.Text = NewDevice.Extra1;
                this.txH2.Text = NewDevice.Extra2;
                this.txH3.Text = NewDevice.Extra3;
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!isEdit)
            {
                string sensorID = this.tbID.Text;

                if (sensorID == "")
                {
                    MessageBox.Show("传感器序号不得为空！");
                    return;
                }

                if (DeviceDAO.IsExist(this.tbID.Text))
                {
                    MessageBox.Show("传感器序号已存在！");
                    return;
                }
            }

            NewDevice.SensorId = this.tbID.Text;
            NewDevice.ModuleName = this.tbModule.Text;
            NewDevice.Manufacture = this.tbManufactory.Text;
            NewDevice.FactoryNo = this.tbFactoryNo.Text;
            NewDevice.CertificateNo = this.tbCertificateNo.Text;
            NewDevice.CalibrateResult = this.rbPass.IsChecked.HasValue && this.rbPass.IsChecked.Value ? 1 : 0;
            NewDevice.CalibrateDate = this.dpCalDate.Text;
            NewDevice.ExpireDate = this.dpExpireDate.Text;
            NewDevice.Remark = this.tbRemark.Text;

            if (this.tbModule.SelectedItem.ToString().Contains("PT100"))
            {
                NewDevice.Extra4 = this.cbP.IsChecked.ToString();
                NewDevice.Extra1 = this.txP1.Text;
                NewDevice.Extra2 = this.txP2.Text;
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K"))
            {
                NewDevice.Extra4 = this.cbK.IsChecked.ToString();
                NewDevice.Extra1 = this.txK1.Text;
                NewDevice.Extra2 = this.txK2.Text;
                NewDevice.Extra3 = this.txK3.Text;
            }
            else
            {
                NewDevice.Extra4 = this.cbH.IsChecked.ToString();
                NewDevice.Extra1 = this.txH1.Text;
                NewDevice.Extra2 = this.txH2.Text;
                NewDevice.Extra3 = this.txH3.Text;
            }

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void tbModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tbModule.SelectedItem.ToString().Contains("PT100"))
            {
                this.spP.Visibility = Visibility.Visible;
                this.spH.Visibility = Visibility.Collapsed;
                this.spK.Visibility = Visibility.Collapsed;
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K"))
            {
                this.spP.Visibility = Visibility.Collapsed;
                this.spH.Visibility = Visibility.Collapsed;
                this.spK.Visibility = Visibility.Visible;
            }
            else
            {
                this.spP.Visibility = Visibility.Collapsed;
                this.spH.Visibility = Visibility.Visible;
                this.spK.Visibility = Visibility.Collapsed;
            }
        }
    }
}
