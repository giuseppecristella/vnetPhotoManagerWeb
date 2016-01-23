using System;
using System.IO;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Web;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Add : System.Web.UI.Page
    {
        private static string _ftpUrl = "46.228.255.118";
        private static string _ftpUser = "user1";
        private static string _ftpPassword = "utente12345";
        private readonly UserDetailRepository _userDetailRepository;
        private static string _userFolder;


        public Add()
        {
            _userDetailRepository = new UserDetailRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
                var userEmail = (string)Session["UserName"];
                var userDetail = _userDetailRepository.GetUserDetail(userEmail);
                _userFolder = userDetail.StructureCode + "/" + userDetail.StructureCode + "/" + userDetail.UserName.Replace("@", "_");
            }
        }

        protected void btnCrop_Click(object sender, EventArgs e)
        {
            if (Session["UploadedImage"] == null) return;
            var imageName = Session["UploadedImage"].ToString();
            var w = string.IsNullOrEmpty(W.Value) ? 1004 : Convert.ToInt32(W.Value);
            var h = string.IsNullOrEmpty(H.Value) ? 768 : Convert.ToInt32(H.Value);
            var x = string.IsNullOrEmpty(X.Value) ? 1 : Convert.ToInt32(X.Value);
            var y = string.IsNullOrEmpty(Y.Value) ? 1 : Convert.ToInt32(Y.Value);

            var path = HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/");
            var cropImage = Crop(string.Format("{0}{1}", path, imageName), w, h, x, y);
            UploadFileToFtp(cropImage, imageName, _userFolder);
            SaveImage(cropImage, path, imageName);
            imgCropped.ImageUrl = string.Format("images/{0}", imageName);
            pnlCrop.Visible = true;
            btnOrder.Visible = true;
        }
        protected void btnOrder_OnClick(object sender, EventArgs e)
        {
            // Salvo in sessione la cartella ftp dove ho salvato il file
            Response.Redirect("CreateOrder.aspx");
        }

        #region Private Methods
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

        private void UploadFileToFtp(byte[] file, string filename, string userFolder)
        {
            try
            {

                var areCreatedNewFolder = CreateFtpFolder(userFolder);

                var ftp = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _ftpUrl, userFolder, filename))) as FtpWebRequest;
                if (ftp == null) return;
                Session["FtpPhotoPath"] = string.Format(@"ftp://{0}/{1}/{2}", _ftpUrl, userFolder, filename);
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

        private bool CreateFtpFolder(string folderName)
        {
            var folders = folderName.Split('/');
            var folderStructure = string.Empty;
            foreach (var f in folders)
            {
                folderStructure += f + "/";
                var ftp =
                    WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}", _ftpUrl, folderStructure))) as FtpWebRequest;
                ftp.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
                ftp.UsePassive = true;
                ftp.UseBinary = true;
                ftp.KeepAlive = false;

                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                try
                {
                    using (var resp = (FtpWebResponse)ftp.GetResponse())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //return false;
                }
            }
            return true;
        }


        #endregion


    }


}