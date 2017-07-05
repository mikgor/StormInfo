using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StormInfo.Pages.Transitions;
using StormInfo.Properties;
using StormInfo.Services.Messages;
using System;
using System.Collections.Generic;
using System.Windows;

namespace StormInfo.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {
        ITransition _notifyTransition;
        string _details;
        double _screenWidth;
        double _screenHeight;
        string _color;
        Dictionary<string, string> _notificationTypes;

        public string Details
        {
            get => _details;
            set
            {
                _details = value;
                RaisePropertyChanged("Details");
            }
        }

        public float TransitionOpacity
        {
            get => _notifyTransition.Opacity;
        }

        public double LeftMargin
            => _screenWidth - 300;

        public double TopMargin
            => _screenHeight - 150;

        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }

        public NotificationViewModel()
        {
            _notifyTransition = new NotificationTransition(() => RaisePropertyChanged("TransitionOpacity"));
            Messenger.Default.Register<NotificationVMUpdateMessage>(this, HandleMessage);
            _screenWidth = SystemParameters.PrimaryScreenWidth;
            _screenHeight = SystemParameters.PrimaryScreenHeight;
            _color = "#FF3C4759";
            _notificationTypes = new Dictionary<string, string>
            {
                { "NotFound", "#FF3C4759" },
                { "Low", "#FF538D3D" },
                { "Mid", "#FF8D8A3D" },
                { "High", "#FF8D3D3D" }
            };
        }

        private void HandleMessage(NotificationVMUpdateMessage message)
        {
            if (Settings.Default.NotificationsEnabled)
            {
                string details = DateTime.Now.ToString("HH:mm\n");
                details += message.TypeName == "NotFound" ? Resources.NotificationDetailsNotFound : 
                    string.Format(Resources.NotificationDetails, message.StormDistance, Settings.Default.LocationName) ;
                Details = details;
                Color = _notificationTypes[message.TypeName];
                _notifyTransition.StartAnimation();
            }
        }
    }
}
