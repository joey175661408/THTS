using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using System.Drawing;
using System.IO;
using NPOI.HPSF;
using System.Data;
using NPOI.SS.UserModel;
using System.Collections.ObjectModel;
using THTS.DataAccess;
using THTS.DataAccess.Entity;
using System.Windows.Forms;

namespace THTS
{
    public class ExcelHelper
    {
        IWorkbook hssfworkbook;
        /// <summary>
        /// HSSFWorkbook
        /// </summary>
        public IWorkbook Workbook
        {
            get
            {
                if (hssfworkbook == null)
                {
                    InitializeWorkbook();
                }

                return hssfworkbook;
            }

            set { hssfworkbook = value; }
        }

        /// <summary>
        /// 从Excel模板读取数据
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadFromExcelTemplate(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))  //路径，打开权限，读取权限
            {
                hssfworkbook = new HSSFWorkbook(file);
                file.Close();
            }
        }

        /// <summary>
        /// 初始化工作簿
        /// </summary>
        public void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();

            //Create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Joey";
            (hssfworkbook as HSSFWorkbook).DocumentSummaryInformation = dsi;

            //Create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Report";
            (hssfworkbook as HSSFWorkbook).SummaryInformation = si;

        }

        /// <summary>
        /// 插入行标题
        /// </summary>
        /// <param name="rowNum">行标题在Excel中的位置（从0开始）</param>
        /// <param name="columns">列头名集合</param>
        /// <param name="sheetName">表名</param>
        /// <returns></returns>
        public bool SetTitleValue(int rowNum, string[] columns, string sheetName)
        {
            ISheet sheet = GetOrCreateSheet(sheetName);
            if (sheet == null)
                return false;

            IRow row = GetOrCreateRow(sheet, rowNum);
            if (row == null)
                return false;

            IFont font = hssfworkbook.CreateFont();
            font.FontName = "宋体";
            font.Boldweight = (short)FontBoldWeight.Bold;

            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(font);

            for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
            {
                ICell cell = GetOrCreateColumn(row, columnIndex);
                cell.CellStyle = style;
                SetCellValue(cell, new NPOIValue(rowNum, cell.ColumnIndex, columns[columnIndex]));
            }
            return true;
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="data">数据库表</param>
        /// <param name="dataFirstRowNum">数据库表在Excel中第一行的位置</param>
        /// <param name="columns">列名数组</param>
        /// <param name="sheetName">Excel数据表名</param>
        /// <returns></returns>
        public bool SetCellValue(DataTable data, int dataFirstRowNum, string[] columns, string sheetName)
        {
            ISheet sheet = GetOrCreateSheet(sheetName);
            if (sheet == null)
                return false;

            if (data == null || data.Rows.Count == 0)
                return true;

            IRow formatRow = GetOrCreateRow(sheet, dataFirstRowNum);
            for (int dataIndex = 0; dataIndex < data.Rows.Count; dataIndex++)
            {
                IRow row = GetOrCreateRow(sheet, dataIndex + dataFirstRowNum);
                row.Height = formatRow.Height;
                for (int colIndex = 0; colIndex < columns.Length; colIndex++)
                {
                    ICell formatCell = GetOrCreateColumn(formatRow, colIndex);
                    ICell cell = GetOrCreateColumn(row, colIndex);
                    cell.CellStyle = formatCell.CellStyle;
                    SetCellValue(cell, new NPOIValue(row.RowNum, cell.ColumnIndex, data.Rows[dataIndex][columns[colIndex]]));
                }
            }
            return true;
        }

        /// <summary>
        /// 设置疲劳测试数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataFirstRowNum"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public bool SetTiredValue(ObservableCollection<DebugTiredTest> data, int dataFirstRowNum, string sheetName)
        {
            ISheet sheet = GetOrCreateSheet(sheetName);
            if (sheet == null)
                return false;

            if (data == null || data.Count == 0)
                return true;

            IRow formatRow = GetOrCreateRow(sheet, dataFirstRowNum);
            for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
            {
                IRow row = GetOrCreateRow(sheet, dataIndex + dataFirstRowNum);
                row.Height = formatRow.Height;

                ICell formatCell0 = GetOrCreateColumn(formatRow, 0);
                ICell cell0 = GetOrCreateColumn(row, 0);
                cell0.CellStyle = formatCell0.CellStyle;
                SetCellValue(cell0, new NPOIValue(row.RowNum, cell0.ColumnIndex, dataIndex+1));

                ICell formatCell1 = GetOrCreateColumn(formatRow, 1);
                ICell cell1 = GetOrCreateColumn(row, 1);
                cell1.CellStyle = formatCell1.CellStyle;
                SetCellValue(cell1, new NPOIValue(row.RowNum, cell1.ColumnIndex, data[dataIndex].Time));

                ObservableCollection<SerialPort.SensorRealValue> valueList = data[dataIndex].SensorValueList;

                for (int colIndex = 0; colIndex < valueList.Count; colIndex++)
                {
                    ICell formatCell = GetOrCreateColumn(formatRow, colIndex + 2);
                    ICell cell = GetOrCreateColumn(row, colIndex + 2);
                    cell.CellStyle = formatCell.CellStyle;
                    SetCellValue(cell, new NPOIValue(row.RowNum, cell.ColumnIndex, (double)valueList[colIndex].SensorValue));
                }
            }
            return true;
        }

        /// <summary>
        /// 设置测试结果数据
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="ResultList"></param>
        /// <returns></returns>
        public bool SetTestResultValue(TestInfo Info, ObservableCollection<TestDataResult> ResultList)
        {
            ISheet sheet = GetOrCreateSheet("Information");
            IRow row = GetOrCreateRow(sheet, 2);
            ICell cell0 = GetOrCreateColumn(row, 0);
            cell0.SetCellValue(Info.Company);
            ICell cell1 = GetOrCreateColumn(row, 1);
            cell1.SetCellValue(Info.UutName);
            ICell cell2 = GetOrCreateColumn(row, 2);
            cell2.SetCellValue(Info.CertificateSN);
            ICell cell3 = GetOrCreateColumn(row, 3);
            cell3.SetCellValue(Info.UutManufacture);
            ICell cell4 = GetOrCreateColumn(row, 4);
            cell4.SetCellValue(Info.UutModule);
            ICell cell5 = GetOrCreateColumn(row, 5);
            cell5.SetCellValue(Info.UutSN);
            ICell cell6 = GetOrCreateColumn(row, 6);
            cell6.SetCellValue(Info.UutCalPosition);
            ICell cell7 = GetOrCreateColumn(row, 7);
            cell7.SetCellValue(Info.EnvironmentTemperature);
            ICell cell8 = GetOrCreateColumn(row, 8);
            cell8.SetCellValue(Info.EnvironmentHumidity);
            ICell cell9 = GetOrCreateColumn(row, 9);
            cell9.SetCellValue(Info.EnvironmentPressure);
            ICell cell10 = GetOrCreateColumn(row, 10);
            cell10.SetCellValue(Info.VerifiedBy);
            ICell cell11 = GetOrCreateColumn(row, 11);
            cell11.SetCellValue(Info.CheckedBy);
            ICell cell12 = GetOrCreateColumn(row, 12);
            cell12.SetCellValue(Info.GetTestDate);
            ICell cell13 = GetOrCreateColumn(row, 13);
            cell13.SetCellValue(Info.RecordSN);
            ICell cell14 = GetOrCreateColumn(row, 14);
            cell14.SetCellValue(Info.Accuracy);
            ICell cell15 = GetOrCreateColumn(row, 15);
            cell15.SetCellValue(Info.TemperatureLower);
            ICell cell16 = GetOrCreateColumn(row, 16);
            cell16.SetCellValue(Info.TemperatureUpper);
            ICell cell17 = GetOrCreateColumn(row, 17);
            cell17.SetCellValue(Info.Extra3);
            ICell cell18 = GetOrCreateColumn(row, 18);
            cell18.SetCellValue(Info.Extra1);
            ICell cell19 = GetOrCreateColumn(row, 19);
            cell19.SetCellValue(Info.Extra2);

            IRow rowCal = GetOrCreateRow(sheet, 6);
            ICell cellCal0 = GetOrCreateColumn(rowCal, 0);
            cellCal0.SetCellValue(Info.CalName);
            ICell cellCal1 = GetOrCreateColumn(rowCal, 1);
            cellCal1.SetCellValue(Info.CalModule);
            ICell cellCal2 = GetOrCreateColumn(rowCal, 2);
            cellCal2.SetCellValue(Info.CalAccuracy);
            ICell cellCal3 = GetOrCreateColumn(rowCal, 3);
            cellCal3.SetCellValue(Info.CalCertificateSN);
            ICell cellCal4 = GetOrCreateColumn(rowCal, 4);
            cellCal4.SetCellValue(Info.CalExpiryDate);

            for (int i = 0; i < ResultList.Count; i++)
            {
                ISheet sheetSource = GetOrCreateSheet("Source" + (i + 1));

                IRow rowI = GetOrCreateRow(sheetSource, 2);
                ICell cellI0 = GetOrCreateColumn(rowI, 0);
                cellI0.SetCellValue(ResultList[i].TemperatureValue);
                ICell cellI1 = GetOrCreateColumn(rowI, 1);
                cellI1.SetCellValue(ResultList[i].HumidityValue);
                ICell cellI2 = GetOrCreateColumn(rowI, 2);
                cellI2.SetCellValue(ResultList[i].TemperatureDepartureUpValue);
                ICell cellI3 = GetOrCreateColumn(rowI, 3);
                cellI3.SetCellValue(ResultList[i].TemperatureDepartureDownValue);
                ICell cellI4 = GetOrCreateColumn(rowI, 4);
                cellI4.SetCellValue(ResultList[i].TemperatureAverageValue);
                ICell cellI5 = GetOrCreateColumn(rowI, 5);
                cellI5.SetCellValue(ResultList[i].TemperatureFluctuationValue);
                ICell cellI6 = GetOrCreateColumn(rowI, 6);
                cellI6.SetCellValue(ResultList[i].HumidityDepartureUpValue);
                ICell cellI7 = GetOrCreateColumn(rowI, 7);
                cellI7.SetCellValue(ResultList[i].HumidityDepartureDownValue);
                ICell cellI8 = GetOrCreateColumn(rowI, 8);
                cellI8.SetCellValue(ResultList[i].HumidityAverageValue);
                ICell cellI9 = GetOrCreateColumn(rowI, 9);
                cellI9.SetCellValue(ResultList[i].HumidityFluctuationValue);

                for (int j = 0; j < ResultList[i].DataList.Count; j++)
                {
                    IRow rowJ = GetOrCreateRow(sheetSource, 9 + j);
                    GetOrCreateColumn(rowJ, 0).SetCellValue(ResultList[i].DataList[j].Time);
                    GetOrCreateColumn(rowJ, 1).SetCellValue(ResultList[i].DataList[j].Count);
                    GetOrCreateColumn(rowJ, 2).SetCellValue(ResultList[i].DataList[j].DeviceTemperature);

                    GetOrCreateColumn(rowJ, 3).SetCellValue(ResultList[i].DataList[j].StringA);
                    GetOrCreateColumn(rowJ, 4).SetCellValue(ResultList[i].DataList[j].StringB);
                    GetOrCreateColumn(rowJ, 5).SetCellValue(ResultList[i].DataList[j].StringC);
                    GetOrCreateColumn(rowJ, 6).SetCellValue(ResultList[i].DataList[j].StringD);
                    GetOrCreateColumn(rowJ, 7).SetCellValue(ResultList[i].DataList[j].StringO);
                    GetOrCreateColumn(rowJ, 8).SetCellValue(ResultList[i].DataList[j].StringE);
                    GetOrCreateColumn(rowJ, 9).SetCellValue(ResultList[i].DataList[j].StringF);
                    GetOrCreateColumn(rowJ, 10).SetCellValue(ResultList[i].DataList[j].StringG);
                    GetOrCreateColumn(rowJ, 11).SetCellValue(ResultList[i].DataList[j].StringH);
                    GetOrCreateColumn(rowJ, 12).SetCellValue(ResultList[i].DataList[j].StringI);
                    GetOrCreateColumn(rowJ, 13).SetCellValue(ResultList[i].DataList[j].StringJ);
                    GetOrCreateColumn(rowJ, 14).SetCellValue(ResultList[i].DataList[j].StringK);
                    GetOrCreateColumn(rowJ, 15).SetCellValue(ResultList[i].DataList[j].StringL);
                    GetOrCreateColumn(rowJ, 16).SetCellValue(ResultList[i].DataList[j].StringM);
                    GetOrCreateColumn(rowJ, 17).SetCellValue(ResultList[i].DataList[j].StringN);
                    GetOrCreateColumn(rowJ, 18).SetCellValue(ResultList[i].DataList[j].StringMaxT);
                    GetOrCreateColumn(rowJ, 19).SetCellValue(ResultList[i].DataList[j].StringMinT);
                    GetOrCreateColumn(rowJ, 20).SetCellValue(ResultList[i].DataList[j].StringAverageT);

                    GetOrCreateColumn(rowJ, 21).SetCellValue(ResultList[i].DataList[j].DeviceHumidity);
                    GetOrCreateColumn(rowJ, 22).SetCellValue(ResultList[i].DataList[j].StringYi);
                    GetOrCreateColumn(rowJ, 23).SetCellValue(ResultList[i].DataList[j].StringBing);
                    GetOrCreateColumn(rowJ, 24).SetCellValue(ResultList[i].DataList[j].StringDing);
                    GetOrCreateColumn(rowJ, 26).SetCellValue(ResultList[i].DataList[j].StringJia);
                    GetOrCreateColumn(rowJ, 27).SetCellValue(ResultList[i].DataList[j].StringMaxH);
                    GetOrCreateColumn(rowJ, 28).SetCellValue(ResultList[i].DataList[j].StringMinH);
                    GetOrCreateColumn(rowJ, 29).SetCellValue(ResultList[i].DataList[j].StringAverageH);
                }

                IRow rowAver = GetOrCreateRow(sheetSource, 4);
                IRow rowMax = GetOrCreateRow(sheetSource, 5);
                IRow rowMin = GetOrCreateRow(sheetSource, 6);
                IRow rowFluc = GetOrCreateRow(sheetSource, 7);
                for (int k = 1; k <= 15; k++)
                {
                    TestDataFluctuationValue temp = ResultList[i].FluctuationList[k.ToString()];
                    ICell cellAver = GetOrCreateColumn(rowAver, k + 2);
                    cellAver.SetCellValue(temp.AverageValue);
                    ICell cellMax = GetOrCreateColumn(rowMax, k + 2);
                    cellMax.SetCellValue(temp.MaxValue);
                    ICell cellMin = GetOrCreateColumn(rowMin, k + 2);
                    cellMin.SetCellValue(temp.MinValue);
                    ICell cellFluc = GetOrCreateColumn(rowFluc, k + 2);
                    cellFluc.SetCellValue(temp.FluctuationValue);
                }

                TestDataFluctuationValue tempA = ResultList[i].FluctuationList["A"];
                ICell cellAverA = GetOrCreateColumn(rowAver, 22);
                cellAverA.SetCellValue(tempA.AverageValue);
                ICell cellMaxA = GetOrCreateColumn(rowMax, 22);
                cellMaxA.SetCellValue(tempA.MaxValue);
                ICell cellMinA = GetOrCreateColumn(rowMin, 22);
                cellMinA.SetCellValue(tempA.MinValue);
                ICell cellFlucA = GetOrCreateColumn(rowFluc, 22);
                cellFlucA.SetCellValue(tempA.FluctuationValue);

                TestDataFluctuationValue tempB = ResultList[i].FluctuationList["B"];
                ICell cellAverB = GetOrCreateColumn(rowAver, 23);
                cellAverB.SetCellValue(tempB.AverageValue);
                ICell cellMaxB = GetOrCreateColumn(rowMax, 23);
                cellMaxB.SetCellValue(tempB.MaxValue);
                ICell cellMinB = GetOrCreateColumn(rowMin, 23);
                cellMinB.SetCellValue(tempB.MinValue);
                ICell cellFlucB = GetOrCreateColumn(rowFluc, 23);
                cellFlucB.SetCellValue(tempB.FluctuationValue);

                TestDataFluctuationValue tempC = ResultList[i].FluctuationList["C"];
                ICell cellAverC = GetOrCreateColumn(rowAver, 24);
                cellAverC.SetCellValue(tempC.AverageValue);
                ICell cellMaxC = GetOrCreateColumn(rowMax, 24);
                cellMaxC.SetCellValue(tempC.MaxValue);
                ICell cellMinC = GetOrCreateColumn(rowMin, 24);
                cellMinC.SetCellValue(tempC.MinValue);
                ICell cellFlucC = GetOrCreateColumn(rowFluc, 24);
                cellFlucC.SetCellValue(tempC.FluctuationValue);

                TestDataFluctuationValue tempD = ResultList[i].FluctuationList["D"];
                ICell cellAverD = GetOrCreateColumn(rowAver, 25);
                cellAverD.SetCellValue(tempD.AverageValue);
                ICell cellMaxD = GetOrCreateColumn(rowMax, 25);
                cellMaxD.SetCellValue(tempD.MaxValue);
                ICell cellMinD = GetOrCreateColumn(rowMin, 25);
                cellMinD.SetCellValue(tempD.MinValue);
                ICell cellFlucD = GetOrCreateColumn(rowFluc, 25);
                cellFlucD.SetCellValue(tempD.FluctuationValue);

                TestDataFluctuationValue tempO = ResultList[i].FluctuationList["O"];
                ICell cellAverO = GetOrCreateColumn(rowAver, 26);
                cellAverO.SetCellValue(tempO.AverageValue);
                ICell cellMaxO = GetOrCreateColumn(rowMax, 26);
                cellMaxO.SetCellValue(tempO.MaxValue);
                ICell cellMinO = GetOrCreateColumn(rowMin, 26);
                cellMinO.SetCellValue(tempO.MinValue);
                ICell cellFlucO = GetOrCreateColumn(rowFluc, 26);
                cellFlucO.SetCellValue(tempO.FluctuationValue);
            }


            return true;
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="p">值</param>
        private void SetCellValue(ICell cell, NPOIValue p)
        {
            object d = p.Value;
            if (d == null)
                d = p.Value = "---";
            switch (p.Value.GetType().ToString())
            {
                case "System.String":
                    cell.SetCellValue(d.ToString());
                    break;
                case "System.DateTime":
                    // cell.SetCellValue(DateTime.Parse(d.ToString()).ToShortDateString());
                    cell.SetCellValue(DateTime.Parse(d.ToString()).ToString());
                    break;
                case "System.Boolean":
                    bool boolV = false;
                    bool.TryParse(p.Value.ToString(), out boolV);
                    cell.SetCellValue(boolV);
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(p.Value.ToString(), out intV);
                    cell.SetCellValue(intV);
                    break;
                case "System.Decimal":
                case "System.Double":
                case "System.Single":

                    if ((double)d == double.NaN || ((double)d).ToString() == "非数字" || d.ToString() == "NaN")
                        cell.SetCellValue("---");

                    else
                    {
                        double doubV = 0;
                        double.TryParse(p.Value.ToString(), out doubV);
                        cell.SetCellValue(doubV);
                        if (p.RoundDigits != int.MinValue)
                        {
                            SetNumericCell(cell, p.RoundDigits);
                        }
                    }
                    break;
                case "System.DBNull":
                    cell.SetCellValue("");
                    break;
                default:
                    cell.SetCellValue(d.ToString());
                    break;
            }
        }

        /// <summary>
        /// 设置单元格的格式-数值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="digits"></param>
        public void SetNumericCell(ICell cell, int digits)
        {
            if (digits <= 0)
                return;

            string formatString = "0.".PadRight(digits + 2, '0');
            short index = HSSFDataFormat.GetBuiltinFormat(formatString);

            ICellStyle cellStyle = null;
            IDataFormat format = hssfworkbook.CreateDataFormat();
            cellStyle = hssfworkbook.CreateCellStyle();

            #region Copy Old Style
            cellStyle.Alignment = cell.CellStyle.Alignment;
            cellStyle.BorderBottom = cell.CellStyle.BorderBottom;
            cellStyle.BorderLeft = cell.CellStyle.BorderLeft;
            cellStyle.BorderRight = cell.CellStyle.BorderRight;
            cellStyle.BorderTop = cell.CellStyle.BorderTop;
            cellStyle.BottomBorderColor = cell.CellStyle.BottomBorderColor;
            cellStyle.FillBackgroundColor = cell.CellStyle.FillBackgroundColor;
            cellStyle.FillForegroundColor = cell.CellStyle.FillForegroundColor;
            cellStyle.FillPattern = cell.CellStyle.FillPattern;
            cellStyle.LeftBorderColor = cell.CellStyle.LeftBorderColor;
            cellStyle.RightBorderColor = cell.CellStyle.RightBorderColor;
            cellStyle.Rotation = cell.CellStyle.Rotation;
            cellStyle.TopBorderColor = cell.CellStyle.TopBorderColor;
            cellStyle.VerticalAlignment = cell.CellStyle.VerticalAlignment;
            #endregion

            if (index > 0)
            {
                cellStyle.DataFormat = index;
            }
            else
            {
                cellStyle.DataFormat = format.GetFormat(formatString);
            }
            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// 设置单元格的格式－日期
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="datatimeFormat"></param>
        public void SetDateCell(ICell cell, string datatimeFormat)
        {
            ICellStyle cellStyle = null;

            cellStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            cellStyle.DataFormat = format.GetFormat(datatimeFormat);

            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// 获取或创建工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public ISheet GetOrCreateSheet(string sheetName)
        {
            ISheet sheet = hssfworkbook.GetSheet(sheetName);
            if (sheet == null)
            {
                sheet = hssfworkbook.CreateSheet(sheetName);
            }
            return sheet;
        }

        /// <summary>
        /// 获取或创建行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public IRow GetOrCreateRow(ISheet sheet, int rowIndex)
        {
            IRow row = sheet.GetRow(rowIndex);
            if (row == null)
                row = sheet.CreateRow(rowIndex);
            return row;
        }

        /// <summary>
        /// 获取或创建列
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public ICell GetOrCreateColumn(IRow row, int colIndex)
        {
            ICell cell = row.GetCell(colIndex);
            if (cell == null)
                cell = row.CreateCell(colIndex);
            return cell;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void WriteToFile(string fileName, bool calc)
        {
            if (calc)
            {
                for (int sheetIndex = 0; sheetIndex < hssfworkbook.NumberOfSheets; sheetIndex++)
                    hssfworkbook.GetSheetAt(sheetIndex).ForceFormulaRecalculation = true;
            }
            FileStream file = new FileStream(fileName, FileMode.Create);
            hssfworkbook.Write(file);
            // file.Flush();//Excel2007时报错
            file.Close();
            file.Dispose();
        }

        /// <summary>
        /// 导出详细数据
        /// </summary>
        /// <param name="Info">参数</param>
        /// <param name="ResultList">数据</param>
        public void ExportDataDetail(TestInfo Info,ObservableCollection<TestDataResult> ResultList)
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
                    ReadFromExcelTemplate(@".\Template\template" + Info.PositionType + ".xls");
                    SetTestResultValue(Info, ResultList);

                    WriteToFile(save.FileName, true);
                    if(MessageBox.Show("导出成功！是否立即打开？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)== DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(save.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！\r\n" + ex.Message);
                }

            }
        }

    }

    /// <summary>
    /// NPOI Value
    /// </summary>
    public struct NPOIValue
    {
        public string SheetName;
        /// <summary>
        /// 行号
        /// </summary>
        public int Row;

        /// <summary>
        /// 列号
        /// </summary>
        public int Column;

        /// <summary>
        /// 值
        /// </summary>
        public object Value;

        /// <summary>
        /// 小数位数
        /// </summary>
        public int RoundDigits;

        public string Description;


        public NPOIValue(object value)
        {
            SheetName = string.Empty;
            Row = -1;
            Column = -1;
            Value = value;
            RoundDigits = int.MinValue;
            Description = string.Empty;
        }

        public NPOIValue(object value, int digits)
        {
            SheetName = string.Empty;
            Row = -1;
            Column = -1;
            Value = value;
            RoundDigits = digits;
            Description = string.Empty;
        }

        public NPOIValue(int row, int col, object value)
        {
            SheetName = string.Empty;
            Row = row;
            Column = col;
            Value = value;
            RoundDigits = int.MinValue;
            Description = string.Empty;
        }

        public NPOIValue(int row, int col, object value, int digits)
        {
            SheetName = string.Empty;
            Row = row;
            Column = col;
            Value = value;
            RoundDigits = digits;
            Description = string.Empty;
        }

        public NPOIValue(string sheetName, int row, int col, object value)
        {
            SheetName = sheetName;
            Row = row;
            Column = col;
            Value = value;
            RoundDigits = int.MinValue;
            Description = string.Empty;
        }

        public NPOIValue(string sheetName, int row, int col, object value, int digits)
        {
            SheetName = sheetName;
            Row = row;
            Column = col;
            Value = value;
            RoundDigits = digits;
            Description = string.Empty;
        }

        public NPOIValue(string sheetName, int row, int col, object value, int digits, string description)
        {
            SheetName = sheetName;
            Row = row;
            Column = col;
            Value = value;
            RoundDigits = digits;
            Description = description;
        }
    }
}
