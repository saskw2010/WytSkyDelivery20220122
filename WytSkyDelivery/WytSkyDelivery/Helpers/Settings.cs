using Xamarin.Forms;

namespace WytSkyDelivery.Helpers
{
    public static class Settings
    {
        #region Constants

        public const string CompanyNameKey = "CompanyName_Key";
        public const string BaseAddressKey = "BaseAddress_Key";
        public const string IsLogedinKey = "IsLogedin_Key";
        public const string IsFeristLogedinKey = "IsFeristLogedin_Key";
        public const string LanguageKey = "Language_Key";
        public const string UserNameKey = "UserName_Key";
        public const string PasswordKey = "Password_Key";
        public const string AuthoTokenKey = "AuthoToken_Key";
        public const string ClientNameKey = "ClientName_Key";

		#endregion

		#region Fields

		#region CompanyName

		public static string CompanyName
		{
			get => Application.Current.Properties.ContainsKey(CompanyNameKey)
				? Application.Current.Properties[CompanyNameKey].ToString()
				: null;
			set
			{
				Application.Current.Properties[CompanyNameKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

        #endregion
		
		#region BaseAddress

		public static string BaseAddress
		{
			get => Application.Current.Properties.ContainsKey(BaseAddressKey)
				? Application.Current.Properties[BaseAddressKey].ToString()
				: "http://Khabir.saskw.net";
			set
			{
				Application.Current.Properties[BaseAddressKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region IsLogedin

		public static bool IsLogedin
		{
			get => Application.Current.Properties.ContainsKey(IsLogedinKey)
				? bool.Parse(Application.Current.Properties[IsLogedinKey].ToString())
				: false;
			set
			{
				Application.Current.Properties[IsLogedinKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

        #endregion
		
		#region IsFeristLogedin

		public static bool IsFeristLogedin
		{
			get => Application.Current.Properties.ContainsKey(IsFeristLogedinKey)
				? bool.Parse(Application.Current.Properties[IsFeristLogedinKey].ToString())
				: true;
			set
			{
				Application.Current.Properties[IsFeristLogedinKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region Language

		public static string Language
		{
			get => Application.Current.Properties.ContainsKey(LanguageKey)
				? Application.Current.Properties[LanguageKey].ToString()
				: "en";
			set
			{
				Application.Current.Properties[LanguageKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region UserName

		public static string UserName
		{
			get => Application.Current.Properties.ContainsKey(UserNameKey)
				? Application.Current.Properties[UserNameKey].ToString()
				: null;
			set
			{
				Application.Current.Properties[UserNameKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region Password

		public static string Password
		{
			get => Application.Current.Properties.ContainsKey(PasswordKey)
				? Application.Current.Properties[PasswordKey].ToString()
				: null;
			set
			{
				Application.Current.Properties[PasswordKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region AuthoToken

		public static string AuthoToken
		{
			get => Application.Current.Properties.ContainsKey(AuthoTokenKey)
				? Application.Current.Properties[AuthoTokenKey].ToString()
				: null;
			set
			{
				Application.Current.Properties[AuthoTokenKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#region ClientName

		public static string ClientName
		{
			get => Application.Current.Properties.ContainsKey(ClientNameKey)
				? Application.Current.Properties[ClientNameKey].ToString()
				: null;
			set
			{
				Application.Current.Properties[ClientNameKey] = value;
				Application.Current.SavePropertiesAsync();
			}
		}

		#endregion

		#endregion
	}
}
