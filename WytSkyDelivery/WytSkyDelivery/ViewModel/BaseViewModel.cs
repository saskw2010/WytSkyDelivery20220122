using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ViewModel
{
    public class BaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region privateFild

        private bool _CanExcute = true;

        #endregion

        #region Commands

        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand GoBackCommand { get; set; }
        
        #endregion

        #region Constructor
        public BaseViewModel()
        {
            try
            {
                GoBackCommand = new Xamarin.CommunityToolkit.ObjectModel.AsyncCommand(async () => { CanExcute = false; await GoBack(); CanExcute = true; }, obj => CanExcute);
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "BaseViewModel", "Constructor");
            }
        }
        #endregion

        #region Properties
        
        public bool CanExcute
        {
            get => _CanExcute;
            set => SetProperty(ref _CanExcute, value);
        }

        #endregion

        #region Methods
        private async System.Threading.Tasks.ValueTask GoBack()
        {
            try
            {
                if ((App.Current.MainPage.Navigation.ModalStack.Count + App.Current.MainPage.Navigation.NavigationStack.Count) >= 2)
                {
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Pages.HomePage())
                    {
                        FlowDirection = Helpers.Settings.Language == "ar" ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight
                    };
                }
            }
            catch (Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
                ExtensionLogMethods.LogExtension(ExceptionMseeage, "", "BaseViewModel", "GoBack");
            }
        }
        #endregion

        #region SetProperty
        protected bool SetProperty<T>(ref T backingStore, T value,
            [System.Runtime.CompilerServices.CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            try
            {
                var changed = PropertyChanged;
                if (changed == null)
                    return;

                changed.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

            }
            catch(Exception ex)
            {
                string ExceptionMseeage = string.Format(" Error : {0} - {1} ", ex.Message, ex.InnerException != null ? ex.InnerException.FullMessage() : "");
                System.Diagnostics.Debug.WriteLine(ExceptionMseeage);
            }
        }
        protected virtual void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
            else
            {
                foreach (System.Reflection.PropertyInfo pi in this.GetType().GetProperties())
                    OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(pi.Name));
            }
        }
        #endregion
    }
}