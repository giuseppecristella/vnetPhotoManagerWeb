using System;
using System.Web.UI;

namespace VnetPhotoManager.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Login.aspx");
        }

        protected void OnClick(object sender, EventArgs e)
        {
            Response.Redirect("PhotoOrder/Add.aspx");
        }
    }
}