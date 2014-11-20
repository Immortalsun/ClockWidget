using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TimeZoneHelper
{
    public class WorldClock : INotifyPropertyChanged
    {
        public string TimeZone { get; set; }
        public string LocationName { get; set; }
        [XmlIgnore]
        public string Time { get; set; }


        public WorldClock(string name, string zone)
        {
            LocationName = name;
            TimeZone = zone;

        }

        public WorldClock()
        {
            LocationName = "";
            TimeZone = "";
        }

        public void Update()
        {
            DateTime currentTime = DateTime.Now;
            DateTime time = TimeZoneInfo.ConvertTime(currentTime,
                TimeZoneInfo.FindSystemTimeZoneById(TimeZone));
            Time = time.ToShortTimeString();
            OnPropertyChanged("Time");
        }

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
