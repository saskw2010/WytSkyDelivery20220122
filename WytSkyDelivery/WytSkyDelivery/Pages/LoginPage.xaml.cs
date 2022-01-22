using System;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : CustomControl.MyContentPage
    {
        public LoginPage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new ViewModel.LoginVM();
                this.FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "LoginPage", "Constructor");
            }
        }
    }
}