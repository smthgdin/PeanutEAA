using System.Net.Http;
using System.Text;

using Newtonsoft.Json;


namespace Peanut.Web.API.DTO
{
    /// <summary>
    /// 构建返回值类
    /// 
    /// 目的：
    ///     1.构建返回值。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    internal class APIActionResultBuilder
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        bool Status { get; set; }

        /// <summary>
        /// 反馈信息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 附加数据（如果有异常或者错误会把相关信息附加在此处）
        /// </summary>
        object ExtraData { get; set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        string EncodingName { get; set; }

        /// <summary>
        /// 内容类型（媒体类型）
        /// </summary>
        string ContentType { get; set; }

        internal APIActionResultBuilder()
        {
            //设置默认值
            this.Status = true;
            this.Message = "处理成功";
            this.ExtraData = null;
            this.EncodingName = "UTF-8";
            this.ContentType = "application/json";
        }

        internal APIActionResultBuilder SetStatus(bool status)
        {
            this.Status = status;
            return this;
        }

        internal APIActionResultBuilder SetMessage(string message)
        {
            this.Message = message;
            return this;
        }

        internal APIActionResultBuilder SetExtraData(dynamic extraData)
        {
            this.ExtraData = extraData;
            return this;
        }

        internal APIActionResultBuilder SetEncoding(string encoding)
        {
            this.EncodingName = encoding;
            return this;
        }

        internal APIActionResultBuilder SetContentType(string contenttype)
        {
            this.ContentType = contenttype;
            return this;
        }

        internal HttpResponseMessage Build()
        {
            var actionResult = new APIActionResult
            {
                Status = this.Status,
                Message = this.Message,
                ExtraData = this.ExtraData
            };

            var jsonString = JsonConvert.SerializeObject(actionResult);

            return new HttpResponseMessage
            {
                //Provides HTTP content based on a string.Inheritance->bject->HttpContent->ByteArrayContent->StringContent
                Content = new StringContent(jsonString, Encoding.GetEncoding(EncodingName), this.ContentType)
            };
        }
    }
}