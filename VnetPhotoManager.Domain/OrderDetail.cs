using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VnetPhotoManager.Domain
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string FtpPhotoPath { get; set; }
        public int CopyNumber { get; set; }
    }
}
