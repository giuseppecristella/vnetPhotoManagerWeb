using System;
using System.Web;
using VnetPhotoManager.Repository;

namespace VnetPhotoManager.Web.PhotoOrder
{
    public partial class DisplayImage : System.Web.UI.Page
    {
        private PrintFormatRepository _printFormatRepository;

        public DisplayImage()
        {
            _printFormatRepository = new PrintFormatRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductId"] == null) return;
            var productId = int.Parse(Request.QueryString["ProductId"]);

            byte[] productImg = _printFormatRepository.GetProductImage(productId);

            if (productImg != null)
            {
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment;filename="
                + "preview");
                Response.BinaryWrite(productImg);
                Response.Flush();
                Response.End();
            }
        }
    }
}