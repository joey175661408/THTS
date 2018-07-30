using THTS.MVVM;
using THTS.SerialPort;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THTS.SettingCenter
{
    public class SettingCenterViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand ConnectCommand { get; private set; }
        public IDelegateCommand SaveCommand { get; private set; }
        public IDelegateCommand CancelCommand { get; private set; }
        #endregion

        #region 属性

        private List<string> _portNameList = new List<string>();
        /// <summary>
        /// 串口号列表
        /// </summary>
        public List<string> PortNameList
        {
            get { return _portNameList; }
            set { _portNameList = value; OnPropertyChanged(); }
        }

        private string _portName = string.Empty;
        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; OnPropertyChanged(); }
        }

        private int _baudRate = 115200;
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; OnPropertyChanged(); }
        }

        private string _deviceType = "离线";
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; OnPropertyChanged(); }
        }

        iInstrument instrument = new iInstrument("COM4", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

        #endregion

        #region 构造函数
        public SettingCenterViewModel()
        {
            ConnectCommand = new DelegateCommand(Connect);
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);

            PortNameList = new List<string>(System.IO.Ports.SerialPort.GetPortNames());

            if (PortNameList.Count >= 1)
            {
                PortName = PortNameList[0];
            }


        }
        #endregion

        #region 方法

        /// <summary>
        /// 连接设备
        /// </summary>
        private void Connect()
        {
            if (PortName == string.Empty)
            {
                MessageBox.Show("请选择串口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            instrument = new iInstrument(PortName, BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            if (!instrument.Open())
            {
                return;
            }

            DeviceType = instrument.GetDeviceType();

            instrument.Close();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void Save()
        {
            DataAccess.Setting setting = new DataAccess.Setting();
            setting.No = "1";
            setting.PortName = PortName;
            setting.BaudRate = BaudRate;

            if (DataAccess.SettingsDAO.SaveOrUpdate(setting))
            {
                MessageBox.Show("保存成功！");
            }else
            {
                MessageBox.Show("保存失败！");
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {

        }
        #endregion
    }
}
