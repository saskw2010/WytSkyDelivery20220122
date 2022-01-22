using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class UserLogin
    {
        public string name { get; set; }
        public string email { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string picture { get; set; }
        public ClaimDto claims { get; set; }
    }
}
