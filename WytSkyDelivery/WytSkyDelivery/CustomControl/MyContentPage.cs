using System;
using Xamarin.Forms;

namespace WytSkyDelivery.CustomControl
{
    public class MyContentPage : ContentPage
    {
        public event EventHandler<object> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, EventArgs.Empty);
        protected override bool OnBackButtonPressed()
        {
            try
            {
                ((ViewModel.BaseViewModel)this.BindingContext).GoBackCommand.Execute(null);
                return true;
            }
            catch(Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "MyContentPage", "OnBackButtonPressed");
            }
            return base.OnBackButtonPressed();
        }
    }
}
