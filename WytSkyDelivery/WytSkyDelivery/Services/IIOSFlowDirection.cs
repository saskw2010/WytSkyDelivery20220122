using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.Services
{
    public interface IIOSFlowDirection
    {
        void SetLayoutRTL();
        void SetLayoutLTR();
    }
}
