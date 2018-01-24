using System;
using System.Linq;
using System.Threading;
using THTS.MVVM;
using System.Collections.ObjectModel;

namespace THTS.DeviceCenter
{
    public class DeviceCenterViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand AddCommand { get; private set; }
        public IDelegateCommand EditCommand { get; private set; }
        public IDelegateCommand DeleteCommand { get; private set; }
        #endregion

        #region 属性
        private ObservableCollection<DataAccess.Device> _deviceList = new ObservableCollection<DataAccess.Device>();
        /// <summary>
        /// 传感器列表数据源
        /// </summary>
        public ObservableCollection<DataAccess.Device> DeviceList
        {
            get { return _deviceList; }
            set { _deviceList = value; OnPropertyChanged(); }
        }

        private DataAccess.Device _selectedDevice;
        /// <summary>
        /// 当前选中的传感器
        /// </summary>
        public DataAccess.Device SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; OnPropertyChanged(); }
        }

        #endregion

        public DeviceCenterViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);

            DeviceList = DataAccess.EntityDAO.DeviceDAO.GetAllData();
        }

        #region 方法
        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            try
            {
                Thread thr = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        DeviceList = DataAccess.EntityDAO.DeviceDAO.GetAllData();

                        this.DispatcherInvoke(() =>
                        {
                            if (DeviceList != null && DeviceList.Count > 0)
                            {
                                DeviceList = MVVM.SequencingService.SetCollectionSequence(DeviceList);

                                DataAccess.Device tempSelected = null;
                                if (SelectedDevice != null)
                                    tempSelected = DeviceList.FirstOrDefault(t => t.Id == SelectedDevice.Id);

                                SelectedDevice = tempSelected == null ? DeviceList[0] : tempSelected;
                            }
                            else
                                SelectedDevice = null;
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
            catch (Exception err)
            { }
        }

        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DeviceNew newDevice = new DeviceNew();
            bool? result = newDevice.ShowDialog();

            if (result.HasValue && result.Value)
            {
                bool save = DataAccess.EntityDAO.DeviceDAO.Save(newDevice.NewDevice);
                if (save)
                {
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        private void Edit()
        {
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete()
        {
        }
        #endregion
    }
}
