/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  ExcelOperateContext 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/12/02 10:15:29 
 * 更新时间 :  2018/12/02 10:15:29  
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System.Collections.Generic;
using System.Data;

using NPOI.SS.UserModel;

namespace Peanut.Infrastructure.Office.Excel
{
    public class ExcelOperateContext : IExcelHelper
    {
        IExcelHelper excelHelper;

        //public ExcelOperateContext(IExcelHelper helper)
        //{
        //    excelHelper = helper;
        //}

        //public ExcelOperateContext()
        //{
        //    读配置文件，得到默认设置
        //}

        public string FilePath { get; set; }

        /// <summary>
        /// 目的是为了支持可能对原有接口的扩展
        /// </summary>
        public IExcelHelper Current => excelHelper;

        /// <summary>
        /// 设置具体算法
        /// 通过接口编程+方法的注入的方式来保证程序的扩展性
        /// </summary>
        /// <param name="helper"></param>
        public void SetImplementStrategy(IExcelHelper helper)
        {
            excelHelper = helper;
        }

        public IWorkbook CreateWorkbook(string filePath)
        {
            return excelHelper.CreateWorkbook(filePath);
        }

        public bool CreateSheet(string sheetName)
        {
            return excelHelper.CreateSheet(sheetName);
        }

        public bool CreateSheet(string sheetName, string[] colNames)
        {
            return excelHelper.CreateSheet(sheetName, colNames);
        }

        public DataTable GetSheet(string sheetName, bool includeRowHeader)
        {
            return excelHelper.GetSheet(sheetName, includeRowHeader);
        }

        public int GetSheetCount()
        {
            return excelHelper.GetSheetCount();
        }

        public IEnumerable<string> GetSheetName()
        {
            return excelHelper.GetSheetName();
        }

        public int GetSheetRowCount(string sheetName)
        {
            return excelHelper.GetSheetRowCount(sheetName);
        }

        public DataTable GetSheetSchema(string sheetName)
        {
            return excelHelper.GetSheetSchema(sheetName);
        }

        public bool RemoveSheet(string sheetName)
        {
            return excelHelper.RemoveSheet(sheetName);
        }

        public bool Save(string filePath)
        {
            return excelHelper.Save(filePath);
        }

        public bool Save()
        {
            return excelHelper.Save();
        }

        public bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader)
        {
            return excelHelper.WriteSheet(sheetName, data, includeRowHeader);
        }

        public bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader, int beginRowIndex, int beginColIndex)
        {
            return excelHelper.WriteSheet(sheetName, data, includeRowHeader, beginRowIndex, beginColIndex);
        }
    }
}
