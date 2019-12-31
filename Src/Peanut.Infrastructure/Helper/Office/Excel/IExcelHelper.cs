/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  IExcelHelper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/12/02 10:10:04 
 * 更新时间 :  2018/12/02 10:10:04 
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System.Collections.Generic;
using System.Data;

using NPOI.SS.UserModel;

namespace Peanut.Infrastructure.Office.Excel
{
    public interface IExcelHelper
    {
        /// <summary>
        /// Excel文件路径
        /// </summary>
        string FilePath { get; set; }

        IWorkbook CreateWorkbook(string filePath);

        /// <summary>
        /// 获取包含的工作表名
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSheetName();

        /// <summary>
        /// 获取Excel工作表个数
        /// </summary>
        int GetSheetCount();

        /// <summary>
        /// 获取Excel中对应工作表的行数
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        int GetSheetRowCount(string sheetName);

        /// <summary>
        /// 创建工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <returns></returns>
        bool CreateSheet(string sheetName);

        /// <summary>
        /// 创建工作表，传入列名，写在第一行
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="colNames">列名</param>
        /// <returns>true:创建工作表成功； false:创建工作表失败</returns>
        bool CreateSheet(string sheetName, string[] colNames);

        /// <summary>
        /// 移除工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <returns>true:移除工作表成功； false:移除工作表失败</returns>
        bool RemoveSheet(string sheetName);

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="data">数据</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <returns>true:写入工作表成功； false:写入工作表失败</returns>
        bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader);

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="data">数据</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <param name="beginRowIndex">开始行数</param>
        /// <param name="beginColIndex">开始列数</param>
        /// <returns>true:写入工作表成功； false:写入工作表失败</returns>
        bool WriteSheet(string sheetName, DataTable data, bool includeRowHeader, int beginRowIndex, int beginColIndex);

        /// <summary>
        /// 获取工作表的表结构
        /// </summary>
        /// <param name="sheetName">工作表</param>
        /// <returns>DataSet</returns>
        DataTable GetSheetSchema(string sheetName);

        /// <summary>
        /// 获取工作表数据
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="includeRowHeader">是否包含列名</param>
        /// <returns>DataSet</returns>
        DataTable GetSheet(string sheetName, bool includeRowHeader);

        /// <summary>
        /// 保存Excel文件到指定路径
        /// </summary>
        /// <param name="filePath">保存的路径</param>
        /// <returns>true:保存Excel成功； false:保存Excel失败</returns>
        bool Save(string filePath);

        /// <summary>
        /// 保存Excel，路径为加载时的路径
        /// </summary>
        /// <returns></returns>
        bool Save();

    }
}
