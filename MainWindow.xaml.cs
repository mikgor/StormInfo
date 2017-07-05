using StormInfo.Pages;
using StormInfo.Services;
using System.Windows;
using AppUpdater.AppUpdater;
using StormInfo.Properties;

namespace StormInfo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                UpdaterConfig updaterConfig = new UpdaterConfig("");
                Updater.CheckForUpdates(Settings.Default.Version, updaterConfig);
            }
            catch { }
            InitializeComponent();
            Notification notification = new Notification();
            Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                notification.Close();
                StormService.Disconnect();
            };
            notification.Show();
        }
    }
}
