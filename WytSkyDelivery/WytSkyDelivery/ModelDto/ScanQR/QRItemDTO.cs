using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRItemDTO : Base
    {
        public int? id { get; set; }
        public object unit_type { get; set; }
        public object unit_value { get; set; }
        public string code { get; set; }
        public string image { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public double price { get; set; }
        public object discounted_price { get; set; }
        public int? expiry_date { get; set; }
        public int? quantity { get; set; }
        public int? created_at { get; set; }
        public int? quantity_in_cart { get; set; }
        public double? final_price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object discount_amount { get; set; }
        public QRCategoryDTO category { get; set; }
        public QRBrandDTO brand { get; set; }
        public QRProductTypeDTO product_type { get; set; }
        public List<QRImageDTO> images { get; set; }
    }
}
