namespace VnetPhotoManager.Web.ViewModels
{
    public class PhotoViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Format { get; set; }
        public int Copies { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string FtpPath { get; set; }
        public bool IsSavedPhoto { get; set; }
    }
}