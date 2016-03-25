using System;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.Account
{
    public partial class EventLogin : System.Web.UI.Page
    {
        private readonly CategoryRepository _categoryRepository;
        private static int _categoryAdminId;
        private static int _categoryId;
        private static string _password;
        public EventLogin()
        {
            _categoryRepository = new CategoryRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (!int.TryParse(Request.QueryString["EventId"], out _categoryAdminId)) Response.Redirect("~/Account/Login.aspx");
            var category = _categoryRepository.GetCategoryById(_categoryAdminId);
            _categoryId = category.CategoryId;
            _password = category.Password;
        }

        protected void OnClick(object sender, EventArgs e)
        {
            if (_password == null) return;
            if (txtPassword.Text.Equals(_password))
            {
                Session[string.Format("Event_{0}", _categoryAdminId)] = _password;
                Response.Redirect(string.Format("~/PhotoOrder/Products.aspx?CatId={0}&CatAdminId={1}", _categoryId, _categoryAdminId));
            }
        }
    }
}