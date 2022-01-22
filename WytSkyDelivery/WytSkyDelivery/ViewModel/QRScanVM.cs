using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WytSkyDelivery.ViewModel
{
    public class QRScanVM : BaseViewModel
    {
        #region privateFields

        private string _UserName = "", _Code = "";
        private bool _IsEnableOpen = true, _IsEnableEdit = true;
        private ModelDto.ScanQR.QROrderDTO _QROrder;
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

        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand ScanQRCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand OpenOrderCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand updateOrderCommand { get; set; }

        #endregion

        #region Constructor

        public QRScanVM()
        {
            try
            {
                UserName = Helpers.Settings.UserName;
                ScanQRCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ScanQRExt(); CanExcute = true; }, obj => CanExcute);
                OpenOrderCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await OpenOrderExt(); CanExcute = true; }, obj => CanExcute);
                updateOrderCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await updateOrderExt(); CanExcute = true; }, obj => CanExcute);


            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "QRScanVM", "Constructor");
            }
        }

        #endregion

        #region Methods

        private bool chekCodeData()
        {
            try
            {
                if (string.IsNullOrEmpty(_Code))
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidOrgCode);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "QRScanVM", "update");
                return false;
            }
        }
        private async System.Threading.Tasks.ValueTask ScanQRExt()
        {
            try
            {
                #if __ANDROID__
	                // Initialize the scanner first so it can track the current context
	                MobileBarcodeScanner.Initialize (Application);
                #endif
                var hasPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();
                if (hasPermission != Xamarin.Essentials.PermissionStatus.Granted)
                {
                    return;
                }
                var scanner = new ZXing.Net.Mobile.Forms.ZXingScannerPage();
                await App.Current.MainPage.Navigation.PushAsync(scanner);
                scanner.OnScanResult += (result) =>
                {
                    if (result != null)
                    {
                        Code = result.Text;
                        Helpers.Toast.ShowToastSuccess(Resources.Resource.Text_OrderCode + " : " + Code);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread( async () =>
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new Pages.OrderDetailsPage(Code));
                        });
                    }
                };
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "ScanQRCommand");
            }
        }
        
       private async System.Threading.Tasks.ValueTask updateOrderExt()
        {
            try
            {
                if (chekCodeData())
                {
                    await updateData();
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "OpenOrderExt");
            }
        }
        public async  Task updateData()
        {
            try
            {
                var res = await Api.ServiceApp.GetAll("QROrder", 0, new Dictionary<string, string>() { { "code", Code } });
                if (res != null && res.Count > 0)
                {
                    QROrder.code = Code;
                    QROrder.statusid = 1;

                   
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
        private async System.Threading.Tasks.ValueTask OpenOrderExt()
        {
            try
            {
                if (chekCodeData())
                {
                    await App.Current.MainPage.Navigation.PushAsync(new Pages.OrderDetailsPage(Code));
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "OpenOrderExt");
            }
        }

        #endregion

    }
}