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
        public IDelegateCommand CalcuteRecordCommand { get; private set; }
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
            if(testinfo.PositionType == 27)
            {
                testinfo.HumiAverageIsChecked = false;
                testinfo.HumiDepartureIsChecked = false;
            }
            Info = testinfo;

            ToleranceInfo.Info = testinfo;

            ToleranceInfo.PositionType = Info.PositionType;

            EditCommand = new DelegateCommand(Edit);
            CalcuteRecordCommand = new DelegateCommand(CalcuteWithTolerance);
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
            ResultList = TestDataDAO.CalcuteAndGroupBy(Info, false);
            AddMaxAndMinValue();
        }

        /// <summary>
        /// 基于修正值计算
        /// </summary>
        /// <param name="obj"></param>
        private void CalcuteWithTolerance(object obj)
        {
            ResultList = TestDataDAO.CalcuteAndGroupBy(Info, true);
            AddMaxAndMinValue();
        }

        /// <summary>
        /// 每个传感器测试数据最后增加最大值和最小值
        /// </summary>
        private void AddMaxAndMinValue()
        {
            for (int i = 0; i < ResultList.Count; i++)
            {
                TestData maxvalue = new TestData()
                {
                    Time = "最大值",
                    A = TryParseToFloat(ResultList[i].FluctuationList["1"].MaxValue),
                    B = TryParseToFloat(ResultList[i].FluctuationList["2"].MaxValue),
                    C = TryParseToFloat(ResultList[i].FluctuationList["3"].MaxValue),
                    D = TryParseToFloat(ResultList[i].FluctuationList["4"].MaxValue),
                    O = TryParseToFloat(ResultList[i].FluctuationList["5"].MaxValue),
                    E = TryParseToFloat(ResultList[i].FluctuationList["6"].MaxValue),
                    F = TryParseToFloat(ResultList[i].FluctuationList["7"].MaxValue),
                    G = TryParseToFloat(ResultList[i].FluctuationList["8"].MaxValue),
                    H = TryParseToFloat(ResultList[i].FluctuationList["9"].MaxValue),
                    I = TryParseToFloat(ResultList[i].FluctuationList["10"].MaxValue),
                    J = TryParseToFloat(ResultList[i].FluctuationList["11"].MaxValue),
                    K = TryParseToFloat(ResultList[i].FluctuationList["12"].MaxValue),
                    L = TryParseToFloat(ResultList[i].FluctuationList["13"].MaxValue),
                    M = TryParseToFloat(ResultList[i].FluctuationList["14"].MaxValue),
                    N = TryParseToFloat(ResultList[i].FluctuationList["15"].MaxValue),
                    T16 = TryParseToFloat(ResultList[i].FluctuationList["16"].MaxValue),
                    T17 = TryParseToFloat(ResultList[i].FluctuationList["17"].MaxValue),
                    T18 = TryParseToFloat(ResultList[i].FluctuationList["18"].MaxValue),
                    T19 = TryParseToFloat(ResultList[i].FluctuationList["19"].MaxValue),
                    T20 = TryParseToFloat(ResultList[i].FluctuationList["20"].MaxValue),
                    T21 = TryParseToFloat(ResultList[i].FluctuationList["21"].MaxValue),
                    T22 = TryParseToFloat(ResultList[i].FluctuationList["22"].MaxValue),
                    T23 = TryParseToFloat(ResultList[i].FluctuationList["23"].MaxValue),
                    T24 = TryParseToFloat(ResultList[i].FluctuationList["24"].MaxValue),
                    T25 = TryParseToFloat(ResultList[i].FluctuationList["25"].MaxValue),
                    T26 = TryParseToFloat(ResultList[i].FluctuationList["26"].MaxValue),
                    T27 = TryParseToFloat(ResultList[i].FluctuationList["27"].MaxValue),

                    Jia = TryParseToFloat(ResultList[i].FluctuationList["O"].MaxValue),
                    Yi = TryParseToFloat(ResultList[i].FluctuationList["A"].MaxValue),
                    Bing = TryParseToFloat(ResultList[i].FluctuationList["B"].MaxValue),
                    Ding = TryParseToFloat(ResultList[i].FluctuationList["C"].MaxValue),
                };

                TestData minvalue = new TestData()
                {
                    Time = "最小值",
                    A = TryParseToFloat(ResultList[i].FluctuationList["1"].MinValue),
                    B = TryParseToFloat(ResultList[i].FluctuationList["2"].MinValue),
                    C = TryParseToFloat(ResultList[i].FluctuationList["3"].MinValue),
                    D = TryParseToFloat(ResultList[i].FluctuationList["4"].MinValue),
                    O = TryParseToFloat(ResultList[i].FluctuationList["5"].MinValue),
                    E = TryParseToFloat(ResultList[i].FluctuationList["6"].MinValue),
                    F = TryParseToFloat(ResultList[i].FluctuationList["7"].MinValue),
                    G = TryParseToFloat(ResultList[i].FluctuationList["8"].MinValue),
                    H = TryParseToFloat(ResultList[i].FluctuationList["9"].MinValue),
                    I = TryParseToFloat(ResultList[i].FluctuationList["10"].MinValue),
                    J = TryParseToFloat(ResultList[i].FluctuationList["11"].MinValue),
                    K = TryParseToFloat(ResultList[i].FluctuationList["12"].MinValue),
                    L = TryParseToFloat(ResultList[i].FluctuationList["13"].MinValue),
                    M = TryParseToFloat(ResultList[i].FluctuationList["14"].MinValue),
                    N = TryParseToFloat(ResultList[i].FluctuationList["15"].MinValue),
                    T16 = TryParseToFloat(ResultList[i].FluctuationList["16"].MinValue),
                    T17 = TryParseToFloat(ResultList[i].FluctuationList["17"].MinValue),
                    T18 = TryParseToFloat(ResultList[i].FluctuationList["18"].MinValue),
                    T19 = TryParseToFloat(ResultList[i].FluctuationList["19"].MinValue),
                    T20 = TryParseToFloat(ResultList[i].FluctuationList["20"].MinValue),
                    T21 = TryParseToFloat(ResultList[i].FluctuationList["21"].MinValue),
                    T22 = TryParseToFloat(ResultList[i].FluctuationList["22"].MinValue),
                    T23 = TryParseToFloat(ResultList[i].FluctuationList["23"].MinValue),
                    T24 = TryParseToFloat(ResultList[i].FluctuationList["24"].MinValue),
                    T25 = TryParseToFloat(ResultList[i].FluctuationList["25"].MinValue),
                    T26 = TryParseToFloat(ResultList[i].FluctuationList["26"].MinValue),
                    T27 = TryParseToFloat(ResultList[i].FluctuationList["27"].MinValue),

                    Jia = TryParseToFloat(ResultList[i].FluctuationList["O"].MinValue),
                    Yi = TryParseToFloat(ResultList[i].FluctuationList["A"].MinValue),
                    Bing = TryParseToFloat(ResultList[i].FluctuationList["B"].MinValue),
                    Ding = TryParseToFloat(ResultList[i].FluctuationList["C"].MinValue),
                };

                ResultList[i].DataList.Add(maxvalue);
                ResultList[i].DataList.Add(minvalue);

            }
        }

        /// <summary>
        /// 将最大值和最小值String类型转换为Float，转换失败则返回-1000
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float TryParseToFloat(string value)
        {
            float result = -1000;

            if (float.TryParse(value, out result))
            {
                return result;
            };

            return -1000;
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
