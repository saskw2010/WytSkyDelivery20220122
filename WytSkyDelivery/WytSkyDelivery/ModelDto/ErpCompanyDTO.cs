using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ErpCompanyDTO : Base
    {
        public string CompanyCode { get; set; }
        public string CompanyUrl { get; set; }
        public string PathName { get; set; }
        public string UserAccount { get; set; }
        public string Admin_Email { get; set; }
        public Nullable<bool> CompanyHasHed { get; set; }
        public Nullable<long> CompanyID { get; set; }
    }
}
