using System;
using System.Windows.Media;
using Newtonsoft.Json;

namespace TimeZoneHelper.MusicApiConnect
{
    public class MusicPlayer
    {
        #region Fields

        private MediaPlayer _player;
        private MusicRequester _requester;
        #endregion

        #region Properties
        public MusicUser User { get; set; }
        public string LoginResultJson { get; set; }
        public string RegisterResultJson { get; set; }
        public bool LoginSuccessful { get; set; }

        #endregion

        #region Constructors

        public MusicPlayer()
        {
            _player = new MediaPlayer();
            _requester = new MusicRequester();
        }

        #endregion

        #region Methods

        public async void RegisterNewUser(string userName, string password,
            string emailAddress)
        {
            User = new MusicUser(userName, password, emailAddress);
            RegisterResultJson = await _requester.Register(User);
        }

        public async void LoginUser(string username, string password)
        {
            User = new MusicUser(username, password);
            LoginResultJson = await _requester.Login(User);
            GetUserTokenFromLoginJson();
        }


        public void GetUserTokenFromLoginJson()
        {
            var results = JsonConvert.DeserializeObject<dynamic>(LoginResultJson);
            try
            {
                var userData = results.user;
                var token = userData.user_token;
                User.UserToken = token;
                _requester.LoginSuccessful= true;
            }
            catch (Exception ex)
            {
                var errorMsg = results.errors;
                _requester.LoginSuccessful = false;
            }
        }

        public void GetUserTokenFromRegisterJson()
        {
            var results = JsonConvert.DeserializeObject<dynamic>(RegisterResultJson);
            try
            {
                var type = results.type;
                var user = type.user;
                var token = user.user_token;
                User.UserToken = token;
                _requester.LoginSuccessful = true;
            }
            catch (Exception ex)
            {
                var errorMsg = results.errors;
                _requester.LoginSuccessful = false;
            }
        }



        #endregion

        #region Events

        #endregion
    }
}
