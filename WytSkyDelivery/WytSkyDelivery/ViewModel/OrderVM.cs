using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WytSkyDelivery.ViewModel
{
    public class OrderVM : BaseViewModel
    {
        #region privateFields
        
        private System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData> _ListData;
        private bool _IsVisibleConnectionError = false, _IsVisibleNoData = false, _IsVisibleData = true;

        #endregion

        #region Properties

        public System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData> ListData
        {
            get => _ListData;
            set => SetProperty(ref _ListData, value);
        }
        public bool IsVisibleConnectionError
        {
            get => _IsVisibleConnectionError;
            set => SetProperty(ref _IsVisibleConnectionError, value);
        }
        public bool IsVisibleNoData
        {
            get => _IsVisibleNoData;
            set => SetProperty(ref _IsVisibleNoData, value);
        }
        public bool IsVisibleData
        {
            get => _IsVisibleData;
            set => SetProperty(ref _IsVisibleData, value);
        }

        #endregion

        #region Command

        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand TryAgainCommand { get; private set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand<ModelDto.ListPageData> SelectedItemCommand { get; private set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand<ModelDto.ListPageData> SeeMoreCommand { get; private set; }

        #endregion

        #region Constructor

        public OrderVM()
        {
            try
            {
                TryAgainCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await  System.Threading.Tasks.Task.CompletedTask; GetData(); CanExcute = true; }, obj => CanExcute);
                SelectedItemCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand<ModelDto.ListPageData>(async (item) => { CanExcute = false; await SelectedItem(item); CanExcute = true; }, obj => CanExcute);
                SeeMoreCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand<ModelDto.ListPageData>(async (item) => { CanExcute = false; await SeeMore(item); CanExcute = true; }, obj => CanExcute);
                GetData();
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "OrderVM", "Constructor");
            }
        }

        #endregion

        #region Methods

        public async void GetData()
        {
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_ConnectionError);
                    return;
                }
                else
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading, null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        var res = await Api.ServiceApp.GetAll("QROrder");//new Dictionary<string, string>() { { "SkyuserName", UserName } }
                        if (res == null)
                        {
                            IsVisibleConnectionError = false;
                            IsVisibleData = false;
                            IsVisibleNoData = true;
                        }
                        else if (res != null && res.Count > 0)
                        {
                            ListData = res;
                            IsVisibleConnectionError = false;
                            IsVisibleData = true;
                            IsVisibleNoData = false;
                        }
                        else
                        {
                            IsVisibleConnectionError = false;
                            IsVisibleData = false;
                            IsVisibleNoData = false;
                        }
                    }
                }
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "OrderVM", "GetData");
            }
        }
        private async System.Threading.Tasks.ValueTask SelectedItem(ModelDto.ListPageData model)
        {
            try
            {
                string code = model.ListOfData.FirstOrDefault(_ => _.Key == "Code").Value;
                if (!string.IsNullOrEmpty(code))
                {
                    await App.Current.MainPage.Navigation.PushAsync(new Pages.OrderDetailsPage(code));
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "OrderVM", "SelectedItem");
            }
        }
        private async System.Threading.Tasks.ValueTask SeeMore(ModelDto.ListPageData model)
        {
            try
            {
                if(model.HeightRequest == -1)
                {
                    model.HeightRequest = 90;
                    model.TextSeeMore = Resources.Resource.Text_MoreDetails;
                }
                else
                {
                    model.HeightRequest = -1;
                    model.TextSeeMore = Resources.Resource.Text_LessDetails;
                }
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "OrderVM", "SelectedItem");
            }
        }
        
        
        #endregion
    }
}
