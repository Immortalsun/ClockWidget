using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneHelper
{
    public class Location
    {
        #region Fields

        #endregion

        #region Properties
        public string CityName { get; set; }
        public string Country { get; set; }
        public TimeSpan UTCOffset { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        #endregion

        #region Constructors

        public Location(string cityName, string country, string utcoffset)
        {
            CityName = cityName;
            Country = country;
            var utc = (int)Double.Parse(utcoffset);
            UTCOffset = new TimeSpan(utc,0,0);
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
    }
}
