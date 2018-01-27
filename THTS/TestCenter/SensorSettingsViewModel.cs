using THTS.MVVM;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        private ObservableCollection<DataAccess.Device> _deviceList = new ObservableCollection<DataAccess.Device>();
        /// <summary>
        /// 传感器列表数据源
        /// </summary>
        public ObservableCollection<DataAccess.Device> DeviceList
        {
            get { return _deviceList; }
            set { _deviceList = value; OnPropertyChanged(); }
        }

        private DataAccess.Device _deviceSelected = new DataAccess.Device();
        /// <summary>
        /// 当前选中的传感器
        /// </summary>
        public DataAccess.Device DeviceSelected
        {
            get { return _deviceSelected; }
            set { _deviceSelected = value; OnPropertyChanged(); }
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

            DeviceList = DataAccess.EntityDAO.DeviceDAO.GetAllData();

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
                Thread thr = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        this.DispatcherInvoke(() =>
                        {
                            if (deviceSelect.DeviceSelectList != null && deviceSelect.DeviceSelectList.Count > 0)
                            {
                                DeviceList = MVVM.SequencingService.SetCollectionSequence(deviceSelect.DeviceSelectList);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                    }
                }));
                thr.IsBackground = true;
                thr.Start();
            }
            
        }

        /// <summary>
        /// 删除传感器
        /// </summary>
        private void SensorDelete()
        {
            //Thread thr = new Thread(new ThreadStart(() =>
            //{
            //    try
            //    {
            //        this.DispatcherInvoke(() =>
            //        {
            //            DeviceList.Remove(DeviceSelected);
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    finally
            //    {
            //    }
            //}));
            //thr.IsBackground = true;
            //thr.Start();
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
