using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.CustomControl.PopupMessage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToastPopupPage : MyPopupPage
    {
        public ToastPopupPage()
        {
            InitializeComponent();
        }

        #region ToastType
        public static readonly BindableProperty ToastTypeProperty =
        BindableProperty.Create(nameof(ToastType),
            typeof(Enums.TypeOfToast), typeof(ToastPopupPage), default, BindingMode.TwoWay, propertyChanged: SelectedItemChanged);
        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ToastPopupPage)bindable)?.ChangeVisualState();
        }

        // Gets or sets ToastType value  
        public Enums.TypeOfToast ToastType
        {
            get => (Enums.TypeOfToast)GetValue(ToastTypeProperty);
            set => SetValue(ToastTypeProperty, value);
        }
        #endregion

        #region ImageSource
        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource),
            typeof(string), typeof(ToastPopupPage), "", BindingMode.TwoWay);
        // Gets or sets ImageSource value  
        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        #endregion

        #region ToastMessage
        public static readonly BindableProperty ToastMessageProperty =
        BindableProperty.Create(nameof(ToastMessage),
            typeof(string), typeof(ToastPopupPage), "", BindingMode.TwoWay);
        // Gets or sets ToastMessage value  
        public string ToastMessage
        {
            get => (string)GetValue(ToastMessageProperty);
            set => SetValue(ToastMessageProperty, value);
        }
        #endregion

        #region ToastTitle
        public static readonly BindableProperty ToastTitleProperty =
        BindableProperty.Create(nameof(ToastTitle),
            typeof(string), typeof(ToastPopupPage), "", BindingMode.TwoWay);
        // Gets or sets ToastTitle value  
        public string ToastTitle
        {
            get => (string)GetValue(ToastTitleProperty);
            set => SetValue(ToastTitleProperty, value);
        }
        #endregion

        #region Methods

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            VisualStateManager.GoToState(MainView, ToastType.ToString());
        }

        #endregion
    }
}