using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : CustomControl.MyContentPage
    {
        public HomePage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new ViewModel.HomeVM();
                this.FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "HomePage", "Constructor");
            }
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    try
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            var exit = await this.DisplayAlert(WytSkyDelivery.Resources.Resource.Text_CloseApp, WytSkyDelivery.Resources.Resource.Msg_CloseApp, WytSkyDelivery.Resources.Resource.Text_Close, WytSkyDelivery.Resources.Resource.Text_Cancle);
        //            if (exit)
        //            {
        //                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        //            }
        //        });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
        //        System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
        //    }
        //    return true;
        //}
    }
}