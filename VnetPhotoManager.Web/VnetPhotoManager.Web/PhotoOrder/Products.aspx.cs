using System;
using System.Linq;
using System.Web.UI.WebControls;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Products : System.Web.UI.Page
    {
        private readonly PrintFormatRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private static int _categoryId;
        public Products()
        {
            _productRepository = new PrintFormatRepository();
            _categoryRepository = new CategoryRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.IsPostBack) return;
            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            if (string.IsNullOrEmpty(Request.QueryString["CatId"])) return;

            if (!int.TryParse(Request.QueryString["CatId"], out _categoryId)) Response.Redirect("~/Account/Login.aspx");
            var userEmail = (string)Session["UserName"];
            int catIdAdmin;
            if (!int.TryParse(Request.QueryString["CatAdminId"], out catIdAdmin)) Response.Redirect("~/Account/Login.aspx");
            if (IsEvent(catIdAdmin) && !IsAuthorizedToEvent(catIdAdmin)) Response.Redirect(string.Format("~/Account/EventLogin.aspx?EventId={0}", catIdAdmin));
            BindProducts(userEmail);
        }

        private bool IsAuthorizedToEvent(int catAdminId)
        {
            var category = _categoryRepository.GetCategoryById(catAdminId);
            if (Session[string.Format("Event_{0}", catAdminId)] == null)
            return false;
            return Session[string.Format("Event_{0}", catAdminId)].ToString().Equals(category.Password);
        }

        private bool IsEvent(int catIdAdmin)
        {
            var category = _categoryRepository.GetCategoryById(catIdAdmin);
            return category != null && (category.IsEvent);
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
            var products = _productRepository.GetPhotoPrintFormats(userEmail);
            if (!products.Any()) return;
            lvProducts.DataSource = products.Where(p => p.CategoryId.Equals(_categoryId));
            lvProducts.DataBind();
        }
        private bool CheckEventPassword()
        {
            // ToDo: Hash password
            const string password = "test";
            return !string.IsNullOrEmpty((string)Session["EventPassword"]) && Session["EventPassword"].ToString().Equals(password);
        }
    }
}