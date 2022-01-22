using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRRegionDTO : Base
    {
        public int? id { get; set; }
        public int? created_at { get; set; }
        public string name { get; set; }
    }
}
