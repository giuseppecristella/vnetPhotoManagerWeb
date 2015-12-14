using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;

namespace VnetPhotoManager.Web.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            //// Enable this once you have account confirmation enabled for password reset functionality
            ////ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
        }

        protected void LogIn(object sender, EventArgs e)
        {
            // var authservice = new vnetAuthServiceProxy();
            //  authservice.
            // authservice.Login(txtUsername.Text, txtPassword.Text,);
            bool loginResult;
            bool loginResultResponse;
            var authservice = new vnetauthenticationservice.AuthenticationService();
            var ret = authservice.CustomValidateUser(txtUsername.Text, txtPassword.Text);

            if (ret.Equals("ok"))
            {
                Response.Redirect("~/Home.aspx");
            }
            else
            {
                // error
            }
        }


    }
}