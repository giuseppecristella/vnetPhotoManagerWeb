namespace VnetPhotoManager.Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public byte[] ImgThumb { get; set; }
        public bool IsEvent { get; set; }
        public string Password { get; set; }
        public int CategoryIdAdmin { get; set; }
    }
}
