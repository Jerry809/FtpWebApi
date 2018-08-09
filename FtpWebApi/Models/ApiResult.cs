using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FtpWebApi.Models
{
    public class BaseApiResult
    {
        public ResultCode Code { get; set; }
        public string Message { get; set; }

        public BaseApiResult()
        {
            Code = ResultCode.Success;
        }
    }

    public class ApiResult<T> : BaseApiResult
    {
        public T Result { get; set; }
    }

    public enum ResultCode
    {
        Success = 0,
        Error = -1,
        SystemError = 1000
    }
}