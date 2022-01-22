using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using WytSkyDelivery.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(WytSkyDelivery.iOS.Services.Localize))]
namespace WytSkyDelivery.iOS.Services
{
    public class Localize : ILocalize
    {
        public void SetLocale(string Language)
        {
            var netLanguageLocale = GetIOSNetLanguage(Language);
            var ci = new System.Globalization.CultureInfo(netLanguageLocale);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
        private string GetIOSNetLanguage(string lang)
        {
            switch (lang)
            {
                case "eg":
                    return "EN-US";
                case "ar":
                    return "AR-EG";
                default:
                    return "EN-US";
            }
        }
    }
}