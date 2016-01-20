using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class OrderSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["OrderNumber"] == null) Response.Redirect("~/Account/Login.aspx");
            lblOrderNumber.Text = Session["OrderNumber"].ToString();
        }
    }
}