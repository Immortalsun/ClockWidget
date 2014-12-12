using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeZoneHelper
{
    public class Location : INotifyPropertyChanged  
    {
        #region Fields

        private bool _isChecked;
        #endregion

        #region Properties
        public string CityName { get; set; }
        public string Country { get; set; }
        public string UTCString { get; set; }
        public TimeSpan UTCOffset { get; set; }
        public TimeZoneInfo TimeZone { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors

        public Location(string cityName, string country, string utcoffset)
        {
            CityName = cityName;
            Country = ", "+country;
            UTCString = "UTC ";
            var utc = (int)Double.Parse(utcoffset);
            UTCOffset = new TimeSpan(utc, 0, 0);
            if (utc > 0)
            {
                UTCString += "+" + utc + ":00";
            }
            else
            {
                UTCString += utc + ":00";
            }
            SetTimeZone();
        }


        #endregion

        #region Methods

        public void SetTimeZone()
        {
            var collect = TimeZoneInfo.GetSystemTimeZones();
            TimeZone =
                collect.FirstOrDefault(p => p.BaseUtcOffset.Equals(UTCOffset));
        }


        #endregion

        #region Events

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
