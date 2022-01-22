using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MunePage : FlyoutPage
    {
        public MunePage()
        {
            try
            {
                InitializeComponent();
                var lang = Helpers.Settings.Language;
                this.FlowDirection = (lang == "en") ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
                NavigationPage.SetHasNavigationBar(this, false);
                Detail = new NavigationPage(new Pages.QRScanPage());
                this.Flyout.BindingContext = new ViewModel.MenuVM();
                MenuItemsListView.ItemSelected += ListView_ItemSelected;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "MunePage", "Constructor");
            }
        }
        public MunePage(Page page)
        {
            try
            {
                InitializeComponent();
                var lang = Helpers.Settings.Language;
                this.FlowDirection = (lang == "en") ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
                NavigationPage.SetHasNavigationBar(this, false);
                //this.Detail = page;
                var _newPage = (Page)Activator.CreateInstance(page.GetType());
                _newPage.Title = page.Title;
                Detail = new NavigationPage(_newPage);
                this.Flyout.BindingContext = new ViewModel.MenuVM();
                MenuItemsListView.ItemSelected += ListView_ItemSelected;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "MunePage", "Constructor");
            }
        }
        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as ModelDto.MenuItem;
                if (item == null)
                {
                    return;
                }
                else if (item.TargetType == null && item.CommandE != null)
                {
                    await item.CommandE.ExecuteAsync();
                    return;
                }
                if (item.TargetType != ((Xamarin.Forms.NavigationPage)this.Detail).CurrentPage.GetType())
                {
                    var page = (Page)Activator.CreateInstance(item.TargetType);
                    page.Title = item.Title;
                    Detail = new NavigationPage(page);
                    IsPresented = false;
                    MenuItemsListView.SelectedItem = null;
                }
                else
                {
                    IsPresented = false;
                    MenuItemsListView.SelectedItem = null;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
            }
        }
        private void MasterDetailPage_IsPresentedChanged(object sender, EventArgs e)
        {
            try
            {
                ((ViewModel.MenuVM)this.Flyout.BindingContext).SetMune();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
            }
        }
    }
}