using System;
using System.Web;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class DisplayImage : System.Web.UI.Page
    {
        private readonly PrintFormatRepository _printFormatRepository;
        private readonly CategoryRepository _categoryRepository;
        public DisplayImage()
        {
            _categoryRepository = new CategoryRepository();
            _printFormatRepository = new PrintFormatRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(Request.QueryString["CategoryId"])) && (string.IsNullOrEmpty(Request.QueryString["ProductId"]))) return;
            byte[] image;
            if (!string.IsNullOrEmpty(Request.QueryString["CategoryId"]))
            {
                var catId = int.Parse(Request.QueryString["CategoryId"]);
                image = _categoryRepository.GetCategoryImage(catId);
            }
            else
            {
                var productId = int.Parse(Request.QueryString["ProductId"]);
                image = _printFormatRepository.GetProductImage(productId);
            }

            if (image != null)
            {
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment;filename="
                + "preview");
                Response.BinaryWrite(image);
                Response.Flush();
                Response.End();
            }
        }
    }
}