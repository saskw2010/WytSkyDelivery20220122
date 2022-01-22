using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto.ScanQR
{
    public class QRLocationDTO : Base
    {
        public int id { get; set; }
        public bool is_default { get; set; }
        public string tag { get; set; }
        public string location_type { get; set; }
        public string building { get; set; }
        public string block { get; set; }
        public string avenue { get; set; }
        public string floor { get; set; }
        public string apartment { get; set; }
        public string street { get; set; }
        public string area { get; set; }
        public string nickname { get; set; }
        public string country_code { get; set; }
        public string mobile_number { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string notes { get; set; }
       
          public string deleted_at { get; set; }
         public string created_at { get; set; }
         public string city { get; set; }
        public QRRegionDTO region { get; set; }
        public object regionid { get; set; }




    }
}
