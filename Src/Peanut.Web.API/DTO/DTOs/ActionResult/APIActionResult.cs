using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peanut.Web.API.DTO
{
    /// <summary>
    /// 操作结果数据结构
    /// </summary>
    public class APIActionResult
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 反馈信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 附加数据（如果有异常或者错误会把相关信息附加在此处）
        /// </summary>
        public dynamic ExtraData { get; set;  }

    }
}