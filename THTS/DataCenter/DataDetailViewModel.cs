using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THTS.DataAccess;
using THTS.DataAccess.Entity;
using THTS.DataAccess.EntityDAO;
using THTS.MVVM;

namespace THTS.DataCenter
{
    public class DataDetailViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand EditCommand { get; private set; }
        public IDelegateCommand ExportRecordCommand { get; private set; }
        #endregion

        #region 属性
        private TestInfo _info;
        /// <summary>
        /// 测试参数
        /// </summary>
        public TestInfo Info
        {
            get { return _info; }
            set { _info = value; OnPropertyChanged(); }
        }

        private TemperatureTolerance _toleranceInfo = new TemperatureTolerance();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public TemperatureTolerance ToleranceInfo
        {
            get { return _toleranceInfo; }
            set { _toleranceInfo = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TestDataResult> _resultList = new ObservableCollection<TestDataResult>();
        /// <summary>
        /// 最终结果数据
        /// </summary>
        public ObservableCollection<TestDataResult> ResultList
        {
            get { return _resultList; }
            set { _resultList = value; OnPropertyChanged(); }
        }
        #endregion

        #region 构造函数
        public DataDetailViewModel(TestInfo testinfo)
        {
            Info = testinfo;

            ToleranceInfo = TemperatureToleranceDAO.GetToleranceInfoData(testinfo.RecordSN);

            EditCommand = new DelegateCommand(Edit);
            ExportRecordCommand = new DelegateCommand(Export);

            CalcuteAndGroupBy();

        }


        #endregion

        #region 方法

        /// <summary>
        /// 计算数据并分组
        /// </summary>
        private void CalcuteAndGroupBy()
        {
            ResultList = TestDataDAO.CalcuteAndGroupBy(Info.RecordSN);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        private void Export()
        {
            ExcelHelper helper = new ExcelHelper();
            helper.ExportDataDetail(Info, ResultList);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        private void Edit()
        {
        }

        #endregion
    }
}
