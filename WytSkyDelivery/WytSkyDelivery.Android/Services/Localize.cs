using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WytSkyDelivery.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(WytSkyDelivery.Droid.Services.Localize))]
namespace WytSkyDelivery.Droid.Services
{
    public class Localize : ILocalize
    {
        [Obsolete]
        public void SetLocale(string Language)
        {
            if (!string.IsNullOrEmpty(Language))
            {
                if (Language == "en")
                {
                    (Forms.Context as MainActivity).Window.DecorView.LayoutDirection = Android.Views.View.LayoutDirectionLtr;
                }
                else
                {
                    //Thread.CurrentThread.CurrentCulture = new CultureInfo("ar");
                    //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");
                    (Forms.Context as MainActivity).Window.DecorView.LayoutDirection = Android.Views.View.LayoutDirectionRtl;
                }
            }
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Recreate();
        }
    }
}