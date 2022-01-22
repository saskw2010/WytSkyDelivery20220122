using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Xamarin.Forms;
using ObjCRuntime;
using UIKit;
using System.Diagnostics;
using Foundation;
using WytSkyDelivery.Services;

[assembly: Dependency(typeof(WytSkyDelivery.iOS.Services.IOSFlowDirection))]
namespace WytSkyDelivery.iOS.Services
{
    public class IOSFlowDirection : IIOSFlowDirection
    {
        [DllImport(ObjCRuntime.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
        internal extern static IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector, UISemanticContentAttribute arg1);
        public IOSFlowDirection()
        {

        }
        public void SetLayoutRTL()
        {
            try
            {
                Selector selector = new Selector("setSemanticContentAttribute:");
                IntPtr_objc_msgSend(UIView.Appearance.Handle, selector.Handle, UISemanticContentAttribute.ForceRightToLeft);
            }
            catch (Exception)
            {
                return;
                //Debug.WriteLine("failed to set layout...." + s.Message.ToString());
            }
        }
        public void SetLayoutLTR()
        {
            try
            {
                Selector selector = new Selector("setSemanticContentAttribute:");
                IntPtr_objc_msgSend(UIView.Appearance.Handle, selector.Handle, UISemanticContentAttribute.ForceLeftToRight);
            }
            catch (Exception)
            {
                return;
                //Debug.WriteLine("failed to set layout...." + s.Message.ToString());
            }
        }
    }
}