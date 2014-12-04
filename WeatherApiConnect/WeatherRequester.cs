using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeZoneHelper.WeatherApiConnect
{
    public class WeatherRequester
    {
        #region Fields

        public static string WeatherRequestUrl =
            "http://www.ncdc.noaa.gov/cdo-web/api/v2/locations";
        private readonly object _threadLock = new object();
        private TimeSpan _updateTime;
        private List<WeatherUpdater> UpdaterList;
        private volatile bool Updating;

        #endregion

        #region Properties
        public bool UpdateSuccessful { get; set; }
        #endregion

        #region Constructors

        public WeatherRequester()
        {
            UpdaterList = new List<WeatherUpdater>();
            _updateTime = new TimeSpan(0,15,0);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Asyc start method for 
        /// updating weather
        /// </summary>
        public async void Start()
        {
            Updating = true;
            while (Updating)
            {
                await UpdateWeather();
                if (UpdateSuccessful)
                {
                    lock (_threadLock)
                    {
                        Monitor.Wait(_threadLock, _updateTime);
                    }
                }
                else
                {
                    Updating = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Async update method, 
        /// actually does requests
        /// </summary>
        /// <returns></returns>
        public async Task UpdateWeather()
        {
            UpdateSuccessful = false;

            if (UpdaterList.Any())
            {
                foreach (var weatherUpdater in UpdaterList)
                {
                    
                }
            }
        }

        /// <summary>
        /// Stops weather updating thread
        /// </summary>
        public void Stop()
        {
            Updating = false;

            lock (_threadLock)
            {
                Monitor.Pulse(_threadLock);
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
