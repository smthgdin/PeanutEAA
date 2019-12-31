/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  ExcelNPOIHelper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/12/02 10:30:02
 * 更新时间 :  2018/12/02 10:30:02
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

// NPOI 2.0 for excel 2003
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
// for excel 2007 +
using NPOI.XSSF.UserModel;

using Peanut.Infrastructure.IO;

namespace Peanut.Infrastructure.Office.Excel
{
    /// <summary>
    /// 使用NPOI操作Excel
    /// </summary>
    internal class ExcelNPOIHelper : IExcelHelper
    {
        /*  1、NOPI 行列从0开始
         *  2、XSSFWorkbook 是 Excel 2007对象,HSSFWorkbook 是 Excel 2003对象,可以使用工厂 WorkbookFactory来读取
         *  3、Workbook里面很多linq方法没实现，遍历建议用 for循环
         *  4、如果删除了所有工作表保存的话，用office excel文件打开会提示没数据报错。
         *  5、设置单元格的值时，如果使用SetValue的重载的double时会自动设置SetCellType=CellType.Numeric
         *  
         */

        /// <summary>
        /// 当前操作的Excel对象
        /// </summary>
        private IWorkbook workBook;

        /// <summary>
        /// NPOI帮助类构造函数
        /// </summary>
        /// <param name="path"></param>
        public ExcelNPOIHelper(string path)
        {
            FilePath = path;

            InitWookBook(path);
        }

        #region interface

        /// <summary>
        /// 当前Excel的文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 获取工作表名称
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSheetName()
        {
            return GetAllSheetName(workBook);
        }

        /// <summary>
        /// 获取工作表数量
        /// </summary>
        /// <returns></returns>
        public int GetSheetCount()
        {
            return GetSheetCount(workBook);
        }

        /// <summary>
        /// 获取Excel中对应工作表的行数
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        public int GetSheetRowCount(string sheetName)
        {
            return GetRowCountBySheet(sheetName);
        }

        /// <summary>
        /// 创建工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <returns></returns>
        public bool CreateSheet(string sheetName)
        {
            return CreateSheetWithColName(sheetName);
        }

        /// <summary>
        /// 创建工作表，传入列名，写在第一行
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="colNames">列名</param>
        /// <returns>true:创建工作表成功； false:创建工作表失败</returns>
        public bool CreateSheet(string sheetName, string[] colNames)
        {
            return CreateSheetWithColName(sheetName, colNames);
        }

