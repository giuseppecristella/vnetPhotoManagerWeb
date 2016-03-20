using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security.Provider;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Products : System.Web.UI.Page
    {
        private readonly PrintFormatRepository _productRepository;

        public Products()
        {
            _productRepository = new PrintFormatRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.IsPostBack) return;
            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            var userEmail = (string)Session["UserName"];
            BindProducts(userEmail);
        }

        protected void lvProducts_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var imgPhotoProduct = e.Item.FindControl("imgPhotoProduct") as Image;
                if (imgPhotoProduct == null) return;

                var hfProductId = e.Item.FindControl("hfProductId") as HiddenField;
                if (hfProductId == null) return;

                imgPhotoProduct.ImageUrl = string.Format("DisplayImage.aspx?ProductId={0}", hfProductId.Value);
            }
        }

        private void BindProducts(string userEmail)
        {
            if (string.IsNullOrEmpty(Request.QueryString["CatId"])) return;
            int catId;
            if (!int.TryParse(Request.QueryString["CatId"], out catId)) return;
            var products = _productRepository.GetPhotoPrintFormats(userEmail);
            if (!products.Any()) return;
            lvProducts.DataSource = products.Where(p => p.CategoryId.Equals(catId));
            lvProducts.DataBind();
        }

    }
}