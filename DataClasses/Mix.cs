using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneHelper
{
    public class Mix
    {
        #region Fields

        #endregion

        #region Properties
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        #endregion

        #region Constructors

        public Mix(string id, string name, string desc)
        {
            ID = id;
            Name = name;
            Description = desc;
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion
    }
}
