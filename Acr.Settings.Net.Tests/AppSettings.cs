using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Acr.Settings.Net.Tests
{
    public class AppSettings : INotifyPropertyChanged
    {
        int blah = 0;
        public int Blah
        {
            get { return this.blah; }
            set
            {
                this.blah = value;
                this.OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
