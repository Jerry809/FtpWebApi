using FtpWebApi.Helper;
using FtpWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FtpWebApi.Controllers
{
    [RoutePrefix("Ftp")]
    public class FtpController : ApiController
    {
        [HttpPost]
        [Route("Query")]
        public async Task<ApiResult<List<string>>> Query(FtpModel model)
        {
            var ftp = new FtpHelper();
            var result = new ApiResult<List<string>>();
            try
            {
                var query = await ftp.Query(model);
                result.Result = query;
            }
            catch (Exception ex)
            {
                result.Result = null;
                result.Code = ResultCode.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ApiResult<Boolean>> Upload(UploadModel model)
        {
            var ftp = new FtpHelper();
            var result = new ApiResult<Boolean>();
            try
            {
                var upload = await ftp.UploadFiles(model);
                result.Result = upload;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Code = ResultCode.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("Download")]
        public async Task<ApiResult<Dictionary<string, byte[]>>> Download(DownloadModel model)
        {
            var ftp = new FtpHelper();
            var result = new ApiResult<Dictionary<string, byte[]>>();
            try
            {
                var download = await ftp.DownloadFile(model);
                result.Result = download;
            }
            catch (Exception ex)
            {
                result.Result = null;
                result.Code = ResultCode.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("Delete")]
        public ApiResult<Boolean> Delete (DeleteModel model)
        {
            var ftp = new FtpHelper();
            var result = new ApiResult<Boolean>();
            try
            {
                var delete = ftp.DeleteFile(model);
                result.Result = delete;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Code = ResultCode.Error;
                result.Message = ex.Message;
            }

            return result;
        }     
    }
}
