﻿using System;
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
        }

        #region 方法
        /// <summary>
        /// 开始测试方法
        /// </summary>
        private void Start()
        {
            if (!DataAccess.EntityDAO.TestInfoDAO.SaveOrUpdate(Info))
            {
                System.Windows.MessageBox.Show("测试信息保存失败！");
                return;
            }

            SensorSettings settings = new SensorSettings();
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
