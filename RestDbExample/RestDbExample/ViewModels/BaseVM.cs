using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }

        int _busyCounter;
        public int BusyCounter
        {
            get { return _busyCounter; }
            set
            {
                if (value == _busyCounter)
                    return;
                _busyCounter = value;
                if (_busyCounter == 0)
                    IsBusy = false;
                else
                    IsBusy = true;
            }
        }

        string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                if (_pageTitle == value) return;
                _pageTitle = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName]String propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null == handler) return;
            try
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
