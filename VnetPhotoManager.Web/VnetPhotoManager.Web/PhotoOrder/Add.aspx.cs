﻿using System;
using System.IO;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Web;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Add : System.Web.UI.Page
    {
        private static string _ftpUrl = "46.228.255.118";
        private static string _ftpUser = "user1";
        private static string _ftpPassword = "utente12345";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCrop_Click(object sender, EventArgs e)
        {
            if(Session["UploadedImage"] == null) return;
            var imageName = Session["UploadedImage"].ToString();
            var w = Convert.ToInt32(W.Value);
            var h = Convert.ToInt32(H.Value);
            var x = Convert.ToInt32(X.Value);
            var y = Convert.ToInt32(Y.Value);

            var path = HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/");
            var cropImage = Crop(string.Format("{0}{1}", path, imageName), w, h, x, y);
            UploadFileToFtp(cropImage, imageName);
            SaveImage(cropImage, path, imageName);
            imgCropped.ImageUrl = string.Format("images/{0}", imageName);
            pnlCrop.Visible = true;
        }

        private static void SaveImage(byte[] cropImage, string path, string imageName)
        {
            using (var ms = new MemoryStream(cropImage, 0, cropImage.Length))
            {
                ms.Write(cropImage, 0, cropImage.Length);
                using (var croppedImage = SD.Image.FromStream(ms, true))
                {
                    var saveTo = string.Format("{0}{1}", path, imageName);
                    croppedImage.Save(saveTo, croppedImage.RawFormat);
                }
            }
        }

        private static byte[] Crop(string imgPath, int width, int height, int x, int y)
        {
            try
            {
                using (var originalImage = SD.Image.FromFile(imgPath))
                {
                    using (var bmp = new SD.Bitmap(width, height))
                    {
                        bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                        using (var graphic = SD.Graphics.FromImage(bmp))
                        {
                            graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.DrawImage(originalImage, new SD.Rectangle(0, 0, width, height), x, y, width, height, SD.GraphicsUnit.Pixel);
                            var ms = new MemoryStream();
                            bmp.Save(ms, originalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                return null;
            }
        }

        private static void UploadFileToFtp(byte[] file, string filename)
        {
            try
            {
                var ftp = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _ftpUrl, "001/001", filename))) as FtpWebRequest;
                if (ftp == null) return;
                ftp.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                var ftpstream = ftp.GetRequestStream();
                ftpstream.Write(file, 0, file.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                // Log Exception
                throw ex;
            }
        }
    }
}