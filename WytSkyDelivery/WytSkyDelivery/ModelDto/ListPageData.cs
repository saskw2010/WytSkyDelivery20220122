using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ListPageData : Base
    {
        #region PrivateField

        private string _TextSeeMore = Resources.Resource.Text_MoreDetails;
        private double _HeightRequest = 90;

        #endregion

        #region Properties

        public string TextSeeMore
        {
            get => _TextSeeMore;
            set => SetProperty(ref _TextSeeMore, value);
        }
        public double HeightRequest
        {
            get => _HeightRequest;
            set => SetProperty(ref _HeightRequest, value);
        }
        public System.Collections.ObjectModel.ObservableCollection<PageDataDTO> ListOfData { get; set; }

        #endregion

    }
}
