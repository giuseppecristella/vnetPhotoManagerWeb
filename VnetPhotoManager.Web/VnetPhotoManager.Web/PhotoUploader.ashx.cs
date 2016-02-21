using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using VnetPhotoManager.Web.Helpers;

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

            string strImage;

            foreach (string s in context.Request.Files)
            {
                HttpPostedFile file = context.Request.Files[s];
                if (file == null) continue;

                if (string.IsNullOrEmpty(file.FileName)) continue;

                strImage = File.Exists(string.Format("{0}{1}", dirFullPath, file.FileName)) ? RenameUploadingImage(dirFullPath, file.FileName) : file.FileName;

                HttpContext.Current.Session["UploadedImage"] = SaveToFolder(file, strImage, numFiles, dirFullPath);

                var name = (string)HttpContext.Current.Session["UploadedImage"];
                var i = new ImageResizer.ImageJob(file, string.Format("~/PhotoOrder/Images/{0}_resized.<ext>", Path.GetFileNameWithoutExtension(name)), new ImageResizer.ResizeSettings(
                    "width=500;format=jpg;mode=max")) { CreateParentDirectory = true };
                //Auto-create the uploads directory.
                i.Build();
            }
            strImage = HttpContext.Current.Session["UploadedImage"] as string;
            context.Response.Write(strImage);
        }

        private string RenameUploadingImage(string path, string imageName)
        {
            var imageNameWithoutExt = Path.GetFileNameWithoutExtension(imageName);
            string[] alreadyRenamedFiles = Directory.GetFiles(path, string.Format("{0}(*", imageNameWithoutExt));
            if (!alreadyRenamedFiles.Any()) return string.Format("{0}({1}).jpg", imageNameWithoutExt, 1);
            var imageSavedIdx = alreadyRenamedFiles.Select(rf => Regex.Match(rf, @"\(([^)]*)\)").Groups[1].Value);
            var lastIdx = imageSavedIdx.Max(idx => int.Parse(idx));
            return string.Format("{0}({1}).jpg", imageNameWithoutExt, lastIdx + 1);
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