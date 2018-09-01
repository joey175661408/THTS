using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IDelegateCommand DeleteCommand { get; private set; }
        #endregion

        #region 属性
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
            ObservableCollection<TestData> datalist = TestDataDAO.GetData(testinfo.RecordSN);

            Dictionary<string, ObservableCollection<TestData>> groupBy = new Dictionary<string, ObservableCollection<TestData>>();

            string temp = string.Empty;
            for (int i = 0; i < datalist.Count; i++)
            {
                if(temp != datalist[i].TemperatureName)
                {
                    temp = datalist[i].TemperatureName;
                    groupBy.Add(temp, new ObservableCollection<TestData>());
                    groupBy[temp].Add(datalist[i]);
                    continue;
                }
                groupBy[temp].Add(datalist[i]);
            }

            foreach (var item in groupBy)
            {
                TestDataResult result = new TestDataResult
                {
                    TemperatureName = item.Key,
                    TemperatureValue = item.Value[0].TemperatureValue,
                    HumidityValue = item.Value[0].HumidityValue,
                    DataList = item.Value
                };
                result.CalcuteResult();
                ResultList.Add(result);
            }
        }
        #endregion

        #region 方法
        #endregion
    }
}
