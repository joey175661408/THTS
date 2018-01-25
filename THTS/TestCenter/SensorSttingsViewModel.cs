using THTS.MVVM;

namespace THTS.TestCenter
{
    public class SensorSttingsViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性
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

        public SensorSttingsViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);

            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();
        }

        #region 方法
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
