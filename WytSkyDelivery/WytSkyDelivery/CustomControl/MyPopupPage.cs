using System;
using Xamarin.Forms;
namespace WytSkyDelivery.CustomControl
{
    public class MyPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public event EventHandler<object> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, EventArgs.Empty);
    }
}
