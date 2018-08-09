using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FtpWebApi.Models
{
    public class FtpModel
    {
        public string ServerIp { get; set; }
        public string UserName { get; set; }
        public string Passwrod { get; set; }
        public string DirectoryPath { get; set; }
    }

    public class UploadModel:FtpModel
    {
        public List<FileModel> Files { get; set; }
    }

    public class DownloadModel : FtpModel
    {
       public List<string> FileNames { get; set; }
    }

    public class DeleteModel: FtpModel
    {
        public List<string> FileNames { get; set; }
    }

    public class FileModel
    {
        public string FileName { get; set; }
        public byte[] FileContents { get; set; }
    }
}