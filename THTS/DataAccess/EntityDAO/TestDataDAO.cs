using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess.Entity;

namespace THTS.DataAccess.EntityDAO
{
    public class TestDataDAO : IDisposable
    {
        SQLiteDB context;

        public TestDataDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        public static ObservableCollection<TestData> GetData(string recordSN)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestDatas.Where(t => t.RecordSN == recordSN).Load();
                return ctx.TestDatas.Local;
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public static bool Save(ObservableCollection<TestData> dataList)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestDatas.AddRange(dataList);
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 计算数据并分组
        /// </summary>
        public static ObservableCollection<TestDataResult> CalcuteAndGroupBy(string recordSN)
        {
            ObservableCollection<TestDataResult> ResultList = new ObservableCollection<TestDataResult>();

            ObservableCollection<TestData> datalist = GetData(recordSN);

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

            return ResultList;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        #endregion
    }
}
