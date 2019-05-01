using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.MVVM;

namespace THTS.TestCenter
{
    public class TestCenterMultiViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性
        private DataAccess.TestInfo _info = new DataAccess.TestInfo();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info
        {
            get { return _info; }
            set { _info = value; OnPropertyChanged(); }
        }

        private DataAccess.TestInfo _info1 = new DataAccess.TestInfo();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info1
        {
            get { return _info1; }
            set { _info1 = value; OnPropertyChanged(); }
        }

        private DataAccess.TestInfo _info2 = new DataAccess.TestInfo();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info2
        {
            get { return _info2; }
            set { _info2 = value; OnPropertyChanged(); }
        }

        private DataAccess.TestInfo _info3 = new DataAccess.TestInfo();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info3
        {
            get { return _info3; }
            set { _info3 = value; OnPropertyChanged(); }
        }

        private DataAccess.TestInfo _info4 = new DataAccess.TestInfo();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info4
        {
            get { return _info4; }
            set { _info4 = value; OnPropertyChanged(); }
        }
        #endregion

        public TestCenterMultiViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);

            Info.Company = MultiFileHelper.IniReadValue("Info", "Company");

            Info.EnvironmentTemperature = MultiFileHelper.IniReadValue("Info", "EnvironmentTemperature");
            Info.EnvironmentPressure = MultiFileHelper.IniReadValue("Info", "EnvironmentPressure");
            Info.EnvironmentHumidity = MultiFileHelper.IniReadValue("Info", "EnvironmentHumidity");

            Info.VerifiedBy = MultiFileHelper.IniReadValue("Info", "VerifiedBy");
            Info.CheckedBy = MultiFileHelper.IniReadValue("Info", "CheckedBy");
            Info.TestDate = DateTime.Now.ToString("yyyy-MM-dd");

            Info.TempAverageIsChecked = true;
            Info.TempDepartureIsChecked = true;

            string temp = DateTime.Now.ToString("yyyyMMddHHmmss");

            Info1.UutName = MultiFileHelper.IniReadValue("Info1", "UutName");
            Info1.UutModule = MultiFileHelper.IniReadValue("Info1", "UutModule");
            Info1.UutSN = MultiFileHelper.IniReadValue("Info1", "UutSN");
            Info1.UutManufacture = MultiFileHelper.IniReadValue("Info1", "UutManufacture");
            Info1.Accuracy = MultiFileHelper.IniReadValue("Info1", "Accuracy");
            Info1.TemperatureLower = MultiFileHelper.IniReadValue("Info1", "TemperatureLower");
            Info1.TemperatureUpper = MultiFileHelper.IniReadValue("Info1", "TemperatureUpper");
            Info1.RecordSN = "R" + temp + "1";
            Info1.CertificateSN = "C" + temp + "1";

            Info2.UutName = MultiFileHelper.IniReadValue("Info2", "UutName");
            Info2.UutModule = MultiFileHelper.IniReadValue("Info2", "UutModule");
            Info2.UutSN = MultiFileHelper.IniReadValue("Info2", "UutSN");
            Info2.UutManufacture = MultiFileHelper.IniReadValue("Info2", "UutManufacture");
            Info2.Accuracy = MultiFileHelper.IniReadValue("Info2", "Accuracy");
            Info2.TemperatureLower = MultiFileHelper.IniReadValue("Info2", "TemperatureLower");
            Info2.TemperatureUpper = MultiFileHelper.IniReadValue("Info2", "TemperatureUpper");
            Info2.RecordSN = "R" + temp + "2";
            Info2.CertificateSN = "C" + temp + "2";

            Info3.UutName = MultiFileHelper.IniReadValue("Info3", "UutName");
            Info3.UutModule = MultiFileHelper.IniReadValue("Info3", "UutModule");
            Info3.UutSN = MultiFileHelper.IniReadValue("Info3", "UutSN");
            Info3.UutManufacture = MultiFileHelper.IniReadValue("Info3", "UutManufacture");
            Info3.Accuracy = MultiFileHelper.IniReadValue("Info3", "Accuracy");
            Info3.TemperatureLower = MultiFileHelper.IniReadValue("Info3", "TemperatureLower");
            Info3.TemperatureUpper = MultiFileHelper.IniReadValue("Info3", "TemperatureUpper");
            Info3.RecordSN = "R" + temp + "3";
            Info3.CertificateSN = "C" + temp + "3";

            Info4.UutName = MultiFileHelper.IniReadValue("Info4", "UutName");
            Info4.UutModule = MultiFileHelper.IniReadValue("Info4", "UutModule");
            Info4.UutSN = MultiFileHelper.IniReadValue("Info4", "UutSN");
            Info4.UutManufacture = MultiFileHelper.IniReadValue("Info4", "UutManufacture");
            Info4.Accuracy = MultiFileHelper.IniReadValue("Info4", "Accuracy");
            Info4.TemperatureLower = MultiFileHelper.IniReadValue("Info4", "TemperatureLower");
            Info4.TemperatureUpper = MultiFileHelper.IniReadValue("Info4", "TemperatureUpper");
            Info4.RecordSN = "R" + temp + "4";
            Info4.CertificateSN = "C" + temp + "4";
        }

        #region 方法
        /// <summary>
        /// 开始测试方法
        /// </summary>
        private void Start()
        {
            if (DataAccess.EntityDAO.TestInfoDAO.IsExit(Info.RecordSN))
            {
                System.Windows.MessageBox.Show("此记录编号已存在！");
                return;
            }

            MultiFileHelper.IniWriteValue("Info", "Company", Info.Company);
            MultiFileHelper.IniWriteValue("Info", "EnvironmentTemperature", Info.EnvironmentTemperature);
            MultiFileHelper.IniWriteValue("Info", "EnvironmentPressure", Info.EnvironmentPressure);
            MultiFileHelper.IniWriteValue("Info", "EnvironmentHumidity", Info.EnvironmentHumidity);
            MultiFileHelper.IniWriteValue("Info", "VerifiedBy", Info.VerifiedBy);
            MultiFileHelper.IniWriteValue("Info", "CheckedBy", Info.CheckedBy);

            MultiFileHelper.IniWriteValue("Info1", "UutName", Info1.UutName);
            MultiFileHelper.IniWriteValue("Info1", "UutModule", Info1.UutModule);
            MultiFileHelper.IniWriteValue("Info1", "UutSN", Info1.UutSN);
            MultiFileHelper.IniWriteValue("Info1", "UutManufacture", Info1.UutManufacture);
            MultiFileHelper.IniWriteValue("Info1", "Accuracy", Info1.Accuracy);
            MultiFileHelper.IniWriteValue("Info1", "TemperatureLower", Info1.TemperatureLower);
            MultiFileHelper.IniWriteValue("Info1", "TemperatureUpper", Info1.TemperatureUpper);

            MultiFileHelper.IniWriteValue("Info2", "UutName", Info2.UutName);
            MultiFileHelper.IniWriteValue("Info2", "UutModule", Info2.UutModule);
            MultiFileHelper.IniWriteValue("Info2", "UutSN", Info2.UutSN);
            MultiFileHelper.IniWriteValue("Info2", "UutManufacture", Info2.UutManufacture);
            MultiFileHelper.IniWriteValue("Info2", "Accuracy", Info2.Accuracy);
            MultiFileHelper.IniWriteValue("Info2", "TemperatureLower", Info2.TemperatureLower);
            MultiFileHelper.IniWriteValue("Info2", "TemperatureUpper", Info2.TemperatureUpper);

            MultiFileHelper.IniWriteValue("Info3", "UutName", Info3.UutName);
            MultiFileHelper.IniWriteValue("Info3", "UutModule", Info3.UutModule);
            MultiFileHelper.IniWriteValue("Info3", "UutSN", Info3.UutSN);
            MultiFileHelper.IniWriteValue("Info3", "UutManufacture", Info3.UutManufacture);
            MultiFileHelper.IniWriteValue("Info3", "Accuracy", Info3.Accuracy);
            MultiFileHelper.IniWriteValue("Info3", "TemperatureLower", Info3.TemperatureLower);
            MultiFileHelper.IniWriteValue("Info3", "TemperatureUpper", Info3.TemperatureUpper);

            MultiFileHelper.IniWriteValue("Info4", "UutName", Info4.UutName);
            MultiFileHelper.IniWriteValue("Info4", "UutModule", Info4.UutModule);
            MultiFileHelper.IniWriteValue("Info4", "UutSN", Info4.UutSN);
            MultiFileHelper.IniWriteValue("Info4", "UutManufacture", Info4.UutManufacture);
            MultiFileHelper.IniWriteValue("Info4", "Accuracy", Info4.Accuracy);
            MultiFileHelper.IniWriteValue("Info4", "TemperatureLower", Info4.TemperatureLower);
            MultiFileHelper.IniWriteValue("Info4", "TemperatureUpper", Info4.TemperatureUpper);

            SensorSettingsMulti settings = new SensorSettingsMulti(Info);
            settings.ShowDialog();
        }

        /// <summary>
        /// 关闭窗体方法
        /// </summary>
        private void Close()
        {
            
        }
        #endregion
    }
}
