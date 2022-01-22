using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.Services
{
    public class RequestProvider
    {
        private static readonly ApiServices _requestProvider;

        public static ApiServices Current => _requestProvider;
        static RequestProvider()
        {
            _requestProvider = new ApiServices();
        }
    }
}
