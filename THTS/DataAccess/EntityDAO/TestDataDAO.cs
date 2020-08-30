using MahApps.Metro.Controls;
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
        public static ObservableCollection<TestDataResult> CalcuteAndGroupBy(TestInfo info, bool calculateWithTolerance)
        {
            ObservableCollection<TestDataResult> ResultList = new ObservableCollection<TestDataResult>();

            ObservableCollection<TestData> datalist = GetData(info.RecordSN);

            #region 计算修正值
            if (calculateWithTolerance)
            {
                ObservableCollection<PositionAndSensor> positionAndSensors = PositionAndSensorDAO.GetPositionAndSensor(info.RecordSN);
                
                foreach (var posionandsensor in positionAndSensors)
                {
                    if(posionandsensor.SensorID == null)
                    {
                        continue;
                    }

                    posionandsensor.ReflashSensorInfo();

                    if (posionandsensor.sensor.ModuleName.Contains("PT100"))
                    {
                        if (!info.Extra4.Equals("True"))
                            continue;
                    }

                    if (posionandsensor.sensor.ModuleName.Contains("湿度传感器"))
                    {
                        if (!info.Extra5.Equals("True"))
                            continue;
                    }

                    if (posionandsensor.sensor.ModuleName.Contains("K"))
                    {
                        if (!info.Extra6.Equals("True"))
                            continue;
                    }

                    if (posionandsensor.sensor.IsCalculate.Equals("True"))
                    {
                        foreach (var item in datalist)
                        {
                            switch (posionandsensor.PositionName)
                            {
                                case "1":
                                    item.A = posionandsensor.sensor.CalcWithTolerance(item.A, posionandsensor.sensor.ModuleName);
                                    break;
                                case "2":
                                    item.B = posionandsensor.sensor.CalcWithTolerance(item.B, posionandsensor.sensor.ModuleName);
                                    break;
                                case "3":
                                    item.C = posionandsensor.sensor.CalcWithTolerance(item.C, posionandsensor.sensor.ModuleName);
                                    break;
                                case "4":
                                    item.D = posionandsensor.sensor.CalcWithTolerance(item.D, posionandsensor.sensor.ModuleName);
                                    break;
                                case "5":
                                    item.O = posionandsensor.sensor.CalcWithTolerance(item.O, posionandsensor.sensor.ModuleName);
                                    break;
                                case "6":
                                    item.E = posionandsensor.sensor.CalcWithTolerance(item.E, posionandsensor.sensor.ModuleName);
                                    break;
                                case "7":
                                    item.F = posionandsensor.sensor.CalcWithTolerance(item.F, posionandsensor.sensor.ModuleName);
                                    break;
                                case "8":
                                    item.G = posionandsensor.sensor.CalcWithTolerance(item.G, posionandsensor.sensor.ModuleName);
                                    break;
                                case "9":
                                    item.H = posionandsensor.sensor.CalcWithTolerance(item.H, posionandsensor.sensor.ModuleName);
                                    break;
                                case "10":
                                    item.I = posionandsensor.sensor.CalcWithTolerance(item.I, posionandsensor.sensor.ModuleName);
                                    break;
                                case "11":
                                    item.J = posionandsensor.sensor.CalcWithTolerance(item.J, posionandsensor.sensor.ModuleName);
                                    break;
                                case "12":
                                    item.K = posionandsensor.sensor.CalcWithTolerance(item.K, posionandsensor.sensor.ModuleName);
                                    break;
                                case "13":
                                    item.L = posionandsensor.sensor.CalcWithTolerance(item.L, posionandsensor.sensor.ModuleName);
                                    break;
                                case "14":
                                    item.M = posionandsensor.sensor.CalcWithTolerance(item.M, posionandsensor.sensor.ModuleName);
                                    break;
                                case "15":
                                    item.N = posionandsensor.sensor.CalcWithTolerance(item.N, posionandsensor.sensor.ModuleName);
                                    break;
                                case "16":
                                    item.T16 = posionandsensor.sensor.CalcWithTolerance(item.T16, posionandsensor.sensor.ModuleName);
                                    break;
                                case "17":
                                    item.T17 = posionandsensor.sensor.CalcWithTolerance(item.T17, posionandsensor.sensor.ModuleName);
                                    break;
                                case "18":
                                    item.T18 = posionandsensor.sensor.CalcWithTolerance(item.T18, posionandsensor.sensor.ModuleName);
                                    break;
                                case "19":
                                    item.T19 = posionandsensor.sensor.CalcWithTolerance(item.T19, posionandsensor.sensor.ModuleName);
                                    break;
                                case "20":
                                    item.T20 = posionandsensor.sensor.CalcWithTolerance(item.T20, posionandsensor.sensor.ModuleName);
                                    break;
                                case "21":
                                    item.T21 = posionandsensor.sensor.CalcWithTolerance(item.T21, posionandsensor.sensor.ModuleName);
                                    break;
                                case "22":
                                    item.T22 = posionandsensor.sensor.CalcWithTolerance(item.T22, posionandsensor.sensor.ModuleName);
                                    break;
                                case "23":
                                    item.T23 = posionandsensor.sensor.CalcWithTolerance(item.T23, posionandsensor.sensor.ModuleName);
                                    break;
                                case "24":
                                    item.T24 = posionandsensor.sensor.CalcWithTolerance(item.T24, posionandsensor.sensor.ModuleName);
                                    break;
                                case "25":
                                    item.T25 = posionandsensor.sensor.CalcWithTolerance(item.T25, posionandsensor.sensor.ModuleName);
                                    break;
                                case "26":
                                    item.T26 = posionandsensor.sensor.CalcWithTolerance(item.T26, posionandsensor.sensor.ModuleName);
                                    break;
                                case "27":
                                    item.T27 = posionandsensor.sensor.CalcWithTolerance(item.T27, posionandsensor.sensor.ModuleName);
                                    break;
                                case "O":
                                    item.Jia = posionandsensor.sensor.CalcWithTolerance(item.Jia, posionandsensor.sensor.ModuleName);
                                    break;
                                case "A":
                                    item.Yi = posionandsensor.sensor.CalcWithTolerance(item.Yi, posionandsensor.sensor.ModuleName);
                                    break;
                                case "B":
                                    item.Bing = posionandsensor.sensor.CalcWithTolerance(item.Bing, posionandsensor.sensor.ModuleName);
                                    break;
                                case "C":
                                    item.Ding = posionandsensor.sensor.CalcWithTolerance(item.Ding, posionandsensor.sensor.ModuleName);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

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
