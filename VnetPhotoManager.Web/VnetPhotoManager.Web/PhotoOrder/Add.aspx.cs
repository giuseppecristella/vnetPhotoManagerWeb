using System;
using System.Collections.Generic;
using System.IO;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using VnetPhotoManager.Domain;
using VnetPhotoManager.Repository;
using VnetPhotoManager.Web.Helpers;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Add : System.Web.UI.Page
    {
        private static string _ftpUrl = "46.228.255.118";
        private static string _ftpUser = "user1";
        private static string _ftpPassword = "utente12345";

        private readonly UserDetailRepository _userDetailRepository;
        private readonly PhotoOrderRepository _photoOrderRepository;
        private readonly PrintFormatRepository _printFormatRepository;

        private static PrintFormat _printFormat;
        private int productId;
        public static string FtpUserFolder { get; private set; }
        public static List<PrintFormat> PrintFormatsLookup { get; set; }

        public List<PhotoViewModel> Photos
        {
            get
            {
                if (Session["Photos"] == null)
                {
                    Session["Photos"] = new List<PhotoViewModel>();
                }
                return Session["Photos"] as List<PhotoViewModel>;
            }
            set
            {
                Session["Photos"] = value;
            }
        }

        public PhotoViewModel PhotoToAdd
        {
            get
            {
                return Session["PhotoToAdd"] as PhotoViewModel;
            }
            set
            {
                Session["PhotoToAdd"] = value;
            }
        }
        

        #region Ctor
        public Add()
        {
            _userDetailRepository = new UserDetailRepository();
            _printFormatRepository = new PrintFormatRepository();
            _photoOrderRepository = new PhotoOrderRepository();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (string.IsNullOrEmpty(Request.QueryString["CatId"]) && string.IsNullOrEmpty(Request.QueryString["ProdId"])) return;
            if (!CheckQueryStrings()) return;

            PhotoToAdd = null;

            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            var userEmail = (string)Session["UserName"];
            var userDetail = _userDetailRepository.GetUserDetail(userEmail);
            FtpUserFolder = GetUserFolder(userDetail);
            _printFormat = GetProductInfo(int.Parse(Request.QueryString["ProdId"]), userEmail);
            BindPhotosAlredyOrderered();
            if (Photos.Any()) BindPhotosCart();
        }
        protected void btnCrop_Click(object sender, EventArgs e)
        {
            if (Session["UploadedImage"] == null) return;
            var imageName = Session["UploadedImage"].ToString();
            var path = HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/");
            byte[] cropImage;
            var resizedWidth = 1024;
            using (var img = System.Drawing.Image.FromFile(string.Format("{0}{1}", path, imageName)))
            {
                // Resize, e preview nella finestra modale
                var w = W.Value.Equals("NaN") || string.IsNullOrEmpty(W.Value) ? img.Width : Convert.ToInt32(W.Value);
                var h = H.Value.Equals("NaN") || string.IsNullOrEmpty(H.Value) ? img.Height : Convert.ToInt32(H.Value);
                var x = X.Value.Equals("NaN") || string.IsNullOrEmpty(X.Value) ? 1 : Convert.ToInt32(X.Value);
                var y = Y.Value.Equals("NaN") || string.IsNullOrEmpty(Y.Value) ? 1 : Convert.ToInt32(Y.Value);

                x = (int)Math.Floor((decimal)(x * ((float)img.Width / resizedWidth)));
                y = (int)Math.Floor((decimal)(y * ((float)img.Width / resizedWidth)));
                w = (int)Math.Floor((decimal)(w * ((float)img.Width / resizedWidth)));
                h = (int)Math.Floor((decimal)(h * ((float)img.Width / resizedWidth)));

                if (x > img.Width) x = img.Width;
                if (w > img.Width) x = img.Width;
                if (y > img.Height) y = img.Height;
                if (h > img.Height) y = img.Height;

                // Calcolo percentuale di ridimensionamento e adeguo le coordinate di ritaglio
                cropImage = Crop(string.Format("{0}{1}", path, imageName), w, h, x, y);
                //Path.GetFileNameWithoutExtension(imageName)

                // Cancello la foto resized perchè mi serve solo per l'anteprima
                File.Delete(string.Format("{0}{1}", path, string.Format("{0}_resized.jpg", Path.GetFileNameWithoutExtension(imageName))));
            }
            // Salvo l'immagine ritagliata nella cartella del web server
            // e la cancello dalla root
            File.Delete(HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/" + imageName));

            SaveImage(cropImage, path, imageName);
            //UploadFileToFtp(cropImage, imageName, FtpUserFolder);

            imgCropped.ImageUrl = string.Format("images/{0}", imageName);
            //pnlCrop.Visible = true;
            btnOrder.Visible = true;
            PhotoToAdd = new PhotoViewModel
            {
                Name = imageName,
                Path = string.Format("images/{0}", imageName),
                Format = _printFormat.ProductId,
                FormatDescription = _printFormat.Description,
                FtpPath = string.Format(@"ftp://{0}/{1}/{2}", _ftpUrl, FtpUserFolder, imageName),
                UnitPrice = (decimal) _printFormat.Price
            };
            Photos.Add(PhotoToAdd);
            lvPhotos.DataSource = Photos;
            lvPhotos.DataBind();
            imgCropped.ImageUrl = string.Empty;

        }
        protected void btnOrder_OnClick(object sender, EventArgs e)
        {
            Photos.Clear();
            foreach (var item in lvPhotos.Items)
            {
                BindItemToPhotoVM(item);
            }
            Response.Redirect("CreateOrder.aspx");
        }
        protected void btnAddSavedPhotoToGrid_OnClick(object sender, EventArgs e)
        {
            foreach (ListViewItem row in lvSavedPhotos.Items)
            {
                var cbSelectSavedPhoto = row.FindControl("cbSelectSavedPhoto") as CheckBox;
                if (cbSelectSavedPhoto == null) continue;
                if (!cbSelectSavedPhoto.Checked) continue;

                var hfFtpPath = row.FindControl("hfFtpPath") as HiddenField;
                if (hfFtpPath == null) continue;
                var lblName = row.FindControl("lblName") as Label;
                if (lblName == null) continue;
                PhotoToAdd = new PhotoViewModel
                {
                    Name = lblName.Text,
                    Path = string.Format("images/{0}", lblName.Text),
                    FtpPath = hfFtpPath.Value,
                    Format = _printFormat.ProductId,
                    FormatDescription = _printFormat.Description,
                    UnitPrice = (decimal)_printFormat.Price
                };
                Photos.Add(PhotoToAdd);
            }
            lvPhotos.DataSource = Photos;
            lvPhotos.DataBind();
        }
        //protected void ddlPrintFormat_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    var item = (ListViewDataItem)ddl.NamingContainer;
        //    string selectedValue = ((DropDownList)(item.FindControl("ddlPrintFormats"))).SelectedValue;

        //    var lblPrice = item.FindControl("lblPrice") as Label;
        //    if (lblPrice == null) return;
        //    lblPrice.Text = _printFormat.Price.ToString();

        //    var txtCopies = item.FindControl("txtCopies") as TextBox;
        //    if (txtCopies == null) return;

        //    //var lblTotPrice = item.FindControl("lblTotPrice") as Label;
        //    //if (lblTotPrice == null) return;
        //    //var total = SetItemPrice(int.Parse(ddl.SelectedItem.Value)) * int.Parse(txtCopies.Text);
        //    //lblTotPrice.Text = total.ToString();
        //}
        protected void lvPhotos_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //var ddlPrintFormats = e.Item.FindControl("ddlPrintFormats") as DropDownList;
                //if (ddlPrintFormats == null) return;
                //ddlPrintFormats.DataSource = PrintFormatsLookup;
                //ddlPrintFormats.DataTextField = "Description";
                //ddlPrintFormats.DataValueField = "ProductId";
                //ddlPrintFormats.DataBind();

                //var lblPrintFormat = e.Item.FindControl("lblPrintFormat") as Label;
                //if (lblPrintFormat == null) return;
                //lblPrintFormat.Text = _printFormat.Description;

                //var lblPrice = e.Item.FindControl("lblPrice") as Label;
                //if (lblPrice == null) return;
                //lblPrice.Text = _printFormat.Price.ToString();

                var txtCopies = e.Item.FindControl("txtCopies") as TextBox;
                if (txtCopies == null) return;

                //var lblTotPrice = e.Item.FindControl("lblTotPrice") as Label;
                //if (lblTotPrice == null) return;
                //var total = SetItemPrice(int.Parse(ddlPrintFormats.SelectedItem.Value)) * int.Parse(txtCopies.Text);
                //lblTotPrice.Text = total.ToString();
            }
        }
        protected void lvPhotos_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (!String.Equals(e.CommandName, "FormatPrieview")) return;
            // var dataItem = (ListViewDataItem) e.Item;
            var ddlPrintFormats = e.Item.FindControl("ddlPrintFormats") as DropDownList;
            if (ddlPrintFormats == null) return;
            var selectedItem = ddlPrintFormats.SelectedItem.Value;
            imgPrintFormat.ImageUrl = string.Format("DisplayImage.aspx?ProductId={0}", selectedItem);
            AppUtility.RegisterStartUpScript(Page, "ShowModal", "openFormatPrieviewModal()");
        }
        #endregion

        #region Bindings
        private void BindItemToPhotoVM(ListViewDataItem item)
        {
            var imgPhoto = item.FindControl("imgPhoto") as Image;
            if (imgPhoto == null) return;
            var lblName = item.FindControl("lblName") as Label;
            if (lblName == null) return;
            var ddlPrintFormats = item.FindControl("ddlPrintFormats") as DropDownList;
            if (ddlPrintFormats == null) return;
            var txtCopies = item.FindControl("txtCopies") as TextBox;
            if (txtCopies == null) return;
            var lblPrice = item.FindControl("lblPrice") as Label;
            if (lblPrice == null) return;
            var hfFtpPath = item.FindControl("hfFtpPath") as HiddenField;
            if (hfFtpPath == null) return;

            Photos.Add(new PhotoViewModel
            {
                Name = lblName.Text,
                Path = string.Format("images/{0}", lblName.Text),
                Format = int.Parse(ddlPrintFormats.SelectedItem.Value),
                Copies = int.Parse(txtCopies.Text),
                UnitPrice = decimal.Parse(lblPrice.Text),
                TotalPrice = decimal.Parse(lblPrice.Text) * int.Parse(txtCopies.Text),
                FtpPath = hfFtpPath.Value
            });
        }
        private void BindPhotosCart()
        {
            lvPhotos.DataSource = Photos;
            lvPhotos.DataBind();
        }

        #endregion

        #region Private Methods
        private PrintFormat GetProductInfo(int productId, string userEmail)
        {
            return _printFormatRepository.GetPhotoPrintFormats(userEmail).FirstOrDefault(pf => pf.ProductId.Equals(productId));
        }

        private bool CheckQueryStrings()
        {
            int prodId;
            return int.TryParse(Request.QueryString["ProdId"], out prodId) && int.TryParse(Request.QueryString["CatId"], out prodId);
        }

        private static string GetUserFolder(UserDetail userDetail)
        {
            return string.Format("{0}/{0}/{1}", userDetail.StructureCode, userDetail.UserName.Replace("@", "_"));
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

        private void BindPhotosAlredyOrderered()
        {
            if (Session["UserName"] == null) return;// Redirect to home
            lvSavedPhotos.DataSource = _photoOrderRepository.GetAll((string)Session["UserName"]);
            lvSavedPhotos.DataBind();
        }
        #endregion

    }

    public class PhotoViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Format { get; set; }
        public int Copies { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string FtpPath { get; set; }
        public string FormatDescription { get; set; }
    }
}