using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRImageDTO : Base
    {
        public int? id { get; set; }
        public string meta_name { get; set; }
        public string media_name { get; set; }
        public int? height { get; set; }
        public int? width { get; set; }
        public int? created_at { get; set; }
    }
}
