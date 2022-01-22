using System;
using Xamarin.Forms;

namespace WytSkyDelivery.ViewModel
{
    public class WelcomeVM : BaseViewModel
    {
        #region privateFields

        #endregion

        #region Properties

        #endregion

        #region Commands

        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand LoginCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand BrowseServicesCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand ChangeLanguageArCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand ChangeLanguageEnCommand { get; set; }

        #endregion

        #region Constructor
        public WelcomeVM()
        {
            try
            {
                LoginCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await System.Threading.Tasks.Task.CompletedTask; loginCommand(); CanExcute = true; }, obj => CanExcute);
                BrowseServicesCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await System.Threading.Tasks.Task.CompletedTask; BrowseServices(); CanExcute = true; }, obj => CanExcute);
                ChangeLanguageArCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await System.Threading.Tasks.Task.CompletedTask; ChangeLanguageAr(); CanExcute = true; }, obj => CanExcute);
                ChangeLanguageEnCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await System.Threading.Tasks.Task.CompletedTask; ChangeLanguageEn(); CanExcute = true; }, obj => CanExcute);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "Constructor");
            }
        }
        #endregion

        #region Methods
        private void loginCommand()
        {
            try
            {
                App.Current.MainPage.Navigation.PushAsync(new Pages.LoginPage());
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "loginCommand");
            }
        }
        private void ChangeLanguage(bool isEn)
        {
            try
            {
                if (Helpers.Settings.Language == "ar" && isEn)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    Helpers.Settings.Language = "en";
                }
                else if (Helpers.Settings.Language == "en" && !isEn)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
                    Helpers.Settings.Language = "ar";
                }
                else
                {
                    return;
                }
                System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat = new System.Globalization.CultureInfo("en").NumberFormat;
                System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat = new System.Globalization.CultureInfo("en").DateTimeFormat;
                (Xamarin.Forms.DependencyService.Get<Services.ILocalize>()).SetLocale(Helpers.Settings.Language);
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
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.WelcomePage())
                {
                    FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                };
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "ChangeLanguage");
            }
        }
        private void ChangeLanguageAr()
        {
            try
            {
                if (Helpers.Settings.Language != "ar")
                {
                    ChangeLanguage(false);
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "ChangeLanguage");
            }
        }
        private void ChangeLanguageEn()
        {
            try
            {
                if (Helpers.Settings.Language != "en")
                {
                    ChangeLanguage(true);
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "ChangeLanguage");
            }
        }
        private void BrowseServices()
        {
            try
            {
                Helpers.Settings.IsLogedin = false;
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.MunePage())
                {
                    FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                };
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "WelcomeVM", "loginCommand");
            }
        }
        #endregion
    }
}
