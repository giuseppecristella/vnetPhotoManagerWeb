using System;
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

            BindPrintFormats(userEmail);
            BindPrintFormatImageThumb(ddlPrintFormat.SelectedItem.Value);
            BindPaymentTypes(userDetail.StructureCode);
        }

        protected void btnCreateOrder_OnClick(object sender, EventArgs e)
        {
            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            var userEmail = Session["UserName"].ToString();
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

            var orderDetail = new OrderDetail
            {
                OrderId = orderId,
                CopyNumber = int.Parse(txtCopies.Text),
                FtpPhotoPath = Session["FtpPhotoPath"].ToString(),
                ProductId = int.Parse(ddlPrintFormat.SelectedItem.Value)
            };
            _orderRepository.SaveOrderDetail(orderDetail);
            Session["OrderNumber"] = orderNum.ToString();
            Response.Redirect("OrderSuccess.aspx");
        }

        protected void ddlPrintFormat_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = sender as DropDownList;
            if (selected == null) return;
            var selectedItem = selected.SelectedItem.Value;
            BindPrintFormatImageThumb(selectedItem);
        }

        #region Private Methods
        private void BindPaymentTypes(string structureCode)
        {
            ddlPayments.DataSource = _paymentMethodRepository.GetPaymentTypes(structureCode);
            ddlPayments.DataTextField = "Description";
            ddlPayments.DataValueField = "PaymentId";
            ddlPayments.DataBind();
        }

        private void BindPrintFormats(string userEmail)
        {
            var printFormats = _printFormatRepository.GetPhotoPrintFormats(userEmail);
            ddlPrintFormat.DataSource = printFormats;
            ddlPrintFormat.DataTextField = "Description";
            ddlPrintFormat.DataValueField = "ProductId";
            ddlPrintFormat.DataBind();
        }
        private void BindPrintFormatImageThumb(string selectedItem)
        {
            imgPrintFormat.ImageUrl = string.Format("DisplayImage.aspx?ProductId={0}", selectedItem);
        }
        #endregion


    }
}