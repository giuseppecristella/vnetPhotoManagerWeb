using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VnetPhotoManager.Domain
{
    public class Order
    {
        public int PaymentId { get; set; }
        public string ClientCode { get; set; }
        public int OrderNumber { get; set; }
        public string Code { get; set; }
        public string SubCode { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Delivered { get; set; }
        public bool IdDelivered { get; set; }
        public string Note { get; set; }
    }
}
