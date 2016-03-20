namespace VnetPhotoManager.Domain
{
    public class PrintFormat
    {
        public string Code { get; set; }
        public byte[] ImgThumb { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}
