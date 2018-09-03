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
            ObservableCollection<TestData> datalist = TestDataDAO.GetData(Info.RecordSN);

            Dictionary<string, ObservableCollection<TestData>> groupBy = new Dictionary<string, ObservableCollection<TestData>>();

            string temp = string.Empty;
            for (int i = 0; i < datalist.Count; i++)
            {
                if (temp != datalist[i].TemperatureName)
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

        /// <summary>
        /// 导出数据
        /// </summary>
        private void Export()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "xls files(*.xls)|*.xls|All files(*.*)|*.*";
            save.DefaultExt = "xls";
            save.AddExtension = true;
            save.RestoreDirectory = true;
            save.FileName = Info.RecordSN + ".xls";
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelHelper helper = new ExcelHelper();
                    helper.ReadFromExcelTemplate(@".\Template\TemperatureAndHumidity9.xls");
                    helper.SetTestResultValue(Info, ResultList);

                    helper.WriteToFile(save.FileName, true);
                    MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！\r\n" + ex.Message);
                }

            }
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
