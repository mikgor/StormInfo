using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StormInfo.Pages.Transitions;
using StormInfo.Services;
using StormInfo.Services.Messages;
using StormInfo.StormApi;
using System.Timers;
using System.Windows.Input;

namespace StormInfo.ViewModel
{
    public class LocationViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        Timer _updateTimer;
        ITransition _transition;
        MyComplexTypeBurza _storm = new MyComplexTypeBurza();
        string _locationName;

        public string LocationName
        {
            get => _locationName;
            set
            {
                _locationName = value;
                RaisePropertyChanged("LocationName");
            }
        }

        public string StormDistance
        {
            get => (StormDirection != "") ?_storm.odleglosc.ToString() + " km" : ">"+Properties.Settings.Default.StormRange +" km";
            set
            {
                _storm.odleglosc = float.Parse(value);
                RaisePropertyChanged("StormDistance");
            }
        }

        public string LightningNumber
        {
            get => _storm.liczba.ToString();
            set
            {
                _storm.liczba = int.Parse(value);
                RaisePropertyChanged("LightningNumber");
            }
        }

        public string StormDirection
        {
            get => _storm.kierunek;
            set
            {
                _storm.kierunek = value;
                RaisePropertyChanged("StormDirection");
            }
        }

        public string StormTime
        {
            get => _storm.okres.ToString() + " mins";
            set
            {
                _storm.okres = int.Parse(value);
                RaisePropertyChanged("StormTime");
            }
        }

        public float TransitionOpacity
            => _transition.Opacity;

        public LocationViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            Messenger.Default.Register<LocationVMUpdateMessage>(this, HandleMessage);
            BackCommand = new RelayCommand(Back);
            SettingsCommand = new RelayCommand(Settings);
            _transition = new Transition(() => RaisePropertyChanged("TransitionOpacity"));
            _updateTimer = new Timer()
            {
                Interval = 300000
            };
            _updateTimer.Elapsed += new ElapsedEventHandler(UpdateData);
        }

        public ICommand BackCommand
        {
            get;
            private set;
        }

        void Back()
        {
            _navigationService.NavigateTo("Intro");
            Messenger.Default.Send<IntroVMUpdateMessage>(new IntroVMUpdateMessage());
        }

        public ICommand SettingsCommand
        {
            get;
            private set;
        }

        void Settings()
        {
            _navigationService.NavigateTo("Settings");
            Messenger.Default.Send<SettingsVMUpdateMessage>(new SettingsVMUpdateMessage());
        }

        public void UpdateData(object source, ElapsedEventArgs e)
        {
            MyComplexTypeBurza storm = StormService.GetStormInfo(Properties.Settings.Default.StormRange);
            StormDistance = storm.odleglosc.ToString();
            LightningNumber = storm.liczba.ToString();
            StormDirection = storm.kierunek;
            StormTime = storm.okres.ToString();
            string notificationType = "NotFound";
            if (StormDirection != "")
                notificationType = storm.odleglosc >= 200 ? "Low" : storm.odleglosc >= 100 ? "Mid" : "High";
            Messenger.Default.Send(new NotificationVMUpdateMessage
            {
                TypeName = notificationType,
                StormDistance = (float)storm.odleglosc
            });
        }

        private void HandleMessage(LocationVMUpdateMessage message)
        {
            _updateTimer.Start();
            LocationName = Properties.Settings.Default.LocationName;
            UpdateData(null, null);
            _transition.StartAnimation();
        }
    }
}
