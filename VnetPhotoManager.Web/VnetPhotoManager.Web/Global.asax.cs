using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;

namespace VnetPhotoManager.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //void Session_End(Object sender, EventArgs E)
        //{
        //    // Clean up session resources
        //    var test = Session["Photos"] as string;
        //}
        void Application_PreRequestHandlerExecute()
        {
            var page = Context.Handler as Page;
            if (page != null) page.Init += delegate
            {
                if (page.Form != null && !string.IsNullOrEmpty(page.Form.Action))
                {
                    page.Form.Action = string.Empty;
                }
            };
        }
    }
}