using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.CustomControl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NEntry : ContentView
    {

        #region Constructor
        public NEntry()
        {
            try
            {
                InitializeComponent();
                //ChangeVisualState();
                this.FlowDirection = Helpers.Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
            catch (System.Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "NEntry", "Constructor");
            }
        }
        #endregion

        #region Property
        
        #region TextColor
        public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor),
            typeof(Color), typeof(NEntry), Color.Black,BindingMode.TwoWay);
        // Gets or sets TextColor value  
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion

        #region KeyboardType 
        public static readonly BindableProperty KeyboardTypeProperty =
           BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(NEntry), defaultValue: Keyboard.Default, BindingMode.OneWay);
        public Keyboard KeyboardType
        {
            get
            {
                return (Keyboard)GetValue(KeyboardTypeProperty);
            }
            set
            {
                SetValue(KeyboardTypeProperty, value);
            }
        }
        #endregion

        #region FontFamily
        public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily),
            typeof(string), typeof(NEntry));
        // Gets or sets FontFamily value  
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion

        #region HeaderColor
        public static readonly BindableProperty HeaderColorProperty =
        BindableProperty.Create(nameof(HeaderColor),
            typeof(Color), typeof(NEntry), Color.Black, BindingMode.TwoWay);
        // Gets or sets HeaderColor value  
        public Color HeaderColor
        {
            get => (Color)GetValue(HeaderColorProperty);
            set => SetValue(HeaderColorProperty, value);
        }
        #endregion

        #region PlaceholderColor
        public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor),
            typeof(Color), typeof(NEntry), Color.Gray, BindingMode.TwoWay);
        // Gets or sets PlaceholderColor value  
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
        #endregion
        
        #region IconSource
        public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource),
            typeof(string), typeof(NEntry), "", BindingMode.TwoWay);
        // Gets or sets IconSource value  
        public string IconSource
        {
            get => (string)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }
        #endregion

        #region Header
        public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(nameof(Header),
            typeof(string), typeof(NEntry), "", BindingMode.TwoWay);
        // Gets or sets Header value  
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        #endregion

        #region Text
        public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text),
            typeof(string), typeof(NEntry), "", BindingMode.TwoWay);
        // Gets or sets Text value  
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion
        
        #region IsEnabledEntry
        public static readonly BindableProperty IsEnabledEntryProperty =
        BindableProperty.Create(nameof(IsEnabledEntry),
            typeof(bool), typeof(NEntry), true, BindingMode.TwoWay);
        // Gets or sets IsEnabledEntry value  
        public bool IsEnabledEntry
        {
            get => (bool)GetValue(IsEnabledEntryProperty);
            set => SetValue(IsEnabledEntryProperty, value);
        }
        #endregion
        
        #region IsPassword
        public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword),
            typeof(bool), typeof(NEntry), false, BindingMode.TwoWay, propertyChanged: ItemChanged);
        // Gets or sets IsPassword value  
        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }
        #endregion
        
        #region IsShowPassword
        public static readonly BindableProperty IsShowPasswordProperty =
        BindableProperty.Create(nameof(IsShowPassword),
            typeof(bool), typeof(NEntry), false, BindingMode.TwoWay,propertyChanged: ItemChanged);
        // Gets or sets IsShowPassword value  
        public bool IsShowPassword
        {
            get => (bool)GetValue(IsShowPasswordProperty);
            set => SetValue(IsShowPasswordProperty, value);
        }
        #endregion

        #endregion

        #region Methods
        private void ShowHidePassword()
        {
            try
            {
                IsShowPassword = !IsShowPassword;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error : {ex.Message}");
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                IsShowPassword = !IsShowPassword;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error : {ex.Message}");
            }
        }
        private static void ItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((NEntry)bindable)?.ChangeVisualState();
        }
        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            if(!IsPassword)
            {
                VisualStateManager.GoToState(MainView, "NotPassword");
            }
            else if(!IsShowPassword)
            {
                VisualStateManager.GoToState(MainView, "HidePassword");
            }
            else
            {
                VisualStateManager.GoToState(MainView, "ShowPassword");
            }
        }
        #endregion
    }
}