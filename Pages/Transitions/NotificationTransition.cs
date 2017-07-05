using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace StormInfo.Pages.Transitions
{
    public class NotificationTransition : ITransition
    {
        private float _opacity = 0f;
        float [] _durations = new float[] { 1100, 10000, 1500 }; //in ms
        int _interval = 100; //in ms
        int _currentPhase = 0;
        Timer Timer;
        private Action onValueChange;

        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                OnValueChange();
            }
        }

        public Action OnValueChange
        {
            get => onValueChange;
            set
            {
                onValueChange = value;
            }
        }

        public NotificationTransition(Action onValueChange)
        {
            Timer = new Timer();
            Timer.Elapsed += new ElapsedEventHandler(AddTime);
            Timer.Interval = _interval;
            OnValueChange = onValueChange;
        }

        void AddTime(object source, ElapsedEventArgs e)
        {
            float value = 1 / (_durations[_currentPhase] / _interval);
 
            switch (_currentPhase)
            {
                case 0:
                    Opacity += value;
                    if (Opacity >= 1)
                    {
                        Opacity = 1;
                        _currentPhase++;
                    }
                    break;

                case 1:
                    Opacity -= value / 10;
                    if (Opacity <= 1 - value * 10)
                    {
                        Opacity = 1;
                        _currentPhase++;
                    }
                    break;

                case 2:
                    Opacity -= value;
                    if (Opacity <= 0)
                        Timer.Enabled = false;
                    break;
            }
        }

        public void StartAnimation()
        {
            Opacity = 0;
            _currentPhase = 0;
            Timer.Enabled = true;
        }
    }
}
