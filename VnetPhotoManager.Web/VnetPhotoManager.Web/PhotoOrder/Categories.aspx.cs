using System;
using System.Web.UI.WebControls;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class Categories : System.Web.UI.Page
    {
        private readonly CategoryRepository _categoryRepository;
        public Categories()
        {
            _categoryRepository = new CategoryRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (Session["UserName"] == null) Response.Redirect("~/Account/Login.aspx");
            var userEmail = (string)Session["UserName"];
            BindCategories(userEmail);
        }

        protected void lvCategories_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var imgPhotoCategory = e.Item.FindControl("imgPhotoCategory") as Image;
                if (imgPhotoCategory == null) return;

                var hfCategoryId = e.Item.FindControl("hfCategoryId") as HiddenField;
                if (hfCategoryId == null) return;

                imgPhotoCategory.ImageUrl = string.Format("DisplayImage.aspx?CategoryId={0}", hfCategoryId.Value);
            }
        }

        private void BindCategories(string userEmail)
        {
            var categories = _categoryRepository.GetCategories(userEmail);
            if (categories == null) return;
            lvCategories.DataSource = categories;
            lvCategories.DataBind();
        }
    }
}