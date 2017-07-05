using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StormInfo.Pages.Transitions;
using StormInfo.Services.Messages;
using System.Windows.Input;
using StormInfo.Properties;

namespace StormInfo.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        ITransition _transition;
        int _stormRange;
        bool _notificationsEnabled;

        public float TransitionOpacity
            => _transition.Opacity;

        public string StormRange
        {
            get => _stormRange.ToString();
            set
            {
                int.TryParse(value, out _stormRange);
                RaisePropertyChanged("StormRange");
            }
        }

        public bool NotificationsEnabled
        {
            get => _notificationsEnabled;
            set
            {
                _notificationsEnabled = value;
                RaisePropertyChanged("NotificationsEnabled");
            }
        }

        public SettingsViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            Messenger.Default.Register<SettingsVMUpdateMessage>(this, HandleMessage);
            BackCommand = new RelayCommand(Back);
            SaveCommand = new RelayCommand(Save);
            _transition = new Transition(() => RaisePropertyChanged("TransitionOpacity"));
        }

        public ICommand BackCommand
        {
            get;
            private set;
        }

        void Back()
        {
            _navigationService.NavigateTo("Location");
            Messenger.Default.Send<LocationVMUpdateMessage>(new LocationVMUpdateMessage());
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        void Save()
        {
            Settings.Default.StormRange = _stormRange;
            Settings.Default.NotificationsEnabled = _notificationsEnabled;
            Settings.Default.Save();
        }

        private void HandleMessage(SettingsVMUpdateMessage message)
        {
            _transition.StartAnimation();
            StormRange = Settings.Default.StormRange.ToString();
            NotificationsEnabled = Settings.Default.NotificationsEnabled;
        }
    }
}
