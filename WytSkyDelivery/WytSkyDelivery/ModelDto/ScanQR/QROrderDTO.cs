using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QROrderDTO : Base
    {
        public string status { get; set; }
        public string payment_type { get; set; }
        public string code { get; set; }
        public string block { get; set; }
        public string avenue { get; set; }
        public string apartment { get; set; }
        public string floor { get; set; }
        public decimal delivery_charge { get; set; }
        public decimal discount_amount { get; set; }
        public decimal total_price { get; set; }
        public string currency { get; set; }
        public string notes { get; set; }
        public string order_date { get; set; }
        public string order_time { get; set; }
        public string created_at { get; set; }
        public string tracking_link { get; set; }
        public int? locationid { get; set; }
        public int? userid { get; set; }
        public int? statusid { get; set; }
      
        public int? regionid { get;  set; }
        public QRUserDTO user { get; set; }
        public List<QRItemDTO> items { get; set; }
        public QRLocationDTO location { get; set; }
    }
}
