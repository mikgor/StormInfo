using System;
using System.Timers;

namespace StormInfo.Pages.Transitions
{
    public class Transition : ITransition
    {
        private float _opacity = 0f;
        float _duration = 1100; //in ms
        int _interval = 100; //in ms
        Timer _timer;
        private Action _onValueChange;

        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                OnValueChange();
            }
        }

        public Action OnValueChange { get => _onValueChange; set => _onValueChange = value; }

        public Transition(Action action)
        {
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(AddTime);
            _timer.Interval = _interval;
            OnValueChange = action;
        }

        void AddTime(object source, ElapsedEventArgs e)
        {
            Opacity += 1 / (_duration / _interval);
            if (Opacity >= 1)
            {
                Opacity = 1;
                _timer.Enabled = false;
            }
        }

        public void StartAnimation()
        {
            _opacity = 0;
            _timer.Enabled = true;
        }
    }
}
