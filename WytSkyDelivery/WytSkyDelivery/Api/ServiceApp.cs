using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WytSkyDelivery.Api
{
    public class ServiceApp
    {
        public const string CONTROLR = "appservices";

        #region GetAll()
        
        public async static System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData>> GetAll(string pageNane,int pageIndex = 0, Dictionary<string, string> filter = null)
        {
            try
            {
                if (filter == null)
                {
                    filter = new Dictionary<string, string>()
                    {
                        {"_datatype", "json"},
                        {"_jsonarray", "1"},
                        {"_pageSize", Services.ApiServices.PAGESIZE},
                        {"_pageIndex", pageIndex + ""},
                    };
                }
                else
                {
                    filter.Add("_datatype", "json");
                    filter.Add("_jsonarray", "1");
                    filter.Add("_pageSize", Services.ApiServices.PAGESIZE);
                    filter.Add("_pageIndex", pageIndex + "");
                }
                var result = await Services.RequestProvider.Current.GetData<System.Collections.ObjectModel.ObservableCollection<Dictionary<string, string>>>(CONTROLR, pageNane, filter, Enums.AuthorizationType.UserNamePassword);
                if (result != null && result.IsPassed)
                {
                    return SetDataToObj(result.Data);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, Newtonsoft.Json.JsonConvert.SerializeObject(filter), "ServiceCatgeory", "GetAll()");
                return null;
            }
        }
        
        #endregion

        #region _auth()
        public async static System.Threading.Tasks.Task<ModelDto.UserLogin> auth(string userName, string password)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>()
                {
                    {"skyuser",userName},
                    {"skyuserpwd",password},
                    {"_datatype","json"},
                    {"_jsonarray","1"},
                };
                var result = await Services.RequestProvider.Current.GetData<ModelDto.UserLogin>(CONTROLR, "_auth", dictionary,Enums.AuthorizationType.nun);
                if (result != null && result.IsPassed)
                {
                    return result.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : ""));
                return null;
            }
        }
        #endregion

        #region GetAll()
        
        public async static System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<T>> GetAll<T>(string pageNane, int pageIndex = 0, Dictionary<string, string> filter = null)
        {
            try
            {
                if (filter == null)
                {
                    filter = new Dictionary<string, string>()
                    {
                        {"_datatype", "json"},
                        {"_jsonarray", "1"},
                        {"_pageSize", Services.ApiServices.PAGESIZE},
                        {"_pageIndex", pageIndex + ""},
                    };
                }
                else
                {
                    filter.Add("_datatype", "json");
                    filter.Add("_jsonarray", "1");
                    filter.Add("_pageSize", Services.ApiServices.PAGESIZE);
                    filter.Add("_pageIndex", pageIndex + "");
                }
                var result = await Services.RequestProvider.Current.GetData<System.Collections.ObjectModel.ObservableCollection<T>>(CONTROLR ,pageNane , filter, Enums.AuthorizationType.UserNamePassword);
                if (result != null && result.IsPassed)
                {
                    return result.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, Newtonsoft.Json.JsonConvert.SerializeObject(filter), "ServiceChat", "GetAll()");
                return null;
            }
        }

        #endregion

        #region OrderDetails(string orderId)

        public async static System.Threading.Tasks.Task<ModelDto.ScanQR.QROrderDTO> OrderDetails(string orderId)
        {
            try
            {
                var result = await Services.RequestProvider.Current.GetDataWithBaseUrl<ModelDto.ScanQR.QROrderDTO>("order_details", orderId, null, "https://app.pharmackw.com","", Enums.AuthorizationType.nun);
                if (result != null && result.success)
                {
                    return result.data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, orderId, "ServiceChat", "GetAll()");
                return null;
            }
        }

        #endregion

        #region SaveNew()
        public async static System.Threading.Tasks.Task<T> SaveNew<T>(string pageNane, Dictionary<string, string> formData)
        {
            try
            {
                var result = await Services.RequestProvider.Current.PostDataMultipart<T>(CONTROLR,pageNane, null, formData, Enums.AuthorizationType.UserNamePassword);
                if (result != null && result.IsPassed)
                {
                    return result.Data;
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>("");
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, Newtonsoft.Json.JsonConvert.SerializeObject(formData), "ServiceChat", "SaveNew()");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>("");
            }
        }
        #endregion

        #region NameToText

        public static string NameToText(string text)
        {
            var str = text.Replace("-", " ").Replace("_", " ").Replace(".", " ");
            string res = System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));

            return res;
        }

        #endregion

        #region SetDataToObj

        public static System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData> SetDataToObj(System.Collections.ObjectModel.ObservableCollection<Dictionary<string, string>> valuePairs)
        {
            System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData> resules = new System.Collections.ObjectModel.ObservableCollection<ModelDto.ListPageData>();
            foreach (var item in valuePairs)
            {
                resules.Add(SetDataToObj(item));
            }
            return resules;
        }
        public static ModelDto.ListPageData SetDataToObj(Dictionary<string, string> valuePairs)
        {
            return new ModelDto.ListPageData()
            {
                ListOfData = new System.Collections.ObjectModel.ObservableCollection<ModelDto.PageDataDTO>(valuePairs.Where(_ => !string.IsNullOrEmpty(_.Value)).Select(x => new ModelDto.PageDataDTO()
                {
                    Key = NameToText(CabetalFirestChart(x.Key)),
                    Value = string.IsNullOrEmpty(x.Value) ? "  A / N  " : x.Value,
                }).ToList())
            };
        }

        private static string CabetalFirestChart(string Key)
        {
            return Key.Substring(0,1).ToUpper() + Key.Substring(1);
        }

        public static ModelDto.ListPageData FromObjToDataView(object model)
        {
            try
            {
                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(model,Newtonsoft.Json.Formatting.Indented,new Utilities.MJsonConverter());
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return SetDataToObj(obj);
            }
            catch(Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                return null;
            }
    
        }
        public static Dictionary<string, string> FromObjToDictionary(object model)
        {
            try
            {
                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(model,Newtonsoft.Json.Formatting.Indented,new Utilities.MJsonConverter());
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return obj;
            }
            catch(Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                return null;
            }
    
        }

        #endregion

    }
}
