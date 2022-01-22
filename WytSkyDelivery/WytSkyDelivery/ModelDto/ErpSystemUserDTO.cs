using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ErpSystemUserDTO : Base
    {
        public string CompanyCode { get; set; }
        public string UserEmail { get; set; }
        public string AdminEmail { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public string DeviceID { get; set; }
        public Nullable<long> SystemId { get; set; }
    }
}
