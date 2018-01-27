using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using THTS.MVVM;
using System.Collections.ObjectModel;

namespace THTS.UserCenter
{
    public class UserCenterViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand AddCommand { get; private set; }
        public IDelegateCommand EditCommand { get; private set; }
        public IDelegateCommand DeleteCommand { get; private set; }
        #endregion

        #region 属性
        private ObservableCollection<DataAccess.User> _userList = new ObservableCollection<DataAccess.User>();
        /// <summary>
        /// 用户列表数据源
        /// </summary>
        public ObservableCollection<DataAccess.User> UserList
        {
            get { return _userList; }
            set { _userList = value; OnPropertyChanged(); }
        }

        private DataAccess.User _userSelected = new DataAccess.User();
        /// <summary>
        /// 当前选中的用户
        /// </summary>
        public DataAccess.User UserSelected
        {
            get { return _userSelected; }
            set { _userSelected = value; OnPropertyChanged(); }
        }
        #endregion

        public UserCenterViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);

            Refresh();
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
                        UserList = DataAccess.UserDAO.GetAllData();

                        this.DispatcherInvoke(() =>
                        {
                            if (UserList != null && UserList.Count > 0)
                            {
                                UserList = MVVM.SequencingService.SetCollectionSequence(UserList);

                                DataAccess.User tempSelected = null;
                                if (UserSelected != null)
                                    tempSelected = UserList.FirstOrDefault(t => t.Id == UserSelected.Id);

                                UserSelected = tempSelected == null ? UserList[0] : tempSelected;
                            }
                            else
                                UserSelected = null;
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
            UserNew newUser = new UserNew();
            bool? result = newUser.ShowDialog();

            if (result.HasValue && result.Value)
            {
                bool save = DataAccess.UserDAO.Save(newUser.NewUser);
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
