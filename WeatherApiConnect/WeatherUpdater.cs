using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace TimeZoneHelper.WeatherApiConnect
{
    public class WeatherUpdater
    {
        #region Fields

        #endregion

        #region Properties
        public string QueryString { get; set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void GenerateQueryString(string city, string state)
        {
            city = city.Trim();
            city = city.Replace(' ', '_');

            StringBuilder sb = new StringBuilder();
            sb.Append(WeatherRequester.WeatherRequestUrl);
            sb.Append(ApiKey.Key);
            sb.Append("/conditions/q/");
            sb.Append(state);
            sb.Append("/");
            sb.Append(city);
            sb.Append(".json");

            QueryString = sb.ToString();
        }

        public void ParseWeatherJson(JToken token)
        {
            JsonReader reader = new JTokenReader(token);

            while (reader.Read())
            {
                var type = reader.GetType();
                
            }

        }

        #endregion

        #region Events

        #endregion
    }
}
