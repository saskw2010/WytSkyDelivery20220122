using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace WytSkyDelivery.ViewModel
{
    public class OrderDetailsVM : BaseViewModel
    {
        #region privateFields

        private string _UserName = "", _Code = "";
        private bool _IsEnableOpen = false, _IsEnableEdit = false;
        private bool _IsVisibleConnectionError = false, _IsVisibleNoData = false, _IsVisibleData = true;
        private ModelDto.ScanQR.QROrderDTO _QROrder;
        private string orderId;

        #endregion

        #region Properties

        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
        }
        public string Code
        {
            get => _Code;
            set => SetProperty(ref _Code, value);
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
        public bool IsEnableOpen
        {
            get => _IsEnableOpen;
            set => SetProperty(ref _IsEnableOpen, value);
        }
        public bool IsEnableEdit
        {
            get => _IsEnableEdit;
            set => SetProperty(ref _IsEnableEdit, value);
        }
        public ModelDto.ScanQR.QROrderDTO QROrder
        {
            get => _QROrder;
            set => SetProperty(ref _QROrder, value);
        }

        #endregion

        #region Commands

        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand OpenLocationCommand { get; set; }

        #endregion

        #region Constructor

        public OrderDetailsVM(string orderId)
        {
            try
            {
                this.orderId = orderId;
                UserName = Helpers.Settings.UserName;
                OpenLocationCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await OpenLocationExt(); CanExcute = true; }, obj => CanExcute);
                GetData();
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "OrderDetailsVM", "Constructor");
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
                        var res = await Api.ServiceApp.OrderDetails(orderId);
                        if (res == null)
                        {
                            IsVisibleConnectionError = false;
                            IsVisibleData = false;
                            IsVisibleNoData = true;
                        }
                        else if (res != null)
                        {
                            QROrder = res;
                            saveData();
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
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "BaseViewVM", "GetData");
            }
        }

        public async void saveData()
        {
            try
            {
                var res = await Api.ServiceApp.GetAll("QROrder", 0, new Dictionary<string, string>() { { "code", orderId } });
                if (res == null || res.Count == 0)
                {
                    var resQRRegion = await Api.ServiceApp.SaveNew<object>("QRRegion", Api.ServiceApp.FromObjToDictionary(QROrder.location.region));
                    QROrder.location.regionid = QROrder.location.region.id;
                   
                    var resLocation = await Api.ServiceApp.SaveNew<object>("QRLocation", Api.ServiceApp.FromObjToDictionary(QROrder.location));
                    var resUser = await Api.ServiceApp.SaveNew<object>("QRUser", Api.ServiceApp.FromObjToDictionary(QROrder.user));
                    QROrder.locationid = QROrder.location.id;
                    QROrder.userid = QROrder.user.id;
                    QROrder.statusid = 1;
                    
                    QROrder.regionid = QROrder.location.region.id;
                    var resOrder = await Api.ServiceApp.SaveNew<object>("QROrder", Api.ServiceApp.FromObjToDictionary(QROrder));
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "BaseViewVM", "saveData");
            }
        }
        private async System.Threading.Tasks.ValueTask OpenLocationExt()
        {
            try
            {
               await Launcher.OpenAsync("https://www.google.com.eg/maps/dir//" + QROrder?.location?.lat + "," + QROrder?.location?.lon + "/@" + QROrder?.location?.lat + "," + QROrder?.location?.lon + ",14z");
                //Xamarin.Forms.Device.OpenUri(new Uri("https://www.google.com.eg/maps/dir//" + QROrder.location.lat + "," + QROrder.location.lon + "/@" + QROrder.location.lat + "," + QROrder.location.lon + ",14z"));
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "OpenLocationExt");
            }
        }

        #endregion

    }
}