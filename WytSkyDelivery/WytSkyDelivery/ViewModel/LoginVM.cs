using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ViewModel
{
    public class LoginVM : BaseViewModel
    {
        #region privateFields
        
        private string _UserName = "", _Password = "", _Code = "", _CompnyName = "";
        private bool _IsVisibleLogin = true, _IsVisibleCode = false;

        #endregion

        #region Properties

        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
        }
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }
        public string Code
        {
            get => _Code;
            set => SetProperty(ref _Code, value);
        }
        public string CompnyName
        {
            get => _CompnyName;
            set => SetProperty(ref _CompnyName, value);
        }
        public bool IsVisibleLogin
        {
            get => _IsVisibleLogin;
            set => SetProperty(ref _IsVisibleLogin, value);
        }
        public bool IsVisibleCode
        {
            get => _IsVisibleCode;
            set => SetProperty(ref _IsVisibleCode, value);
        }

        #endregion

        #region Commands
        
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand LoginCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand UseCodeCommand { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand ChangeCompanyCommand { get; set; }

        #endregion

        #region Constructor

        public LoginVM()
        {
            try
            {
                //if (Helpers.Settings.IsFeristLogedin)
                //{
                //    IsVisibleCode = true;
                //    IsVisibleLogin = false;
                //    Helpers.Settings.BaseAddress = "http://Khabir.saskw.net";
                //}
                //else
                //{
                //    IsVisibleCode = false;
                //    IsVisibleLogin = true;
                //}
                //Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                LoginCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await loginExt(); CanExcute = true; }, obj => CanExcute);
                UseCodeCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await UseCodeExt(); CanExcute = true; }, obj => CanExcute);
                ChangeCompanyCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await ChangeCompanyExt(); CanExcute = true; }, obj => CanExcute);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "LoginVM", "Constructor");
            }
        }

        #endregion

        #region Methods

        private bool chekLogInData()
        {
            try
            {
                if (string.IsNullOrEmpty(_UserName))
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidUserName);
                    return false;
                }
                else if (string.IsNullOrEmpty(_Password))
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidPassword);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "LoginVM", "update");
                return false;
            }
        }
        private bool chekCodeData()
        {
            try
            {
                if (string.IsNullOrEmpty(_Code))
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidOrgCode);
                    return false;
                }
                else if (string.IsNullOrEmpty(_UserName))
                {
                    Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidUserName);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "LoginVM", "update");
                return false;
            }
        }
        private async System.Threading.Tasks.ValueTask loginExt()
        {
            try
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading, null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    {
                        Helpers.Toast.ShowToastError(Resources.Resource.Msg_ConnectionError);
                        return;
                    }
                    else if (chekLogInData())
                    {
                        var res = await Api.ServiceApp.auth(UserName, Password);
                        if (res != null)
                        {
                            Helpers.Settings.IsLogedin = true;
                            Helpers.Settings.UserName = UserName;
                            Helpers.Settings.Password = Password;
                            Helpers.Settings.AuthoToken = res.access_token;
                            Helpers.Toast.ShowToastSuccess(Resources.Resource.Msg_Success);
                            App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.MunePage())
                            {
                                FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                            };
                        }
                        else
                        {
                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                            Helpers.Toast.ShowToastError(Resources.Resource.Msg_Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "loginCommand");
            }
        }
        private async System.Threading.Tasks.ValueTask UseCodeExt()
        {
            try
            {
                using(Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading, null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    {
                        Helpers.Toast.ShowToastError(Resources.Resource.Msg_ConnectionError);
                        return;
                    }
                    else if (chekCodeData())
                    {
                        Helpers.Settings.UserName = "sky365";
                        Helpers.Settings.Password = "sky365@365";
                        var res = await Api.ServiceApp.GetAll<ModelDto.ErpCompanyDTO>("erpcompany", filter: new Dictionary<string, string> { { "CompanyCode", _Code } });

                        if (res != null)
                        {
                            if (res.Count > 0)
                            {
                                Helpers.Settings.BaseAddress = res[0].CompanyUrl;
                                Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                                string DId = Xamarin.Forms.DependencyService.Get<Services.IDevice>().GetIdentifier();
                                var res2 = await Api.ServiceApp.GetAll<ModelDto.ErpSystemUserDTO>("ErpSystemUser", 
                                    filter: new Dictionary<string, string> { 
                                        { "CompanyCode", _Code },
                                        { "DeviceID", DId },
                                        { "UserEmail", UserName}
                                    });
                                if (res2 != null && res2.Count > 0)
                                {
                                    if(res2[0].IsEnable == true)
                                    {
                                        CompnyName = res[0].PathName;
                                        Helpers.Settings.IsFeristLogedin = false;
                                        IsVisibleLogin = true;
                                        IsVisibleCode = false;
                                        Helpers.Settings.UserName = "";
                                        Helpers.Settings.Password = "";
                                    }
                                    else
                                    {
                                        Helpers.Toast.ShowToastWarning(Resources.Resource.Msg_ConectToAdminForRegesterUser);
                                        Helpers.Settings.BaseAddress = "http://Khabir.saskw.net";
                                        Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                                        Helpers.Settings.UserName = "";
                                        Helpers.Settings.Password = "";
                                    }
                                }
                                else
                                {
                                    var res3 = await Api.ServiceApp.SaveNew<ModelDto.ReturnData>("ErpSystemUser", new Dictionary<string, string>()
                                    {
                                        {"CompanyCode", res[0].CompanyCode },
                                        {"UserEmail", UserName },
                                        {"AdminEmail", res[0].Admin_Email },
                                        {"IsEnable", "false" },
                                        {"DeviceID", DId },
                                    });
                                    Helpers.Toast.ShowToastWarning(Resources.Resource.Msg_ConectToAdminForRegesterUser);
                                    Helpers.Settings.BaseAddress = "http://Khabir.saskw.net";
                                    Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                                    Helpers.Settings.UserName = "";
                                    Helpers.Settings.Password = "";
                                }
                            }
                            else
                            {
                                Helpers.Settings.UserName = "";
                                Helpers.Settings.Password = "";
                                Helpers.Toast.ShowToastError(Resources.Resource.Msg_NotValidOrgCode);
                            }
                        }
                        else
                        {
                            Helpers.Settings.UserName = "";
                            Helpers.Settings.Password = "";
                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                            Helpers.Toast.ShowToastError(Resources.Resource.Msg_Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "UseCodeExt");
            }
        }
        private async System.Threading.Tasks.ValueTask ChangeCompanyExt()
        {
            try
            {
                Helpers.Settings.IsFeristLogedin = true;
                IsVisibleCode = true;
                IsVisibleLogin = false;
                Helpers.Settings.BaseAddress = "http://Khabir.saskw.net";
                Services.ApiServices.BaseImage = $"{Helpers.Settings.BaseAddress}/PicStocks/";
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "SignInSignUpVM", "UseCodeExt");
            }
        }

        #endregion

    }
}