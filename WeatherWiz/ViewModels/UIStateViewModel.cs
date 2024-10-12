using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.ViewModels
{
    public class EventParams
    {
        public object? Sender { get; set; }
        public PanUpdatedEventArgs? EventArgs { get; set; }
    }
    public class UIStateViewModel : BaseViewModel
    {
        private int _translationY;
        private bool _opened;

        public int TranslationY
        {
            get => _translationY;
            set => SetProperty(ref _translationY, value);
        }
        public bool Opened
        {
            get => _opened;
            set => SetProperty(ref _opened, value);
        }  

        public UIStateViewModel()
        {
            TranslationY = 650;
        } // End Constructor
        public async Task PanUpdate(EventParams e)
        {
            if (e.Sender is not Border border) return;
            switch (e.EventArgs?.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Running:
                    border?.SetBinding(Border.TranslationYProperty, new Binding("TranslationY", source: this));
                    int previous = TranslationY + (int)e.EventArgs.TotalY;
                    if (previous < 0)
                    {
                        if (e.EventArgs.TotalY < 0)
                        {
                            previous = 0;
                            Opened = true;
                        }
                    }
                    else Opened = false;
                    TranslationY = previous;
                    break;
                case GestureStatus.Canceled:
                case GestureStatus.Completed:
                    int finalY = TranslationY <= 500 ? 0 : 650;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    await border.TranslateTo(border.TranslationX, finalY, 250, Easing.Linear);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
                    if (finalY == 0) Opened = true;
                    else Opened = false;
                    TranslationY = finalY;
                    break;
            }
        }
    }

}
