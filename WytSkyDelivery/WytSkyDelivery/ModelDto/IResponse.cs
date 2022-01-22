using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class IResponse<T>
    {
        public string Content { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool IsPassed { get; set; }
    }
}
