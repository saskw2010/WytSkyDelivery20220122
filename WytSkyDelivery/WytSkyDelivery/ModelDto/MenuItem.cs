using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WytSkyDelivery.ModelDto
{

    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; } = "IconMenu.png";
        public Type TargetType { get; set; }
        public Xamarin.CommunityToolkit.ObjectModel.IAsyncCommand CommandE { get; set; }
    }
}