using FtpWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FtpWebApi.Helper
{
    public class FtpHelper
    {
        public async Task<List<string>> Query(FtpModel model)
        {
            string uri = "FTP://" + model.ServerIp + "/" + model.DirectoryPath;
            var ftp = (FtpWebRequest)WebRequest.Create(uri); //建立FTP連線            
            ftp.Credentials = new NetworkCredential(model.UserName, model.Passwrod); //帳密驗證
            ftp.Timeout = 2000; //等待時間
            ftp.UseBinary = true; //傳輸資料型別 二進位/文字
            ftp.Method = WebRequestMethods.Ftp.ListDirectory; //取得檔案清單

            var result = new List<string>();

            try
            {
                using (var sr = new StreamReader(ftp.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    while (!(sr.EndOfStream))
                        result.Add(await sr.ReadLineAsync());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Boolean> UploadFiles(UploadModel model)
        {
            string uri;
            int count = 0;

            foreach (var file in model.Files)
            {
                count++;
                uri = $"FTP://{model.ServerIp}/{model.DirectoryPath}/{file.FileName}";
                var ftp = (FtpWebRequest)WebRequest.Create(uri); //建立FTP連線
                ftp.Credentials = new NetworkCredential(model.UserName, model.Passwrod); //帳密驗證
                ftp.KeepAlive = count == model.Files.Count ? false : true; //關閉/保持 連線
                ftp.Timeout = 2000; //等待時間
                ftp.UseBinary = true; //傳輸資料型別 二進位/文字
                ftp.UsePassive = false; //通訊埠接聽並等待連接
                ftp.Method = WebRequestMethods.Ftp.UploadFile; //上傳檔案
                ftp.Proxy = null;

                try
                {
                    using (var request = ftp.GetRequestStream())
                    {
                        await request.WriteAsync(file.FileContents, 0, file.FileContents.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return true; ;
        }

        public async Task<Dictionary<string, byte[]>> DownloadFile(DownloadModel model)
        {
            string uri;
            int count = 0;
            var result = new Dictionary<string, byte[]>();

            foreach (var fileName in model.FileNames)
            {
                count++;
                uri = $"FTP://{model.ServerIp}/{model.DirectoryPath}/{fileName}";
                var ftp = (FtpWebRequest)WebRequest.Create(uri); //建立FTP連線
                ftp.Credentials = new NetworkCredential(model.UserName, model.Passwrod); //帳密驗證
                ftp.Timeout = 2000; //等待時間
                ftp.UseBinary = true; //傳輸資料型別 二進位/文字
                ftp.UsePassive = false; //通訊埠接聽並等待連接
                ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile; //下傳檔案
                ftp.KeepAlive = count == model.FileNames.Count ? false : true;

                try
                {
                    using (var response = (FtpWebResponse)ftp.GetResponse())
                    {
                        // //資料串流設為接收FTP回應下載
                        using (var responseStream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                var buffer = Encoding.UTF8.GetBytes(await reader.ReadToEndAsync());
                                result.Add(fileName, buffer);
                            }
                            //傳輸位元初始化
                            //byte[] buffer = new byte[8]; int iRead = 0;

                            //do
                            //{
                            //    iRead = await responseStream.ReadAsync(buffer, 0, buffer.Length); //接收資料串流
                            //} while (!(iRead == 0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        public Boolean DeleteFile(DeleteModel model)
        {
            string uri;
            int count = 0;

            foreach (var fileName in model.FileNames)
            {
                count++;
                uri = $"FTP://{model.ServerIp}/{model.DirectoryPath}/{fileName}";
                var ftp = (FtpWebRequest)WebRequest.Create(uri); //建立FTP連線
                ftp.Credentials = new NetworkCredential(model.UserName, model.Passwrod); //帳密驗證
                ftp.KeepAlive = false; //關閉/保持 連線
                ftp.Timeout = 2000; //等待時間
                ftp.Method = WebRequestMethods.Ftp.DeleteFile; //刪除檔案 

                try
                {
                    var myFtpResponse = (FtpWebResponse)ftp.GetResponse(); //刪除檔案/資料夾
                    myFtpResponse.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return true;
        }
    }
}