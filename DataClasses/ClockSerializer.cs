using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TimeZoneHelper
{
    public class ClockSerializer 
    {


        #region Fields

        #endregion

        #region Properties
        public ClockList TimeList { get; private set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void SetClocks(ObservableCollection<WorldClock> clocks)
        {
            TimeList = new ClockList();
            TimeList.Clocks = new List<WorldClock>();
            TimeList.Clocks.AddRange(clocks);
        }

        public void SetColors(int a, int r, int g, int b)
        {
            TimeList.Alpha = a;
            TimeList.Red = r;
            TimeList.Green = g;
            TimeList.Blue = b;
        }

        public void Serialize()
        {
            try
            {
                if (TimeList != null && TimeList.Clocks.Any())
                {
                    var writer = new XmlSerializer(typeof (ClockList));
                    var path = Path.GetTempPath();
                    var fileName = "savedClocks.xml";
                    var fullPath = Path.Combine(path, fileName);
                    StreamWriter file = new StreamWriter(fullPath, false);

                    writer.Serialize(file, TimeList);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public ClockList Deserialize()
        {
            try
            {
                var reader = new XmlSerializer(typeof(ClockList));
                var path = Path.GetTempPath();
                var fileName = "savedClocks.xml";
                var fullPath = Path.Combine(path, fileName);
                if (!File.Exists(fullPath))
                {
                    return null;
                }
                FileInfo info = new FileInfo(fullPath);
                if (info.Length > 0)
                {
                    var file = new StreamReader(fullPath);
                    TimeList = new ClockList();
                    TimeList = (ClockList)reader.Deserialize(file);
                    if (TimeList != null)
                        return TimeList;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return null;
        }

        #endregion

        #region Events

        #endregion
    }

    public class ClockList
    {
        public List<WorldClock> Clocks { get; set; }
        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
