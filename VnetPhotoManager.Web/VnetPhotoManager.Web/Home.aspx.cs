using System;

namespace VnetPhotoManager.Web
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClick(object sender, EventArgs e)
        {
            Response.Redirect("PhotoOrder/Categories");
        }
    }
}