using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.MVVM;

namespace THTS.TestCenter
{
    public class TestCenterViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性
        private DataAccess.TestInfo _info;
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public DataAccess.TestInfo Info
        {
            get { return _info; }
            set { _info = value; OnPropertyChanged(); }
        }
        #endregion

        public TestCenterViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);

            Info = DataAccess.EntityDAO.TestInfoDAO.GetTestInfoData();
            Info.RecordSN = DateTime.Now.ToString("yyyyMMddHHmmss");
            Info.TestDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #region 方法
        /// <summary>
        /// 开始测试方法
        /// </summary>
        private void Start()
        {
            if(Info.TemperatuerVisibility == System.Windows.Visibility.Collapsed &&
                Info.HumidityVisibility == System.Windows.Visibility.Collapsed)
            {
                System.Windows.MessageBox.Show("无测试项目！");
                return;
            }

            if (DataAccess.EntityDAO.TestInfoDAO.IsExit(Info.RecordSN))
            {
                System.Windows.MessageBox.Show("此记录编号已存在！");
                return;
            }

            SensorSettings settings = new SensorSettings(Info);
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
