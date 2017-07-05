using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StormInfo.Services
{
    // Inspired by http://codepassion.azurewebsites.net/my-navigationservice-for-universal-apps/
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Uri> _framesDictionary = new Dictionary<string, Uri>();
        public object CurrentData { get; set; }

        public string CurrentPageKey
            => _framesDictionary.Where(x => x.Value == CurrentFrameController.Source).First().Key;

        public void RegisterPage(string key, string url)
        {
            _framesDictionary.Add(key, new Uri(url, UriKind.Relative));
        }

        public Frame CurrentFrameController
            => (Application.Current.Windows.OfType<Window>().FirstOrDefault() as MainWindow).FrameController;

        public void NavigateTo(string key)
        {
            var nextFrame = _framesDictionary[key];
            CurrentData = null;
            CurrentFrameController.Source = nextFrame;
        }

        public void NavigateTo(string key, object data)
        {
            NavigateTo(key);
            CurrentData = data;
        }

        public bool CanGoBack()
            => CurrentFrameController.CanGoBack;

        public void GoBack()
        {
            if (!CanGoBack()) return;
            CurrentFrameController.GoBack();
        }
    }
}
