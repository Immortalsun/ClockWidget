using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimeZoneHelper.WeatherApiConnect
{
    public class LocationSearcher
    {
        #region Fields

        #endregion

        #region Properties
        public string QueryString { get; set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void GenerateSearchQuery(string locationName)
        {
            locationName = locationName.Trim();
            locationName = locationName.Replace(' ', '_');

            StringBuilder sb = new StringBuilder();
            sb.Append(WeatherRequester.WeatherSearchURL);
            sb.Append("key=");
            sb.Append(ApiKey.Key);
            sb.Append("&q=");
            sb.Append(locationName);
            sb.Append("&timezone=yes");
            //sb.Append("&popular=yes"); //narrow results to only show popular cities
            sb.Append("&format=json");

            QueryString = sb.ToString();
        }

        public List<Location> ParseLocationJson(string json)
        {

            var locationList = new List<Location>();
            try
            {
                var results = JsonConvert.DeserializeObject<dynamic>(json);

                var searchApi = results.search_api;
                var searchResult = searchApi.result;

                var searchArray = (JArray) searchResult;

                foreach (var element in searchArray)
                {
                    var area = element.SelectToken("areaName");
                    var cityName = area[0].Value<string>("value");
                    var country =
                        element.SelectToken("country")[0].Value<string>("value");
                    if (country.Equals("United States of America"))
                    {
                        country =
                            element.SelectToken("region")[0].Value<string>(
                                "value");
                    }
                    var utc =
                        element.SelectToken("timezone")[0].Value<string>("offset");
                    locationList.Add(new Location(cityName, country, utc));

                }
            }
            catch (Exception ex)
            {
                locationList.Add
                (new Location("Search returned no results.", "Try Again", "0"));
                return locationList;
            }

            return locationList;
        }

        #endregion

        #region Events

        #endregion
    }
}
