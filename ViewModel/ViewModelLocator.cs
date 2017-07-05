using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using StormInfo.Services;

namespace StormInfo.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<INavigationService>(() =>
            {
                var navigationService = new NavigationService();
                navigationService.RegisterPage("Settings", "Pages/SettingsPage.xaml");
                navigationService.RegisterPage("Intro", "Pages/IntroPage.xaml");
                navigationService.RegisterPage("Location", "Pages/LocationPage.xaml");
                return navigationService;
            });
            SimpleIoc.Default.Register<SettingsViewModel>(true);
            SimpleIoc.Default.Register<NotificationViewModel>(true);
            SimpleIoc.Default.Register<MainViewModel>(true);
            SimpleIoc.Default.Register<LocationViewModel>(true);
        }

        public MainViewModel Main
        {
            get => ServiceLocator.Current.GetInstance<MainViewModel>();
        }

        public LocationViewModel Location
        {
            get => ServiceLocator.Current.GetInstance<LocationViewModel>();
        }

        public NotificationViewModel Notification
        {
            get => ServiceLocator.Current.GetInstance<NotificationViewModel>();
        }

        public SettingsViewModel Settings
        {
            get => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}