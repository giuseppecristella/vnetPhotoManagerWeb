using System;
using System.EnterpriseServices;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class CreateOrder : System.Web.UI.Page
    {
        private readonly OrderRepository _orderRepository;

        public CreateOrder()
        {
            _orderRepository = new OrderRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null) return;
            var userEmail = (string)Session["UserName"];
            var printFormats = _orderRepository.GetPhotoPrintFormats(userEmail);
            ddlPrintFormat.DataSource = printFormats;
            ddlPrintFormat.DataTextField = "Description";
            ddlPrintFormat.DataValueField = "Code";
            ddlPrintFormat.DataBind();

        }
    }
}