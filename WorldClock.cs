﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TimeZoneHelper.WeatherApiConnect;

namespace TimeZoneHelper
{
    public class WorldClock : INotifyPropertyChanged
    {
        public string TimeZone { get; set; }
        public string LocationName { get; set; }
        [XmlIgnore]
        public string Time { get; set; }

        [XmlIgnore]
        public string CurrentTemp { get; set; }

        [XmlIgnore]
        public WeatherUpdater Updater { get; set; }


        public WorldClock(string name, string zone)
        {
            LocationName = name;
            TimeZone = zone;
            Updater = new WeatherUpdater(name);
            Updater.WeatherUpdateEvent += UpdateCurrentTemp;
            CurrentTemp = "loading...";
        }

        public WorldClock()
        {
            LocationName = "";
            TimeZone = "";
            CurrentTemp = "loading...";
        }

        public void Update()
        {
            DateTime currentTime = DateTime.Now;
            DateTime time = TimeZoneInfo.ConvertTime(currentTime,
                TimeZoneInfo.FindSystemTimeZoneById(TimeZone));
            Time = time.ToShortTimeString();
            OnPropertyChanged("Time");
        }

        public void UpdateCurrentTemp(WeatherEventArgs args)
        {
            CurrentTemp = args.NewTemp;
            OnPropertyChanged("CurrentTemp");
        }

        public void ResetUpdater()
        {
            Updater = new WeatherUpdater(LocationName);
            Updater.WeatherUpdateEvent += UpdateCurrentTemp;
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
