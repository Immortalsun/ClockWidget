using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;

namespace TimeZoneHelper.MusicApiConnect
{
    public class MusicDataHandler
    {
        #region Fields
        #endregion

        #region Properties
        public MusicUser User { get; set; }
        public string LoginResultJson { get; set; }
        public string RegisterResultJson { get; set; }
        public bool LoginSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods

        public async Task<bool> RegisterNewUser(string userName, string password,
            string emailAddress)
        {
            User = new MusicUser(userName, password, emailAddress);
            RegisterResultJson = await MusicRequester.Register(User);
            return LoginSuccessful;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            User = new MusicUser(username, password);
            LoginResultJson = await MusicRequester.Login(User);
            GetUserTokenFromLoginJson();
            return LoginSuccessful;
        }


        public void GetUserTokenFromLoginJson()
        {
            if (String.IsNullOrEmpty(LoginResultJson))
            {
                ErrorMessage = "Login error"+"\n"+"Check your username and password";
                return;
            }
            var results = JsonConvert.DeserializeObject<dynamic>(LoginResultJson);
            try
            {
                var userData = results.user;
                var token = userData.user_token;
                User.UserToken = token;
                LoginSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = results.errors;
                LoginSuccessful = false;
            }
        }

        public void GetUserTokenFromRegisterJson()
        {
            if (String.IsNullOrEmpty(RegisterResultJson))
            {
                ErrorMessage = "Registration failed"+"\n"+"Try again in a moment";
                return;
            }
            var results = JsonConvert.DeserializeObject<dynamic>(RegisterResultJson);
            try
            {
                var type = results.type;
                var user = type.user;
                var token = user.user_token;
                User.UserToken = token;
                LoginSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = results.errors;
                LoginSuccessful = false;
            }
        }



        #endregion

        #region Events

        #endregion
    }
}
