using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XSource.Helpers
{
    public static class XWaitIndicator
    {

        public static SplashScreenManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = SplashScreenManager.CreateWaitIndicator(new DevExpress.Mvvm.DXSplashScreenViewModel() { Status = "En cours..." }, topmost: true);
                }
                return _manager;
            }
        }

        static SplashScreenManager _manager;

        public static void Show(int minDuration = 1000)
        {
            var owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            Manager.Show(0, minDuration, owner, startupLocation: WindowStartupLocation.CenterOwner);
        }

        public static void Close()
        {
            Manager.Close();
        }
    }
}
