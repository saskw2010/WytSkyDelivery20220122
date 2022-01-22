using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.Helpers
{
    public class Toast
    {
        public static void ShowToast(CustomControl.PopupMessage.ToastPopupPage popup,int CloseAfterSecond)
        {
            try
            {
                Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
                if (CloseAfterSecond > 0)
                {
                    Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(CloseAfterSecond), () =>
                    {
                        try
                        {
                            // Do something
                            if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack != null
                            && Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Count > 0)
                            {
                                Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                            }
                            return false;
                        }
                        catch (System.Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
                            return false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToast");
            }
        }
        public static void ShowToastMessage(string title, string message,int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Message,
                    ToastMessage = message,
                    ToastTitle = title,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup,CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastMessage");
            }
        }
        public static void ShowToastSuccess(string title, string message,int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Success,
                    ToastMessage = message,
                    ToastTitle = title,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastSuccess");
            }
        }
        public static void ShowToastError(string title, string message,int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Error,
                    ToastMessage = message,
                    ToastTitle = title,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastError");
            }
        }
        public static void ShowToastWarning(string title, string message,int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Warning,
                    ToastMessage = message,
                    ToastTitle = title,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastWarning");
            }
        }
        public static void ShowToastMessage(string message, int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Message,
                    ToastMessage = message,
                    ToastTitle = Resources.Resource.Text_Message_,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastMessage");
            }
        }
        public static void ShowToastSuccess(string message, int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Success,
                    ToastMessage = message,
                    ToastTitle = Resources.Resource.Text_Success,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastSuccess");
            }
        }
        public static void ShowToastError(string message, int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Error,
                    ToastMessage = message,
                    ToastTitle = Resources.Resource.Text_Error,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastError");
            }
        }
        public static void ShowToastWarning(string message, int CloseAfterSecond = 5)
        {
            try
            {
                var popup = new CustomControl.PopupMessage.ToastPopupPage()
                {
                    ToastType = Enums.TypeOfToast.Warning,
                    ToastMessage = message,
                    ToastTitle = Resources.Resource.Text_Warning,
                    ImageSource = "Icon.png"
                };
                ShowToast(popup, CloseAfterSecond);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "Toast", "ShowToastWarning");
            }
        }
    }
}
