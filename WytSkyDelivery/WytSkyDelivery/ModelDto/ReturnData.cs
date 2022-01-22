using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ReturnData
    {
        public Nullable<int> rowsAffected { get; set; }
        public string clientScript { get; set; }
        public string Content { get; set; }
    }
}
