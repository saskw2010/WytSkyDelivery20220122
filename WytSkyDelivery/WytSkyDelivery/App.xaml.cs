using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                string languageName = Helpers.Settings.Language;
                WytSkyDelivery.Resources.Resource.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(languageName);
                System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat = new System.Globalization.CultureInfo("en").NumberFormat;
                System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat = new System.Globalization.CultureInfo("en").DateTimeFormat;
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    if (Helpers.Settings.Language == "ar")
                    {
                        (Xamarin.Forms.DependencyService.Get<Services.IIOSFlowDirection>()).SetLayoutRTL();
                    }
                    else
                    {
                        (Xamarin.Forms.DependencyService.Get<Services.IIOSFlowDirection>()).SetLayoutLTR();
                    }
                }
                //Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                if (Helpers.Settings.IsLogedin)
                {
                    MainPage = new NavigationPage(new Pages.MunePage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight
                    };
                }
                else
                {
                    MainPage = new NavigationPage(new Pages.WelcomePage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight
                    };
                }
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "App", "Constructor");
                MainPage = new NavigationPage(new Pages.WelcomePage())
                {
                    FlowDirection = Helpers.Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight
                };
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
