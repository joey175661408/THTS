using THTS.MVVM;
using System.Collections.Generic;

namespace THTS.TestCenter
{
    public class SensorSettingsViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand SensorSelectCommand { get; private set; }
        public IDelegateCommand SensorDeleteCommand { get; private set; }
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性
        /// <summary>
        /// 频道列表
        /// </summary>
        public List<string> ChannelList = new List<string>();

        private DataAccess.TemperatureTolerance _toleranceInfo;
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TemperatureTolerance ToleranceInfo
        {
            get { return _toleranceInfo; }
            set { _toleranceInfo = value; OnPropertyChanged(); }
        }
        #endregion

        public SensorSettingsViewModel()
        {
            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();
            for (int i = 0; i < 30; i++)
            {
                ChannelList.Add(i.ToString("00"));
            }

            SensorSelectCommand = new DelegateCommand(SensorSelect);
            SensorDeleteCommand = new DelegateCommand(SensorDelete);
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);

        }

        #region 方法
        /// <summary>
        /// 选择传感器
        /// </summary>
        private void SensorSelect()
        {
            DeviceCenter.DeviceCenter deviceSelect = new DeviceCenter.DeviceCenter(true);
            bool? result = deviceSelect.ShowDialog();
            if (result.HasValue && result.Value && deviceSelect.DeviceSelectList != null)
            {
                ToleranceInfo.DeviceSelectList = deviceSelect.DeviceSelectList;
            }
        }
        /// <summary>
        /// 删除传感器
        /// </summary>
        private void SensorDelete()
        {

        }
        /// <summary>
        /// 开始测试方法
        /// </summary>
        private void Start()
        {
            if (!DataAccess.EntityDAO.TemperatureToleranceDAO.SaveOrUpdate(ToleranceInfo))
            {
                System.Windows.MessageBox.Show("测试信息保存失败！");
                return;
            }

            SensorTest test = new SensorTest();
            test.ShowDialog();
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
