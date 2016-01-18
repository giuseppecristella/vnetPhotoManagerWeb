using System.Configuration;

namespace VnetPhotoManager.Infrastructure
{
    public class AppConfiguration
    {
        public static string ConnectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
