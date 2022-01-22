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
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WytSkyDelivery.Droid.Services.UniqueIdAndroid))]
namespace WytSkyDelivery.Droid.Services
{
    public class UniqueIdAndroid : WytSkyDelivery.Services.IDevice
    {
        public string GetIdentifier()
        {
            try
            {

                Android.Telephony.TelephonyManager mTelephonyMgr;
                mTelephonyMgr = (Android.Telephony.TelephonyManager)Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext.GetSystemService(Android.Content.Context.TelephonyService);
                return mTelephonyMgr.GetImei(0);
                //return Settings.Secure.GetString(Forms.Context.ContentResolver, Settings.Secure.AndroidId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erorr : {ex.Message}");
                return "";
            }
        }
    }
}