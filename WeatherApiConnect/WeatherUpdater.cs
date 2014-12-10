using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace TimeZoneHelper.WeatherApiConnect
{
    public class WeatherUpdater : IWeatherUpdated
    {
        #region Fields
        #endregion

        #region Properties
        public string QueryString { get; set; }
        public string UpdateJson { get; set; }
        #endregion

        #region Constructors

        public WeatherUpdater(string cityName)
        {
          GenerateQueryString(cityName);
        }

        #endregion

        #region Methods

        public void GenerateQueryString(string name)
        {
            name = name.Trim();
            name = name.Replace(' ', '_');


            StringBuilder sb = new StringBuilder();
            sb.Append(WeatherRequester.WeatherRequestUrl);
            sb.Append("key=");
            sb.Append(ApiKey.Key);
            sb.Append("&q=");
            sb.Append(name);
            sb.Append("&format=json");

            QueryString= sb.ToString();
        }

        public void ParseWeatherJson()
        {
          var results = JsonConvert.DeserializeObject<dynamic>(UpdateJson);
          var data = results.data;
          var conditions = data.current_condition;
          var values = conditions[0];
            var args = new WeatherEventArgs();
            args.Fahrenheit = values.temp_F + " °F";
            args.Celsius = values.temp_C + " °C";
            if (!String.IsNullOrEmpty(args.Fahrenheit))
            {
                WeatherUpdateEvent.Invoke(args);
            }
        }

        #endregion

        #region Events

        #endregion

        public event NotifyWeatherUpdatedEvent WeatherUpdateEvent;
    }

    public interface IWeatherUpdated
    {
        event NotifyWeatherUpdatedEvent WeatherUpdateEvent;
    }

    public delegate void NotifyWeatherUpdatedEvent(WeatherEventArgs args);

    public class WeatherEventArgs
    {
        public string Fahrenheit { get; set; }
        public string Celsius { get; set; }
    }


}
