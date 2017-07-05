using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StormInfo.Pages.Transitions;
using StormInfo.Properties;
using StormInfo.Services;
using StormInfo.Services.Messages;
using System.Windows.Input;

namespace StormInfo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        string _tip = "";
        string _locationName = "Type here your location";
        ITransition _transition;

        public string LocationName {
            get => _locationName;
            set
            {
                _locationName = value;
                RaisePropertyChanged("LocationName");
            }
        }

        public string Tip {
            get => !_tip.Contains("[]") && _tip.Length>0 ? Resources.Propositions + _tip : "";
            set
            {
                _tip = value;
                RaisePropertyChanged("Tip");
            }
        }

        public float TransitionOpacity
            => _transition.Opacity;

        public MainViewModel(INavigationService navigationService)
        {
            Messenger.Default.Register<IntroVMUpdateMessage>(this, HandleMessage);
            this._navigationService = navigationService;
            CheckInfoCommand = new RelayCommand(CheckInfo);
            LocationName = Settings.Default.LocationName;
            _transition = new Transition(() => RaisePropertyChanged("TransitionOpacity"));
            _transition.StartAnimation();
        }

        public ICommand CheckInfoCommand
        {
            get;
            private set;
        }

        private void CheckInfo()
        {
            StormService.Initialize(LocationName);
            if (StormService.Initialized)
            {
                _navigationService.NavigateTo("Location");
                Settings.Default.LocationName = LocationName;
                Settings.Default.Save();
                Tip = "";
                Messenger.Default.Send<LocationVMUpdateMessage>(new LocationVMUpdateMessage());
            }
            else
                Tip = StormService.GetLocationsByName(LocationName);
        }

        private void HandleMessage(IntroVMUpdateMessage message)
        {
            _transition.StartAnimation();
        }
    }
}