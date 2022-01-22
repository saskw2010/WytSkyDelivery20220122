using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WytSkyDelivery.DataTemplate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseViewDT : ContentView
    {
        public BaseViewDT()
        {
            InitializeComponent();
        }

        #region ListOfData

        public static readonly BindableProperty ListOfDataProperty =
        BindableProperty.Create(nameof(ListOfData),
            typeof(object), typeof(BaseViewDT), false, BindingMode.TwoWay);
        // Gets or sets DataModel value  
        public object ListOfData
        {
            get => (object)GetValue(ListOfDataProperty);
            set => SetValue(ListOfDataProperty, value);
        }

        #endregion

        #region DataModel
        public static readonly BindableProperty DataModelProperty =
        BindableProperty.Create(nameof(DataModel),
            typeof(object), typeof(BaseViewDT), false, BindingMode.TwoWay, propertyChanged: ItemChanged);
        // Gets or sets DataModel value  
        public object DataModel
        {
            get => (object)GetValue(DataModelProperty);
            set => SetValue(DataModelProperty, value);
        }

        private static void ItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((BaseViewDT)bindable)?.ChangeVisualState();
        }
        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            if(DataModel != null)
            {
                var res = Api.ServiceApp.FromObjToDataView(DataModel);
                ListOfData = res.ListOfData;
            }
        }
        #endregion
    }
}