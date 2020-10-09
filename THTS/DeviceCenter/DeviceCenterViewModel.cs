using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        public IDelegateCommand SearchFlyoutCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        public IDelegateCommand SearchCommand { get; private set; }
        public IDelegateCommand ResetSearchConditionsCommand { get; private set; }
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

        private DataAccess.Device _deviceSelected = new DataAccess.Device();
        /// <summary>
        /// 当前选中的传感器
        /// </summary>
        public DataAccess.Device DeviceSelected
        {
            get { return _deviceSelected; }
            set { _deviceSelected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 查询窗口是否弹开
        /// </summary>
        public bool IsOpen { get; set; }

        #endregion

        public DeviceCenterViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            SearchFlyoutCommand = new DelegateCommand(SearchFlyout);
            CloseCommand = new DelegateCommand(Close);
            SearchCommand = new DelegateCommand(Search);
            ResetSearchConditionsCommand = new DelegateCommand(ResetSearchConditions);

            Refresh();

            IsOpen = false;
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
                                if (DeviceSelected != null)
                                    tempSelected = DeviceList.FirstOrDefault(t => t.Id == DeviceSelected.Id);

                                DeviceSelected = tempSelected == null ? DeviceList[0] : tempSelected;
                            }
                            else
                                DeviceSelected = null;
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
                else
                {
                    MessageBox.Show("保存失败！");
                }
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        private void Edit()
        {
            DeviceNew newDevice = new DeviceNew(DeviceSelected);
            bool? result = newDevice.ShowDialog();

            if (result.HasValue && result.Value)
            {
                bool update = DataAccess.EntityDAO.DeviceDAO.Update(newDevice.NewDevice);
                if (update)
                {
                    Refresh();
                }
                else
                {
                    MessageBox.Show("更新失败！");
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete()
        {
            if(DeviceSelected == null || DeviceSelected.SensorId == null)
            {
                return;
            }

            bool delete = DataAccess.EntityDAO.DeviceDAO.Delete(DeviceSelected);
            if (delete)
            {
                Refresh();
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }


        /// <summary>
        /// 弹出查询窗口
        /// </summary>
        private void SearchFlyout()
        {
            IsOpen = !IsOpen;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void Close()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void Search()
        {
        }

        /// <summary>
        /// 清空查询条件
        /// </summary>
        private void ResetSearchConditions()
        {
        }
        #endregion
    }
}
