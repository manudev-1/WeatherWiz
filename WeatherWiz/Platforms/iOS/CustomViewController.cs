#if IOS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace WeatherWiz.Platforms.iOS
{
    public class CustomViewController : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MakeStatusBarTranslucent();
        }

        private void MakeStatusBarTranslucent()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                EdgesForExtendedLayout = UIRectEdge.All;

                View.BackgroundColor = UIColor.Clear;

                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            }
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }
    }
}
#endif