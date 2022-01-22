using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace WytSkyDelivery.ViewModel
{
    public class HomeVM : BaseViewModel
    {
        #region privateFields

        private Stack<ModelDto.ErpPages> _pages = new Stack<ModelDto.ErpPages>();
        private System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages> _ListData;
        private System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages> _UnfilterList;
        private bool _IsVisibleConnectionError = false, _IsVisibleNoData = false, _IsVisibleData = true;
        
        #endregion

        #region Properties

        public System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages> ListData
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
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand<ModelDto.ErpPages> SelectedItemCommand { get; private set; }

        #endregion

        #region Constructor

        public HomeVM()
        {
            try
            {
                TryAgainCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await  System.Threading.Tasks.Task.CompletedTask; GetData(); CanExcute = true; }, obj => CanExcute);
                SelectedItemCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand<ModelDto.ErpPages>(async (item) => { CanExcute = false; await SelectedItem(item); CanExcute = true; }, obj => CanExcute);
                GoBackCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await GoBack(); CanExcute = true; }, obj => CanExcute);
                GetData();
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "HomeVM", "Constructor");
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
                        var res = await Api.ServiceApp.GetAll<ModelDto.ErpPages>("ErpPages");
                        if (res == null)
                        {
                            IsVisibleConnectionError = false;
                            IsVisibleData = false;
                            IsVisibleNoData = true;
                        }
                        else if (res != null && res.Count > 0)
                        {
                            res.ForEach(x => x.PathName = x.PathName?.Replace("-", ""));
                            _UnfilterList = res;
                            ListData = new System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages>(res.Where(x => x.ParentID == null));
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
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "HomeVM", "GetData");
            }
        }
        private async System.Threading.Tasks.ValueTask SelectedItem(ModelDto.ErpPages model)
        {
            try
            {
                if (model.HasChild == true)
                {
                    _pages.Push(model);
                    ListData = new System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages>(_UnfilterList.Where(x => x.ParentID == model.PageID));
                }
                else
                {
                    await App.Current.MainPage.Navigation.PushAsync(new Pages.BaseViewPage(model));
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "HomeVM", "SelectedItem");
            }
        }
        private async System.Threading.Tasks.ValueTask GoBack()
        {
            try
            {
                if (_pages.Count > 0)
                {
                    _pages.Pop();
                    if (_pages.Count > 0)
                    {
                        var model = _pages.Peek();
                        ListData = new System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages>(_UnfilterList.Where(x => x.ParentID != null && x.ParentID == model.PageID));
                    }
                    else
                    {
                        ListData = new System.Collections.ObjectModel.ObservableCollection<ModelDto.ErpPages>(_UnfilterList.Where(x => x.ParentID == null));
                    }
                }
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "BaseViewVM", "SelectedItem");
            }
        }

        #endregion
    }
}
