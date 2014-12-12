
namespace TimeZoneHelper
{
    public class MusicUser
    {
        #region Fields
        #endregion

        #region Properties
        public string UserName { get; set; }
        public string Password { get;  set; }
        public string Email { get; set; }
        public string UserToken { get; set; }
        #endregion

        #region Constructors

        public MusicUser(string username, string password, string emailAddress = null)
        {
            UserName = username;
            Password = password;
            Email = emailAddress;
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion
    }
}
