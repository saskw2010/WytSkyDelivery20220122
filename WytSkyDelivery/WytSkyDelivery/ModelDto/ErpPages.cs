using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class ErpPages : Base
    {
        public Nullable<long> PageID { get; set; }
        public string PagesNameAr { get; set; }
        public string PagesNameEn { get; set; }
        public string Name { get { return Helpers.Settings.Language == "ar" ? (string.IsNullOrEmpty(PagesNameAr) ? PagesNameEn : PagesNameAr) : (string.IsNullOrEmpty(PagesNameEn) ? PagesNameAr : PagesNameEn); } }
        public string Permission { get; set; }
        public Nullable<long> IconID { get; set; }
        public Nullable<int> Order { get; set; } = 0;
        public string PathName { get; set; }
        public Nullable<bool> HasChild { get; set; }
        public Nullable<long> ParentID { get; set; }
        public string PicStockControllername { get; set; }
        public string PicStockFileName { get; set; }
        public string ParentPathName { get; set; }
        public string Picurl { get { return PicStockControllername + "-" + IconID + "-" + PicStockFileName; } }
        public string ImageUrl { get { return string.IsNullOrEmpty(Picurl) || Picurl == "--" ? "Icon" : Services.ApiServices.BaseImage + Picurl; } }

        
        
    }
}
