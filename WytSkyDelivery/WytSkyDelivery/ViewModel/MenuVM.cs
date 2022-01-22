using WytSkyDelivery.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ViewModel
{
    public class MenuVM : BaseViewModel
    {
        #region privateFields

        private System.Collections.ObjectModel.ObservableCollection<ModelDto.MenuItem> _MenuItems;
        private bool _IsVisibleUser = false,_IsPresented = false, _IsVisibleExpert = false, _IsVisibleVip = false;
        private bool _IsLogedin = Helpers.Settings.IsLogedin;
        private string _name = "", _points = "", _ClientName = Helpers.Settings.ClientName;
        
        #endregion

        #region Properties
        public System.Collections.ObjectModel.ObservableCollection<ModelDto.MenuItem> MenuItems
        {
            get => _MenuItems;
            set => SetProperty(ref _MenuItems, value);
        }
        public bool IsVisibleUser
        {
            get => _IsVisibleUser;
            set => SetProperty(ref _IsVisibleUser, value);
        }
        public bool IsVisibleVip
        {
            get => _IsVisibleVip;
            set => SetProperty(ref _IsVisibleVip, value);
        }
        public bool IsVisibleExpert
        {
            get => _IsVisibleExpert;
            set => SetProperty(ref _IsVisibleExpert, value);
        }
        public bool IsPresented
        {
            get => _IsPresented;
            set => SetProperty(ref _IsPresented, value);
        }
        public bool IsVisibleStack
        {
            get => _IsLogedin;
            set => SetProperty(ref _IsLogedin, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string ClientName
        {
            get => _ClientName;
            set => SetProperty(ref _ClientName, value);
        }
        public string Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
        }
        #endregion

        #region Command
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand Command_ItemTapped { get; private set; }
        #endregion

        #region Constructor
        public MenuVM()
        {
            try
            {
                SetMune();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
            }
        }
        #endregion

        #region Methods
        
        private async System.Threading.Tasks.ValueTask ShearApp()
        {
            try
            {
                await Plugin.Share.CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage()
                {
                    Title = "WytSkyDelivery",
                    Text =
                    $"Google Play :{Environment.NewLine}https://play.google.com/store/apps/details {Environment.NewLine}" +
                    $"App Store   :{Environment.NewLine}https://itunes.apple.com ",
                });
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error : {ex.Message}");
            }
        }      
        private async System.Threading.Tasks.ValueTask CustomerLogout()
        {
            try
            {
                Helpers.Settings.IsLogedin = false;
                Helpers.Settings.Password = "";
                Helpers.Settings.AuthoToken = "";
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.WelcomePage())
                {
                    FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                };
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error : {ex.Message}");
            }
        }
        private async System.Threading.Tasks.ValueTask CustomerLogin()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new Pages.WelcomePage());
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error : {ex.Message}");
            }
        }
        public void SetMune()
        {
            try
            {
                if (Helpers.Settings.IsLogedin)
                {
                    MenuItems = new System.Collections.ObjectModel.ObservableCollection<ModelDto.MenuItem>(new[]
                    {
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_Home , TargetType = typeof(Pages.HomePage)},
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_ShareApp , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ShearApp(); CanExcute = true; }, obj => CanExcute)  , TargetType = null },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_Orders , TargetType = typeof(Pages.OrderPage) },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_ScanQR , TargetType = typeof(Pages.QRScanPage) },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_LogOut , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await CustomerLogout(); CanExcute = true; }, obj => CanExcute)  , TargetType = null },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_Language , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ChangeLanguage(); CanExcute = true; }, obj => CanExcute) ,TargetType = null},
                    });
                }
                else
                {
                    MenuItems = new System.Collections.ObjectModel.ObservableCollection<ModelDto.MenuItem>(new[]
                    {
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_Home , TargetType = typeof(Pages.HomePage)},
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_ShareApp , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ShearApp(); CanExcute = true; }, obj => CanExcute)  , TargetType = null },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_SignIn , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await CustomerLogin(); CanExcute = true; }, obj => CanExcute)  , TargetType = null },
                        new ModelDto.MenuItem { Icon="MenuIcon.png",Title = Resources.Resource.Text_Language , CommandE = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ChangeLanguage(); CanExcute = true; }, obj => CanExcute) ,TargetType = null},
                   
                    
                    });
                    ClientName = "Client";
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
            }
        }
        private async System.Threading.Tasks.ValueTask ChangeLanguage()
        {
            try
            {
                if (Helpers.Settings.Language == "ar")
                {
                    WytSkyDelivery.Resources.Resource.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    Helpers.Settings.Language = "en";
                }
                else
                {
                    WytSkyDelivery.Resources.Resource.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
                    Helpers.Settings.Language = "ar";
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
                var page = (Xamarin.Forms.NavigationPage)((Pages.MunePage)((Xamarin.Forms.NavigationPage)App.Current.MainPage).CurrentPage).Detail;
                page.Title = "";
                page.CurrentPage.Title = "";
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.MunePage(page.CurrentPage))
                {
                    FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                };
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
            }
        }
        
        #endregion

    }
}