using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRCountryDTO : Base
    {
        public int? id { get; set; }
        public string capital { get; set; }
        public string email { get; set; }
        public int? visibility { get; set; }
        public string citizenship { get; set; }
        public string country_code { get; set; }
        public string currency { get; set; }
        public string currency_code { get; set; }
        public string currency_sub_unit { get; set; }
        public string currency_symbol { get; set; }
        public int? currency_decimals { get; set; }
        public string full_name { get; set; }
        public string iso_3166_2 { get; set; }
        public string iso_3166_3 { get; set; }
        public string name { get; set; }
        public string region_code { get; set; }
        public string sub_region_code { get; set; }
        public int? eea { get; set; }
        public string calling_code { get; set; }
        public string flag { get; set; }
    }
}
