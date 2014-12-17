using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneHelper.DataClasses
{
    public class SearchType
    {
        #region Fields

        #endregion

        #region Properties
        public string DisplayName { get; set; }
        public SearchOption Option { get; set; }
        #endregion

        #region Constructors

        public SearchType(string name, SearchOption option)
        {
            DisplayName = name;
            Option = option;
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

        public enum SearchOption
        {
            Artist,
            Tag,
            Keyword,
            SimilarToMix
        }
    }
}
