using System;
using System.Web.UI;

namespace VnetPhotoManager.Web.Helpers
{
    public static class AppUtility
    {
        public static void RegisterStartUpScript(Page CurrrentPage, string ScriptKey, string Script)
        {
            ScriptManager.RegisterStartupScript(CurrrentPage, CurrrentPage.GetType(), ScriptKey, Script, true);
        }
    }
}