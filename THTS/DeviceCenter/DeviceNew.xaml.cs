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
                tempDevice.RealValue1 = string.Empty;
                tempDevice.RealValue2 = string.Empty;
                tempDevice.RealValue3 = string.Empty;
                tempDevice.ReferenceValue1 = string.Empty;
                tempDevice.ReferenceValue2 = string.Empty;
                tempDevice.ReferenceValue3 = string.Empty;
                tempDevice.ToleranceValue1 = string.Empty;
                tempDevice.ToleranceValue2 = string.Empty;
                tempDevice.ToleranceValue3 = string.Empty;
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
            ShowCalculateResult();

        }

        private void ShowCalculateResult()
        {
            if (this.tbModule.SelectedItem.ToString().Contains("PT100"))
            {
                this.cbP.IsChecked = string.IsNullOrEmpty(NewDevice.IsCalculate) ? false : NewDevice.IsCalculate.Equals("True") ? true : false;
                this.txPR1.Text = NewDevice.ReferenceValue1;
                this.txPP1.Text = NewDevice.RealValue1;
                this.txPT1.Text = NewDevice.ToleranceValue1;
                this.txPR2.Text = NewDevice.ReferenceValue2;
                this.txPP2.Text = NewDevice.RealValue2;
                this.txPT2.Text = NewDevice.ToleranceValue2;
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K"))
            {
                this.cbK.IsChecked = string.IsNullOrEmpty(NewDevice.IsCalculate) ? false : NewDevice.IsCalculate.Equals("True") ? true : false;
                this.txKR1.Text = NewDevice.ReferenceValue1;
                this.txKP1.Text = NewDevice.RealValue1;
                this.txKT1.Text = NewDevice.ToleranceValue1;
                this.txKR2.Text = NewDevice.ReferenceValue2;
                this.txKP2.Text = NewDevice.RealValue2;
                this.txKT2.Text = NewDevice.ToleranceValue2;
                this.txKR3.Text = NewDevice.ReferenceValue3;
                this.txKP3.Text = NewDevice.RealValue3;
                this.txKT3.Text = NewDevice.ToleranceValue3;
            }
            else
            {
                this.cbH.IsChecked = string.IsNullOrEmpty(NewDevice.IsCalculate) ? false : NewDevice.IsCalculate.Equals("True") ? true : false;
                this.txHR1.Text = NewDevice.ReferenceValue1;
                this.txHP1.Text = NewDevice.RealValue1;
                this.txHT1.Text = NewDevice.ToleranceValue1;
                this.txHR2.Text = NewDevice.ReferenceValue2;
                this.txHP2.Text = NewDevice.RealValue2;
                this.txHT2.Text = NewDevice.ToleranceValue2;
                this.txHR3.Text = NewDevice.ReferenceValue3;
                this.txHP3.Text = NewDevice.RealValue3;
                this.txHT3.Text = NewDevice.ToleranceValue3;
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
                NewDevice.IsCalculate = this.cbP.IsChecked.ToString();
                NewDevice.ReferenceValue1 = this.txPR1.Text;
                NewDevice.RealValue1 = this.txPP1.Text;
                NewDevice.ToleranceValue1 = this.txPT1.Text;
                NewDevice.ReferenceValue2 = this.txPR2.Text;
                NewDevice.RealValue2 = this.txPP2.Text;
                NewDevice.ToleranceValue2 = this.txPT2.Text;
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K"))
            {
                NewDevice.IsCalculate = this.cbK.IsChecked.ToString();
                NewDevice.ReferenceValue1 = this.txKR1.Text;
                NewDevice.RealValue1 = this.txKP1.Text;
                NewDevice.ToleranceValue1 = this.txKT1.Text;
                NewDevice.ReferenceValue2 = this.txKR2.Text;
                NewDevice.RealValue2 = this.txKP2.Text;
                NewDevice.ToleranceValue2 = this.txKT2.Text;
                NewDevice.ReferenceValue3 = this.txKR3.Text;
                NewDevice.RealValue3 = this.txKP3.Text;
                NewDevice.ToleranceValue3 = this.txKT3.Text;
            }
            else
            {
                NewDevice.IsCalculate = this.cbH.IsChecked.ToString();
                NewDevice.ReferenceValue1 = this.txHR1.Text;
                NewDevice.RealValue1 = this.txHP1.Text;
                NewDevice.ToleranceValue1 = this.txHT1.Text;
                NewDevice.ReferenceValue2 = this.txHR2.Text;
                NewDevice.RealValue2 = this.txHP2.Text;
                NewDevice.ToleranceValue2 = this.txHT2.Text;
                NewDevice.ReferenceValue3 = this.txHR3.Text;
                NewDevice.RealValue3 = this.txHP3.Text;
                NewDevice.ToleranceValue3 = this.txHT3.Text;
            }

            NewDevice.Calcute();

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

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            float Rvalue1, Rvalue2, Rvalue3, Revalue1, Revalue2, Revalue3;

            if (this.tbModule.SelectedItem.ToString().Contains("PT100")&& this.cbP.IsChecked==true)
            {
                if (float.TryParse(this.txPR1.Text, out Rvalue1) && float.TryParse(this.txPP1.Text, out Revalue1))
                {
                    this.txPT1.Text = (Rvalue1 - Revalue1).ToString("F2");
                }

                if (float.TryParse(this.txPR2.Text, out Rvalue2) && float.TryParse(this.txPP2.Text, out Revalue2))
                {
                    this.txPT2.Text = (Rvalue2 - Revalue2).ToString("F2");
                }
            }
            else if (this.tbModule.SelectedItem.ToString().Contains("K") && this.cbK.IsChecked == true)
            {
                if (float.TryParse(this.txKR1.Text, out Rvalue1) && float.TryParse(this.txKP1.Text, out Revalue1))
                {
                    this.txKT1.Text = (Rvalue1 - Revalue1).ToString("F2");
                }

                if (float.TryParse(this.txKR2.Text, out Rvalue2) && float.TryParse(this.txKP2.Text, out Revalue2))
                {
                    this.txKT2.Text = (Rvalue2 - Revalue2).ToString("F2");
                }

                if (float.TryParse(this.txKR3.Text, out Rvalue3) && float.TryParse(this.txKP3.Text, out Revalue3))
                {
                    this.txKT3.Text = (Rvalue3 - Revalue3).ToString("F2");
                }
            }
            else if (this.cbH.IsChecked == true)
            {
                if (float.TryParse(this.txHR1.Text, out Rvalue1) && float.TryParse(this.txHP1.Text, out Revalue1))
                {
                    this.txHT1.Text = (Rvalue1 - Revalue1).ToString("F2");
                }

                if (float.TryParse(this.txHR2.Text, out Rvalue2) && float.TryParse(this.txHP2.Text, out Revalue2))
                {
                    this.txHT2.Text = (Rvalue2 - Revalue2).ToString("F2");
                }

                if (float.TryParse(this.txHR3.Text, out Rvalue3) && float.TryParse(this.txHP3.Text, out Revalue3))
                {
                    this.txHT3.Text = (Rvalue3 - Revalue3).ToString("F2");
                }
            }

        }
    }
}