        /// <summary>
        /// 移除工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <returns>true:移除工作表成功； false:移除工作表失败</returns>
        public bool RemoveSheet(string sheetName)
        {
            return Remove(sheetName);
        }

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="data">数据</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <returns>true:写入工作表成功； false:写入工作表失败</returns>
        public bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader)
        {
            return WriteData(sheetName, data, includeRowHeader);
        }

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="data">数据</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <param name="beginRowIndex">开始行数</param>
        /// <param name="beginColIndex">开始列数</param>
        /// <returns>true:写入工作表成功； false:写入工作表失败</returns>
        public bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader, int beginRowIndex, int beginColIndex)
        {
            return WriteData(sheetName, data, includeRowHeader, beginRowIndex, beginColIndex);
        }

        /// <summary>
        /// 获取工作表的表结构
        /// </summary>
        /// <param name="sheetName">工作表</param>
        /// <returns>DataSet</returns>
        public DataTable GetSheetSchema(string sheetName)
        {
            return GetSchema(sheetName);
        }

        /// <summary>
        /// 获取工作表数据
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <returns>DataSet</returns>
        public DataTable GetSheet(string sheetName, bool includeRowHeader)
        {
            return GetData(sheetName, includeRowHeader);
        }

        /// <summary>
        /// 保存Excel文件到指定路径
        /// </summary>
        /// <param name="excelFilePath">保存的路径</param>
        /// <returns>true:保存Excel成功； false:保存Excel失败</returns>
        public bool Save(string excelFilePath)
        {
            return SaveFile(excelFilePath);
        }

        /// <summary>
        /// 保存Excel，路径为加载时的路径
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return SaveFile(FilePath);
        }

        #endregion

        #region private methods

        /// <summary>
        /// 初始化Excel对象,如果Excel不存在则创建新对象，如果存在则读取
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        private void InitWookBook(string path)
        {
            if (FileHelper.Exists(path) == false)
            {
                //创建空的Excel对象
                string ext = Path.GetExtension(path);
                if (!string.IsNullOrWhiteSpace(ext) && ext.Contains("xlsx"))
                    workBook = new XSSFWorkbook();
                else
                    workBook = new HSSFWorkbook();
            }
            else
            {
                //如果文件存在，则可以使用工厂来获取Excel对象
                workBook = WorkbookFactory.Create(path);
            }
        }

        /// <summary>
        /// 获取Excel所有工作表名
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private IEnumerable<string> GetAllSheetName(IWorkbook book)
        {
            NullCheck(book);
            List<string> sheetNames = new List<string>();

            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                sheetNames.Add(book.GetSheetAt(i).SheetName);
            }

            return sheetNames;
        }

        /// <summary>
        /// 获取工作表的数量
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private int GetSheetCount(IWorkbook book)
        {
            return NullCheck(book) ? book.NumberOfSheets : 0;
        }

        /// <summary>
        /// 获取工作表的行数
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private int GetRowCountBySheet(string sheetName)
        {
            NullCheck(workBook);
            ISheet sheet = workBook.GetSheet(sheetName);
            if (sheet == null)
            {
                throw new Exception(sheetName + " is not exists");
            }
            return GetRowCount(sheet);
        }

        /// <summary>
        /// 获取工作表的行数
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private int GetRowCount(ISheet sheet)
        {
            return sheet.PhysicalNumberOfRows;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private static bool NullCheck(IWorkbook book)
        {
            if (book == null)
                throw new ArgumentException("ExcelHelper Init Error");

            return true;
        }

        /// <summary>
        /// 创建Sheet，带列名，列名为空时，不写列名
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="colNames">列名</param>
        /// <returns></returns>
        private bool CreateSheetWithColName(string sheetName, string[] colNames = null)
        {
            NullCheck(workBook);
            ISheet sheet = workBook.CreateSheet(sheetName);
            if (colNames == null || colNames.Length <= 0)
            {
                //创建完即可返回
                return true;
            }

            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < colNames.Length; i++)
            {
                ICell cell = row.CreateCell(i, CellType.String);
                cell.SetCellValue(colNames[i]);
            }

            return true;
        }

        /// <summary>
        /// 写数据到Sheet
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="table">数据</param>
        /// <param name="includeRowHeader">是否包含列头</param>
        /// <param name="rowIndex">开始行号</param>
        /// <param name="colIndex">开始列号</param>
        /// <returns></returns>
        private bool WriteData(string sheetName, DataTable table, bool includeRowHeader, int rowIndex = 0, int colIndex = 0)
        {
            NullCheck(workBook);

            ISheet sheet = workBook.CreateSheet(sheetName);
            int beginRow = 0;

            if (includeRowHeader)
            {
                WriteSheetHeader(table, ref sheet, rowIndex, colIndex);
                beginRow = 1;
            }

            return WriteSheetData(table, sheet, beginRow, rowIndex, colIndex);

        }

        /// <summary>
        /// 写列头数据到Sheet
        /// </summary>
        /// <param name="table">数据</param>
        /// <param name="sheet">工作表名</param>
        /// <param name="rowIndex">开始行号</param>
        /// <param name="colIndex">开始列号</param>
        private static void WriteSheetHeader(DataTable table, ref ISheet sheet, int rowIndex = 0, int colIndex = 0)
        {
            IRow row = sheet.CreateRow(rowIndex);

            //将col改成col<= by xht
            for (int col = 0; col <= table.Columns.Count - 1; col++)
            {
                row.CreateCell(col + colIndex, CellType.String);
                string colName = table.Columns[col].ColumnName;
                row.Cells[col].SetCellValue(colName);
            }
        }

        /// <summary>
        /// 写数据到Sheet，不包含列头
        /// </summary>
        /// <param name="table">数据</param>
        /// <param name="sheet">工作表名</param>
        /// <param name="beginRowNum">从第几行开始</param>
        /// <param name="rowIndex">开始行号</param>
        /// <param name="colIndex">开始列号</param>
        /// <returns></returns>
        private bool WriteSheetData(DataTable table, ISheet sheet, int beginRowNum, int rowIndex = 0, int colIndex = 0)
        {
            try
            {
                beginRowNum = beginRowNum + rowIndex;
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    IRow row = sheet.CreateRow(beginRowNum++);
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        var value = table.Rows[r][c].ToString();
                        //创建单元格
                        ICell cell = row.CreateCell(c + colIndex);

                        double d;
                        //判断DataTable的数据类型
                        if (double.TryParse(value, out d))
                        {
                            cell.SetCellType(CellType.Numeric);
                            cell.SetCellValue(d);
                            continue;
                        }
                        cell.SetCellValue(value);

                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取Sheet的表结构
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private DataTable GetSchema(string sheetName)
        {
            NullCheck(workBook);

            ISheet sheet = workBook.GetSheet(sheetName);
            DataTable dt = new DataTable(sheet.SheetName);

            if (!GetSheetHeader(sheet, true, ref dt))
                throw new Exception("GetSchema");

            return dt;
        }

        /// <summary>
        /// 获取Sheet的数据
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="includeRowHeader">是否包含列头</param>
        /// <returns></returns>
        private DataTable GetData(string sheetName, bool includeRowHeader)
        {
            NullCheck(workBook);
            ISheet sheet = workBook.GetSheet(sheetName);
            DataTable dt = new DataTable(sheet.SheetName);

            int beginRow = 0;

            if (includeRowHeader)
                beginRow = 1;

            if (!GetSheetHeader(sheet, includeRowHeader, ref dt))
                throw new Exception("GetSchema");

            if (GetSheetData(sheet, beginRow, ref dt))
                return dt;

            return null;
        }

        /// <summary>
        /// 获取工作表的列头
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="includeRowHeader">是否包含列头信息</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        private bool GetSheetHeader(ISheet sheet, bool includeRowHeader, ref DataTable dt)
        {
            IRow headerRow = sheet.GetRow(0);

            for (int i = 0; i < headerRow.Cells.Count; i++)
            {
                if (!includeRowHeader)//如果不包含列名
                {
                    //如果不包含第一列名，则返回匿名列
                    const string colName = "第{0}列";
                    DataColumn dc = new DataColumn(string.Format(colName, i + 1), typeof(string));
                    dt.Columns.Add(dc);
                    continue;
                }
                else
                {
                    string colName = headerRow.GetCell(i).StringCellValue;
                    DataColumn dc = new DataColumn(colName);
                    dt.Columns.Add(dc);
                    continue;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取工作表的数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="beginRow">从第几行开始读</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        private bool GetSheetData(ISheet sheet, int beginRow, ref DataTable dt)
        {
            int colCount = sheet.GetRow(0).Cells.Count;

            for (int i = beginRow; i < sheet.PhysicalNumberOfRows; i++)
            {
                DataRow dr = dt.NewRow();

                for (int j = 0; j < colCount; j++)
                {
                    ICell cell = sheet.GetRow(i).GetCell(j);
                    if (cell == null)
                        continue;//过滤空格

                    //如果是数字或者函数就读取它的数值
                    if (cell.CellType == CellType.Numeric || cell.CellType == CellType.Formula)
                        dr[j] = cell.NumericCellValue;
                    else
                        dr[j] = cell.StringCellValue;
                }

                dt.Rows.Add(dr);
            }

            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        private bool SaveFile(string excelFilePath)
        {
            NullCheck(workBook);
            using (FileStream fs = new FileStream(excelFilePath, FileMode.Create))
            {
                workBook.Write(fs);
                fs.Dispose();
            }
            return true;
        }

        /// <summary>
        /// 移除指定工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private bool Remove(string sheetName)
        {
            NullCheck(workBook);

            var index = workBook.GetSheetIndex(sheetName);
            workBook.RemoveSheetAt(index);

            return workBook.GetSheetIndex(sheetName) != -1;    //待确定找不到是否会返回-1
        }

        private void Dispose()
        {
            if (workBook != null)
                workBook = null;
        }

        public IWorkbook CreateWorkbook(string filePath)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}

