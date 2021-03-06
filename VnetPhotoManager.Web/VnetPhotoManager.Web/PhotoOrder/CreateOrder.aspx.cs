﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using VnetPhotoManager.Domain;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class CreateOrder : System.Web.UI.Page
    {
        private readonly PrintFormatRepository _printFormatRepository;
        private readonly OrderRepository _orderRepository;
        private readonly PaymentMethodRepository _paymentMethodRepository;
        private readonly UserDetailRepository _userDetailRepository;

        private static string _ftpUrl = "46.228.255.118";
        private static string _ftpUser = "user1";
        private static string _ftpPassword = "utente12345";
        public static string FtpUserFolder { get; private set; }
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

        public CreateOrder()
        {
            _printFormatRepository = new PrintFormatRepository();
            _orderRepository = new OrderRepository();
            _paymentMethodRepository = new PaymentMethodRepository();
            _userDetailRepository = new UserDetailRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (Session["UserName"] == null) return;
            var userEmail = (string)Session["UserName"];
            var userDetail = _userDetailRepository.GetUserDetail(userEmail);
            if (userEmail != userDetail.UserName) return;
            FtpUserFolder = GetUserFolder(userDetail);
            //BindPrintFormats(userEmail);
            BindPhotosList();
            BindPaymentTypes(userDetail.StructureCode);
        }

        private string GetUserFolder(UserDetail userDetail)
        {
            return string.Format("{0}/{0}/{1}", userDetail.StructureCode, userDetail.UserName.Replace("@", "_"));
        }

        protected void btnCreateOrder_OnClick(object sender, EventArgs e)
        {
            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            var userEmail = Session["UserName"].ToString();

            var photosToUpload = Photos.Where(p => p.IsNewPhoto);
            var path = HttpContext.Current.Server.MapPath("~/PhotoOrder/Images/");
            foreach (var ph in photosToUpload)
            {
                byte[] buff = System.IO.File.ReadAllBytes(path + ph.Name);
                UploadFileToFtp(buff, ph.Name, FtpUserFolder);
            }

            var userDetail = _userDetailRepository.GetUserDetail(userEmail);
            var lastOrderNum = _orderRepository.GetLastOrder();
            var orderNum = (lastOrderNum == null) ? 1 : lastOrderNum.OrderNumber + 1;
            var order = new Order
            {
                PaymentId = int.Parse(ddlPayments.SelectedItem.Value),
                ClientCode = userDetail.UserName,
                Code = userDetail.StructureCode,
                Created = DateTime.Now,
                Delivered = DateTime.Now,
                Description = string.Format("Ordine numero {0} del {1}. Cliente {2} {3}", orderNum, DateTime.Now, userDetail.Name, userDetail.Surname),
                IdDelivered = false,
                Note = txtNotes.Text,
                OrderNumber = orderNum,
                SubCode = userDetail.StructureCode,
            };

            var orderId = _orderRepository.SaveOrder(order);

            foreach (var photo in Photos)
            {
                // TODO: Update To FTP
                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    CopyNumber = photo.Copies,
                    FtpPhotoPath = photo.FtpPath,
                    ProductId = photo.Format
                };
                _orderRepository.SaveOrderDetail(orderDetail);
            }
            Photos = null;
            Session["OrderNumber"] = orderNum.ToString();
            Response.Redirect("OrderSuccess.aspx");
        }

        #region Private Methods
        private void BindPaymentTypes(string structureCode)
        {
            ddlPayments.DataSource = _paymentMethodRepository.GetPaymentTypes(structureCode);
            ddlPayments.DataTextField = "Description";
            ddlPayments.DataValueField = "PaymentId";
            ddlPayments.DataBind();
        }

        private void BindPhotosList()
        {
            lvPhotos.DataSource = Photos;
            lvPhotos.DataBind();
            var lblTotal = lvPhotos.FindControl("lblTotal") as Label;
            if (lblTotal == null) return;
            lblTotal.Text = Photos.Sum(p => p.TotalPrice).ToString();
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