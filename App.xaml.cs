using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace XSource
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemedWindow.RoundCorners = true;
            var palette = new Win10Palette(true, accentColor:System.Windows.Media.Colors.OrangeRed, appMode:WindowsAppMode.Dark);

            var theme = Theme.CreateTheme(palette, Theme.Win10Dark);
            Theme.RegisterTheme(theme);
            ApplicationThemeHelper.ApplicationThemeName = theme.Name;
        }
    }
}
