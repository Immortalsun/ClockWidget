using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeZoneHelper.DataClasses;

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
        public string SearchResultJson { get; set; }
        public bool LoginSuccessful { get; set; }
        public bool SearchReturnedResults { get; set; }
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

        public async Task<List<Mix>> SearchMixSets(
            string query, SearchType type)
        {
            SearchResultJson = await MusicRequester.SearchMixSets(query, type, User);
            var results = ParseSearchResults();
            return results;
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

        public List<Mix> ParseSearchResults()
        {
            if (string.IsNullOrEmpty(SearchResultJson))
            {
                SearchReturnedResults = false;
                ErrorMessage = "Search returned no results. Try again";
                return null;
            }
            var results = JsonConvert.DeserializeObject<dynamic>(SearchResultJson);
            try
            {
                var mixList = new List<Mix>();
                var mixSets = results.mix_set;
                var mixArray = (JArray)mixSets.mixes;

                foreach (var mix in mixArray)
                {
                    var id = mix.SelectToken("id").Value<string>();
                    var name = mix.SelectToken("name").Value<string>();
                    var desc = mix.SelectToken("description").Value<string>();

                    mixList.Add(new Mix(id, name, desc));
                }


                return mixList;
            }
            catch (Exception ex)
            {
                ErrorMessage = results.errors;
                SearchReturnedResults = false;
            }
            return null;
        }



        #endregion

        #region Events

        #endregion
    }
}
