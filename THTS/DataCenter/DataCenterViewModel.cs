using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess;
using THTS.DataAccess.EntityDAO;
using THTS.MVVM;

namespace THTS.DataCenter
{
     public class DataCenterViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand EditCommand { get; private set; }
        public IDelegateCommand ExportRecordCommand { get; private set; }
        public IDelegateCommand DeleteCommand { get; private set; }
        #endregion

        #region 属性
        private ObservableCollection<TestInfo> _testRecordList;
        /// <summary>
        /// 数据记录
        /// </summary>
        public ObservableCollection<TestInfo> TestRecordList
        {
            get { return _testRecordList; }
            set { _testRecordList = value; OnPropertyChanged(); }
        }

        private TestInfo _selectedTestInfo;
        /// <summary>
        /// 选中的数据
        /// </summary>
        public TestInfo SelectedTestInfo
        {
            get { return _selectedTestInfo; }
            set { _selectedTestInfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region 构造函数
        public DataCenterViewModel()
        {
            EditCommand = new DelegateCommand(Edit);
            ExportRecordCommand = new DelegateCommand(Export);
            DeleteCommand = new DelegateCommand(Delete);

            TestRecordList = TestInfoDAO.GetAllData();
            TestRecordList = SequencingService.SetCollectionSequence(TestRecordList);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 查看编辑
        /// </summary>
        private void Edit()
        {
            if(SelectedTestInfo == null)
            {
                return;
            }

            DataDetailView view = new DataDetailView(SelectedTestInfo);
            view.ShowDialog();
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete()
        {
            if (SelectedTestInfo == null)
            {
                return;
            }

            if (TestInfoDAO.Delete(SelectedTestInfo))
            {
                TestRecordList = TestInfoDAO.GetAllData();
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        private void Export()
        {
            if (SelectedTestInfo == null)
            {
                return;
            }

            ExcelHelper helper = new ExcelHelper();
            helper.ExportDataDetail(SelectedTestInfo, TestDataDAO.CalcuteAndGroupBy(SelectedTestInfo.RecordSN));
        }
        #endregion
    }
}
