using System.Security.AccessControl;

namespace VnetPhotoManager.Domain
{
    public class PaymentType
    {
        public int PaymentId { get; set; }
        public string Description { get; set; }
        public string PaypalCode { get; set; }
    }
}
