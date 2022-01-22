using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRUserDTO : Base
    {
        public int? id { get; set; }
        public int? is_guest { get; set; }
        public bool? is_new_user { get; set; }
        public string full_name { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public string country_code { get; set; }
        public string primary_phone { get; set; }
        public string email { get; set; }
        public QRCountryDTO country { get; set; }
        public object coupon { get; set; }
    }
}
