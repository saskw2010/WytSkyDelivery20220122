using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ResultApi<T>
    {
        public Boolean success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
