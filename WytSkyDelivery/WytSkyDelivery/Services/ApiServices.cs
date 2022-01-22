using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http.Headers;

namespace WytSkyDelivery.Services
{
    public class ApiServices
    {

        //Api Url
        public static string BaseAddress = "http://pharmackw.saskw.net";
        public static string BaseAddressApi = "https://app.pharmackw.com";
        public const string PAGESIZE = "100";
        private const string apiAddress = "";
        public static string BaseImage = "https://app.pharmackw.com/PicStocks/";

        #region PostDataWithBaseUrl<T, T1>(T1 data, string control, string action, Dictionary<string, string> d,string BaseUrl, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.ResultApi<T>> PostDataWithBaseUrl<T, T1>(T1 data, string control, string action, Dictionary<string, string> d,string BaseUrl, string apiAddress, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseUrl),
            }; 
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;

                    if (data != null)
                        json = JsonConvert.SerializeObject(data);
                    StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseUrl}{url} ");
                    request = await client.PostAsync(url, dataConverted);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.ResultApi<T>()
                        {
                            success = false,
                            message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.ResultApi<T>()
                        {
                            success = false,
                            message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    return JsonConvert.DeserializeObject<ModelDto.ResultApi<T>>(content);
                    //return new ModelDto.ResultApi<T>()
                    //{
                    //    success = true,
                    //    ResultApi = $"Ok",
                    //    Data = obj,
                    //};
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostDataWithBaseUrl");
                return new ModelDto.ResultApi<T>()
                {
                    success = false,
                    message = Error,
                };
            }
        }
        #endregion

        #region PostDataWithBaseUrlNoLoading<T, T1>(T1 data, string control, string action, Dictionary<string, string> d,string BaseUrl, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.ResultApi<T>> PostDataWithBaseUrlNoLoading<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, string BaseUrl, string apiAddress, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseUrl),
            };
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                HttpResponseMessage request = null;

                if (data != null)
                    json = JsonConvert.SerializeObject(data);
                StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                StringBuilder query = new StringBuilder().Append("?");
                if (d != null && d.Count > 0)
                {
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseUrl}{url} ");
                request = await client.PostAsync(url, dataConverted);
                var content = await request.Content.ReadAsStringAsync();
                if (!request.IsSuccessStatusCode && (
                    request.StatusCode == HttpStatusCode.GatewayTimeout ||
                    request.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.ResultApi<T>()
                    {
                        success = false,
                        message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                    };
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                    return null;
                }
                else if (!request.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.ResultApi<T>()
                    {
                        success = false,
                        message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                return JsonConvert.DeserializeObject<ModelDto.ResultApi<T>>(content);
                //return new ModelDto.ResultApi<T>()
                //{
                //    success = true,
                //    ResultApi = $"Ok",
                //    Data = obj,
                //};
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostDataWithBaseUrl");
                return new ModelDto.ResultApi<T>()
                {
                    success = false,
                    message = Error,
                };
            }
        }
        #endregion

        #region GetDataWithBaseUrl<T>(string control, string action, Dictionary<string, string> d,string BaseUrl, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.ResultApi<T>> GetDataWithBaseUrl<T>(string control, string action, Dictionary<string, string> d, string BaseUrl, string apiAddress, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseUrl),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseUrl}{url} ");
                    request = await client.GetAsync(url);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.ResultApi<T>()
                        {
                            success = false,
                            message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.ResultApi<T>()
                        {
                            success = false,
                            message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    //return JsonConvert.DeserializeObject<ModelDto.ResultApi<T>>(content);
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.ResultApi<T>()
                    {
                        success = true,
                        data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {BaseUrl}/{url} {Environment.NewLine}", "ApiServices", "GetDataWithBaseUrl");
                return new ModelDto.ResultApi<T>()
                {
                    success = false,
                    message = Error,
                };
            }
        }
        #endregion

        #region GetDataWithBaseUrlNoLoading<T>(string control, string action, Dictionary<string, string> d,string BaseUrl, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.ResultApi<T>> GetDataWithBaseUrlNoLoading<T>(string control, string action, Dictionary<string, string> d, string BaseUrl, string apiAddress, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseUrl),
            };
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                HttpResponseMessage request = null;
                StringBuilder query = new StringBuilder().Append("?");
                if (d != null && d.Count > 0)
                {
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseUrl}{url} ");
                request = await client.GetAsync(url);
                var content = await request.Content.ReadAsStringAsync();
                if (!request.IsSuccessStatusCode && (
                    request.StatusCode == HttpStatusCode.GatewayTimeout ||
                    request.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.ResultApi<T>()
                    {
                        success = false,
                        message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                    };
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                    return null;
                }
                else if (!request.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.ResultApi<T>()
                    {
                        success = false,
                        message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                return JsonConvert.DeserializeObject<ModelDto.ResultApi<T>>(content);
                //return new ModelDto.ResultApi<T>()
                //{
                //    success = true,
                //    ResultApi = $"Ok",
                //    Data = obj,
                //};
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {BaseUrl}/{url} {Environment.NewLine}", "ApiServices", "PostDataWithBaseUrl");
                return new ModelDto.ResultApi<T>()
                {
                    success = false,
                    message = Error,
                };
            }
        }
        #endregion

        #region PostData<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> PostData<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;

                    if (data != null)
                        json = JsonConvert.SerializeObject(data);
                    StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}{url} ");
                    request = await client.PostAsync(url, dataConverted);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostData");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataIgnoreException(data,control,action,d,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> PostDataIgnoreException<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                HttpResponseMessage request = null;

                if (data != null)
                    json = JsonConvert.SerializeObject(data);
                StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                StringBuilder query = new StringBuilder().Append("?");
                if (d != null && d.Count > 0)
                {
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                request = await client.PostAsync(url, dataConverted);
                var content = await request.Content.ReadAsStringAsync();
                if (!request.IsSuccessStatusCode && (
                    request.StatusCode == HttpStatusCode.GatewayTimeout ||
                    request.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                    };
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                    return null;
                }
                else if (!request.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(content);
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = true,
                    Message = $"Ok", Content = content,
                    Data = obj,
                };
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataWithFile<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, List<Stream> files, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> PostDataWithFile<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, List<Stream> files, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;

                    if (data != null)
                        json = JsonConvert.SerializeObject(data);
                    StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    if (files != null)
                    {
                        var formData = new MultipartFormDataContent();
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpContent fileStreamContent = new StreamContent(files[i]);
                            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = $"File_{i}" };
                            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            formData.Add(fileStreamContent);
                        }
                        request = await client.PostAsync(url, formData);
                    }
                    else
                    {
                        request = await client.PostAsync(url, dataConverted);
                    }
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostDataWithFile");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataWithFileWithBaseUrl<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, List<Stream> files, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> PostDataWithFileWithBaseUrl<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, string BaseUrl, string apiAddress, List<Dictionary<string, Stream>> files, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(90),
                BaseAddress = new Uri(BaseUrl),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;

                    if (data != null)
                        json = JsonConvert.SerializeObject(data);
                    StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseUrl}/{url} ");
                    if (files != null)
                    {
                        var formData = new MultipartFormDataContent();
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpContent fileStreamContent = new StreamContent(files[i].FirstOrDefault().Value);
                            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = $"File{files[i].FirstOrDefault().Key}" };
                            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            formData.Add(fileStreamContent);
                        }
                        request = await client.PostAsync(url, formData);
                    }
                    else
                    {
                        request = await client.PostAsync(url, dataConverted);
                    }
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostDataWithFile");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataMultipart(data,control,action,d,files,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> PostDataMultipart<T>(string control, string action, Dictionary<string, string> d, Dictionary<string, string> formData, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    RestSharp.RestClient client = new RestSharp.RestClient($"{BaseAddress}/{url}");
                    client.Timeout = -1;
                    var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        request.AddHeader("Authorization", $"Bearer {Helpers.Settings.AuthoToken}");
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(Helpers.Settings.UserName, Helpers.Settings.Password);
                    }
                    request.AlwaysMultipartFormData = true;
                    foreach (var item in formData)
                    {
                        request.AddParameter(item.Key, item.Value);
                    }
                    RestSharp.IRestResponse response = await client.ExecuteAsync(request);
                    var content = response.Content;
                    if (!response.IsSuccessful && (
                        response.StatusCode == HttpStatusCode.GatewayTimeout ||
                        response.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {response.StatusCode} ) ",
                        };
                    }
                    else if (!response.IsSuccessful)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {response.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                //ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {Newtonsoft.Json.JsonConvert.SerializeObject(formData)}", "ApiServices", "PostDataMultipart");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataMultipartNoLoding(data,control,action,d,files,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> PostDataMultipartNoLoding<T>(string control, string action, Dictionary<string, string> d, Dictionary<string, string> formData, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            try
            {
                StringBuilder query = new StringBuilder().Append("?");
                if (d != null && d.Count > 0)
                {
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                var client = new RestSharp.RestClient($"{BaseAddress}/{url}");
                client.Timeout = -1;
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    request.AddHeader("Authorization", $"Bearer {Helpers.Settings.AuthoToken}");
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(Helpers.Settings.UserName, Helpers.Settings.Password);
                }
                request.AlwaysMultipartFormData = true;
                foreach (var item in formData)
                {
                    request.AddParameter(item.Key, item.Value);
                }
                RestSharp.IRestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                if (!response.IsSuccessful && (
                    response.StatusCode == HttpStatusCode.GatewayTimeout ||
                    response.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"Time Out :: StatusCode is ( {response.StatusCode} ) ",
                    };
                }
                else if (!response.IsSuccessful)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"StatusCode is ( {response.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(content);
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = true,
                    Message = $"Ok", Content = content,
                    Data = obj,
                };
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                //ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {Newtonsoft.Json.JsonConvert.SerializeObject(formData)}", "ApiServices", "PostDataMultipart");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PutDataMultipart(data,control,action,d,files,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> PutDataMultipart<T>(string control, string action, Dictionary<string, string> d, Dictionary<string, string> formData, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    StringBuilder query = new StringBuilder().Append("?");
                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    var client = new RestSharp.RestClient($"{BaseAddress}/{url}");
                    client.Timeout = -1;
                    var request = new RestSharp.RestRequest(RestSharp.Method.PUT);
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        request.AddHeader("Authorization", $"Bearer {Helpers.Settings.AuthoToken}");
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(Helpers.Settings.UserName, Helpers.Settings.Password);
                    }
                    request.AlwaysMultipartFormData = true;
                    foreach (var item in formData)
                    {
                        request.AddParameter(item.Key, item.Value);
                    }
                    RestSharp.IRestResponse response = await client.ExecuteAsync(request);
                    var content = response.Content;
                    if (!response.IsSuccessful && (
                        response.StatusCode == HttpStatusCode.GatewayTimeout ||
                        response.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {response.StatusCode} ) ",
                        };
                    }
                    else if (!response.IsSuccessful)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {response.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                //ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {Newtonsoft.Json.JsonConvert.SerializeObject(formData)}", "ApiServices", "PutDataMultipart");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PutData<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> PutData<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string url = "", json = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;
                    if (data != null)
                        json = JsonConvert.SerializeObject(data);
                    StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                    StringBuilder query = new StringBuilder().Append("?");

                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    request = await client.PutAsync(url, dataConverted);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PutData");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region GetData<T>(string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> GetData<T>(string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;
                    StringBuilder query = new StringBuilder().Append("?");

                    if (d != null && d.Count > 0)
                    {
                        d.Add("IsActive", "true");
                        d.Add("IsDelete", "false");
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    request = await client.GetAsync(url);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; 
                        Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} ", "ApiServices", "GetData");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region DeleteData(control,action,d,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> DeleteData<T>(string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                //using (Acr.UserDialogs.UserDialogs.Instance.Loading(Resources.Resource.Text_Loading"), null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Authorization == Enums.AuthorizationType.Token)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                    }
                    else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    HttpResponseMessage request = null;
                    StringBuilder query = new StringBuilder().Append("?");

                    if (d != null && d.Count > 0)
                    {
                        var lastElement = d.ElementAt(d.Count - 1);
                        d.Remove("TimeStamp");
                        foreach (var item in d)
                        {
                            var flag = item.Key == lastElement.Key;
                            if (!flag)
                                query.Append(item.Key + "=" + item.Value).Append("&");
                            else
                                query.Append(item.Key + "=" + item.Value);
                        }
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}{query}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}{query}";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action))
                        {
                            url = $"{apiAddress}{control}";
                        }
                        else
                        {
                            url = $"{apiAddress}{control}/{action}";
                        }
                    }
                    Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                    request = await client.DeleteAsync(url);
                    var content = await request.Content.ReadAsStringAsync();
                    if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                        {
                            FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                        };
                        return null;
                    }
                    else if (!request.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error {content}");
                        return new ModelDto.IResponse<T>()
                        {
                            IsPassed = false,
                            Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                        };
                    }
                    var obj = JsonConvert.DeserializeObject<T>(content);
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = true,
                        Message = $"Ok", Content = content,
                        Data = obj,
                    };
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} ", "ApiServices", "DeleteData");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region PostDataNoLoding<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        public async Task<ModelDto.IResponse<T>> PostDataNoLoding<T, T1>(T1 data, string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string json = "", url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                HttpResponseMessage request = null;

                if (data != null)
                    json = JsonConvert.SerializeObject(data);
                StringContent dataConverted = new StringContent(json, Encoding.UTF8, "application/json");
                StringBuilder query = new StringBuilder().Append("?");
                if (d != null && d.Count > 0)
                {
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                request = await client.PostAsync(url, dataConverted);
                var content = await request.Content.ReadAsStringAsync();
                if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                    };
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                    return null;
                }
                else if (!request.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(content);
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = true,
                    Message = $"Ok", Content = content,
                    Data = obj,
                };
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} {Environment.NewLine} Data : {json}", "ApiServices", "PostDataNoLoding");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion

        #region GetDataNoLoding(control,action,d,IsAuthorization)
        public async Task<ModelDto.IResponse<T>> GetDataNoLoding<T>(string control, string action, Dictionary<string, string> d, Enums.AuthorizationType Authorization = 0)
        {
            string url = "";
            HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri(BaseAddress),
            };
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Authorization == Enums.AuthorizationType.Token)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Settings.AuthoToken);
                }
                else if (Authorization == Enums.AuthorizationType.UserNamePassword)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Helpers.Settings.UserName}:{Helpers.Settings.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                HttpResponseMessage request = null;
                StringBuilder query = new StringBuilder().Append("?");

                if (d != null && d.Count > 0)
                {
                    d.Add("IsActive", "true");
                    d.Add("IsDelete", "false");
                    var lastElement = d.ElementAt(d.Count - 1);
                    d.Remove("TimeStamp");
                    foreach (var item in d)
                    {
                        var flag = item.Key == lastElement.Key;
                        if (!flag)
                            query.Append(item.Key + "=" + item.Value).Append("&");
                        else
                            query.Append(item.Key + "=" + item.Value);
                    }
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}{query}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}{query}";
                    }

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(action))
                    {
                        url = $"{apiAddress}{control}";
                    }
                    else
                    {
                        url = $"{apiAddress}{control}/{action}";
                    }
                }
                Debug.WriteLine($"Url: {BaseAddress}/{url} ");
                request = await client.GetAsync(url);
                var content = await request.Content.ReadAsStringAsync();
                if (!request.IsSuccessStatusCode && (
                        request.StatusCode == HttpStatusCode.GatewayTimeout ||
                        request.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"Time Out :: StatusCode is ( {request.StatusCode} ) ",
                    };
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    Helpers.Settings.AuthoToken = ""; client.DefaultRequestHeaders.Authorization = null; Helpers.Toast.ShowToastError(Resources.Resource.Msg_TimeExpired);
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.LoginPage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                    return null;
                }
                else if (!request.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Error {content}");
                    return new ModelDto.IResponse<T>()
                    {
                        IsPassed = false,
                        Message = $"StatusCode is ( {request.StatusCode} ) {Environment.NewLine} Error : {content}  ",
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(content);
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = true,
                    Message = $"Ok", Content = content,
                    Data = obj,
                };
            }
            catch (Exception ex)
            {
                string Error = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(Error);
                ExtensionLogMethods.LogExtension(Error, $"url : {client.BaseAddress.AbsoluteUri}/{url} ", "ApiServices", "GetDataNoLoding");
                return new ModelDto.IResponse<T>()
                {
                    IsPassed = false,
                    Message = Error,
                };
            }
        }
        #endregion


    }
}
