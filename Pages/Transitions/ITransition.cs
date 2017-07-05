using System;

namespace StormInfo.Pages.Transitions
{
    public interface ITransition
    {
        float Opacity { get; set; }
        Action OnValueChange { get; set; }
        void StartAnimation();
    }
}
