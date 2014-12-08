using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TimeZoneHelper.WeatherApiConnect
{
    public class WeatherRequester
    {
        #region Fields

        public static string WeatherRequestUrl =
            "http://api.worldweatheronline.com/free/v2/weather.ashx?";
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

        public void AddNewUpdater(WeatherUpdater updater)
        {
            UpdaterList.Add(updater);
        }


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
                    FireUpdaters();
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
                    string json =
                        await GetWeatherUpdateFromWeb(weatherUpdater.QueryString);
                    if (!String.IsNullOrEmpty(json))
                    {
                        weatherUpdater.UpdateJson = json;
                    }
                }
                UpdateSuccessful = true;
            }
        }

        public async Task<String> GetWeatherUpdateFromWeb(string requestUrl)
        {
            string json;

            var memoryStream = new MemoryStream();

            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);

            WebResponse response = webRequest.GetResponse();
            
                using (Stream responseStream = response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(memoryStream);
                }
            


            using (memoryStream)
            {
                memoryStream.Position = 0;

                var reader = new StreamReader(memoryStream);

                json = reader.ReadToEnd();

            }

            response.Dispose();

            memoryStream.Close();

            return json;
        }

        private void FireUpdaters()
        {
            foreach (var weatherUpdater in UpdaterList)
            {
                weatherUpdater.ParseWeatherJson();
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
