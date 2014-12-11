using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneHelper
{
    public class MusicUser
    {
        #region Fields
        #endregion

        #region Properties
        public string UserName { get; private set; }
        public string Password { get; private set; }
        #endregion

        #region Constructors

        public MusicUser(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion
    }
}
