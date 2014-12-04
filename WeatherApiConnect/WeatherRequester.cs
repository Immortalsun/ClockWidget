using System;
using System.Collections.Generic;
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
                    JToken token =
                        await GetWeatherUpdateFromWeb(weatherUpdater.QueryString);
                }
            }
        }

        public async Task<JToken> GetWeatherUpdateFromWeb(string requestUrl)
        {
            JToken token;

            var memoryStream = new MemoryStream();

            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);
            webRequest.Headers.Add("token", ApiKey.Key);

            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(memoryStream);
                }
            }

            using (memoryStream)
            {
                memoryStream.Position = 0;

                var reader = new StreamReader(memoryStream);

                var jsonString = reader.ReadToEnd();

                token = JObject.Parse(jsonString);
            }

            memoryStream.Close();

            return token;
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
