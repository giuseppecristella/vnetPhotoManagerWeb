using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.SessionState;

namespace VnetPhotoManager.Web
{
    /// <summary>
    /// Summary description for PhotoUploader
    /// </summary>
    public class PhotoUploader : IHttpHandler, IRequiresSessionState
    {
        private static string _ftpUrl = "46.228.255.118";
        private static string _ftpUser = "user1";
        private static string _ftpPassword = "utente12345";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var dirFullPath = HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/");
            var files = Directory.GetFiles(dirFullPath);
            var numFiles = files.Length;
            numFiles = numFiles + 1;

            var strImage = "";

            foreach (string s in context.Request.Files)
            {
                HttpPostedFile file = context.Request.Files[s];
                if (file == null) continue;

                if (string.IsNullOrEmpty(file.FileName)) continue;
                HttpContext.Current.Session["UploadedImage"] = SaveToFolder(file, strImage, numFiles, dirFullPath);
                //UploadFileToFtp(file);
            }
            strImage = HttpContext.Current.Session["UploadedImage"] as string;
            context.Response.Write(strImage);
        }


        private static void UploadFileToFtp(HttpPostedFile file)
        {
            try
            {

                var ftp = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _ftpUrl, "001/001", file.FileName))) as FtpWebRequest;
                if (ftp == null) return;
                ftp.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                byte[] buffer = new byte[file.InputStream.Length];
                file.InputStream.Read(buffer, 0, buffer.Length);
                file.InputStream.Close();

                var ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SaveToFolder(HttpPostedFile file, string strImage, int numFiles, string dirFullPath)
        {
            strImage = file.FileName;
            var imagesFolder = string.Format("{0}{1}", dirFullPath, strImage);
            file.SaveAs(imagesFolder);
            return strImage;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}