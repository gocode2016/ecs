using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace CaiPinKuAPI.Common
{
    public class ExcelInfo
    {
        public int TotalCount { get; set; }
        public IList<int> ErrorRows { get; set; }
        public DataTable Table { get; set; }
    }
    public class ExcelExtend
    {
        /// <summary>
        /// 以HTTP的形式创建Excel文件
        /// </summary>
        /// <param name="ds"></param>
        public static void CreateExcel(DataTable dt, string name)
        {
            var ms = CreateExcelToMS(dt, name);

            HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 创建Excel返回MemoryStream
        /// </summary>
        /// <param name="ds"></param>
        public static MemoryStream CreateExcelToMS(DataTable dt, string name)
        {
            HSSFWorkbook hssfworkbook;
            string filename = name + ".xls";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            HttpContext.Current.Response.Clear();

            InitializeWorkbook(out hssfworkbook);
            GenerateData(ref hssfworkbook, dt);

            return WriteToStream(hssfworkbook);
        }

        public static void CreateExcel<T>(List<T> list, string name)
        {
            CreateExcel(EntityListToDataTable(list), name);
        }

        /// <summary>
        /// 保存Excel
        /// </summary>
        public static void SaveExcel<T>(List<string> list, string path, string name)
        {
            HSSFWorkbook hssfworkbook;
            InitializeWorkbook(out hssfworkbook);

            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            for (int i = 0; i < list.Count; i++)
            {
                IRow row = sheet1.CreateRow(i);
                row.CreateCell(0).SetCellValue(list[i]);
            }
            var ms = WriteToStream(hssfworkbook);
            var uploadPath = System.Web.HttpRuntime.AppDomainAppPath + path.Replace("/", "\\");//System.Web.HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            FileStream dumpFile = new FileStream(uploadPath + name, FileMode.Create, FileAccess.ReadWrite);
            ms.WriteTo(dumpFile);

            ms.Dispose();
            dumpFile.Dispose();
        }

        /// <summary>
        /// 写入字符流
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <returns></returns>
        private static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="hssfworkbook"></param>
        private static void GenerateData(ref HSSFWorkbook hssfworkbook, DataTable dt)
        {
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");

            IRow row0 = sheet1.CreateRow(0);
            for (int z = 0; z < dt.Columns.Count; z++)
            {
                row0.CreateCell(z).SetCellValue(dt.Columns[z].ColumnName.ToString());
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cellValue = dt.Rows[i][j].ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        double cellDoubleVal;
                        if (double.TryParse(cellValue, out cellDoubleVal))
                        {
                            row.CreateCell(j).SetCellValue(cellDoubleVal);
                        }
                        else
                        {
                            row.CreateCell(j).SetCellValue(cellValue);
                        }
                    }
                    else
                    {
                        row.CreateCell(j).SetCellValue("");
                    }                    
                }
            }
        }

        /// <summary>
        /// 初始化工作薄
        /// </summary>
        /// <param name="hssfworkbook"></param>
        private static void InitializeWorkbook(out HSSFWorkbook hssfworkbook)
        {
            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "";
            hssfworkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "";
            hssfworkbook.SummaryInformation = si;
        }

        /// <summary>
        /// 泛型转换成DataTable
        /// </summary>
        public static DataTable EntityListToDataTable<T>(List<T> entityList)
        {
            DataTable dt = new DataTable();
            //取类型T所有Propertie
            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();
            Type colType = null;
            foreach (PropertyInfo propInfo in entityProperties)
            {
                string displayName = GetDisplayName(propInfo);

                if (propInfo.PropertyType.IsGenericType)
                {
                    colType = Nullable.GetUnderlyingType(propInfo.PropertyType);
                }
                else
                {
                    colType = propInfo.PropertyType;
                }
                if (colType.FullName.StartsWith("System") && !string.IsNullOrEmpty(displayName))
                {
                    dt.Columns.Add(displayName, colType);
                }
            }
            if (entityList != null && entityList.Count > 0)
            {
                foreach (T entity in entityList)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo propInfo in entityProperties)
                    {
                        string displayName = GetDisplayName(propInfo);

                        if (dt.Columns.Contains(displayName))
                        {
                            object objValue = propInfo.GetValue(entity, null);
                            newRow[displayName] = objValue == null ? DBNull.Value : objValue;
                        }
                    }
                    dt.Rows.Add(newRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取字段Display特性
        /// </summary>
        private static string GetDisplayName(PropertyInfo propInfo)
        {
            string displayName = string.Empty;
            foreach (var attr in propInfo.GetCustomAttributes(true))
            {
                DisplayAttribute displayAttr = attr as DisplayAttribute;

                if (displayAttr != null)
                {
                    displayName = displayAttr.Name;
                }
            }
            return displayName;
        }

        /// <summary> 
        /// 读取Excel文件到DataSet中 
        /// </summary> 
        /// <param name="filePath">文件路径</param>
        /// <returns></returns> 
        public static ExcelInfo ExcelToDataTable(string filePath)
        {
            ExcelInfo info = new ExcelInfo() { TotalCount = 0, ErrorRows = new List<int>(), Table = new DataTable() };
            DataTable dt = new DataTable();
            //HSSFWorkbook hssfworkbook;
            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //hssfworkbook = new HSSFWorkbook(file);
                try
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                catch (Exception)
                {
                    hssfworkbook = new XSSFWorkbook(filePath);
                }
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                info.TotalCount++;
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                if (row.Cells.Count >= cellCount)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    dt.Rows.Add(dataRow);
                }
                else
                {
                    info.ErrorRows.Add(i);
                }
            }
            info.Table = dt;
            return info;
        }

        /// <summary> 
        /// 读取Excel文件到DataSet中,包含空的cell
        /// </summary> 
        /// <param name="filePath">文件路径</param>
        /// <returns></returns> 
        public static ExcelInfo ExcelToDataTable2(string filePath)
        {
            ExcelInfo info = new ExcelInfo() { TotalCount = 0, ErrorRows = new List<int>(), Table = new DataTable() };
            DataTable dt = new DataTable();
            //HSSFWorkbook hssfworkbook;
            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //hssfworkbook = new HSSFWorkbook(file);
                try
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                catch (Exception)
                {
                    hssfworkbook = new XSSFWorkbook(filePath);
                }
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                info.TotalCount++;
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                //if (row.Cells.Count >= cellCount)
                //{
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    dt.Rows.Add(dataRow);
                //}
                //else
                //{
                //    info.ErrorRows.Add(i);
                //}
            }
            info.Table = dt;
            return info;
        }
    }
}
