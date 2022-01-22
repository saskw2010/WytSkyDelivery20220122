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
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new ViewModel.WelcomeVM();
                this.FlowDirection = Helpers.Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomePage", "Constructor");
            }
        }
    }
}