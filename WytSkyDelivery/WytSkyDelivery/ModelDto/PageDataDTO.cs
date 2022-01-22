using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class PageDataDTO : Base
    {
        #region PrivateField

        private string _Key;
        private string _Value;

        #endregion

        #region Properties

        public string Key
        {
            get => _Key;
            set => SetProperty(ref _Key, value);
        }
        public string Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }
        

        #endregion

    }
}
