using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TimeZoneHelper.DataClasses;

namespace TimeZoneHelper.MusicApiConnect
{
    public static class MusicRequester
    {
        #region Fields

        public static string MusicMixRequestURL = "http://8tracks.com/mixes";

        public static string MusicMixSetRequestUrl =
            "http://8tracks.com/mix_sets/";
        public static string MusicLoginURL = "https://8tracks.com/sessions.json";
        public static string MusicRegisterURL = "https://8tracks.com/users.json";
        #endregion

        #region Properties
        #endregion

        #region Constructors

        #endregion

        #region Methods

        #region Static Web Request methods
        public static async Task<String> PostRequestToMusicServer
            (string data, string url, List<Tuple<string,string>> headers = null)
        {
             var request =
                (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Item1, header.Item2);
                }
            }

            string requestString = data;

            byte[] requestArr = Encoding.UTF8.GetBytes(requestString);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = requestArr.Length;

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(requestArr, 0, requestArr.Length);

            requestStream.Close();

            var responsejson = "";
            try
            {

                WebResponse response = await request.GetResponseAsync();

                requestStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(requestStream);

                 responsejson = reader.ReadToEnd();

                reader.Close();
                requestStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Trace.Write(ex);
            }



            return responsejson;
        }

        public static async Task<String> GetRequestToMusicServer(string url,
            List<Tuple<string,string>> headers = null)
        {
            string json;

            var memoryStream = new MemoryStream();

            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            WebResponse response = webRequest.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                await responseStream.CopyToAsync(memoryStream);
            }

            using (memoryStream)
            {
                memoryStream.Position = 0;

                var reader = new StreamReader(memoryStream);

                json = reader.ReadToEnd();

            }

            response.Dispose();

            memoryStream.Close();

            return json;
        }
        #endregion


        #region User Login and Registration
        public static async Task<string> Login(MusicUser user)
        {
            var loginString = GenerateLoginRequest(user);
            var json = await PostRequestToMusicServer(loginString, MusicLoginURL);
            return json;
        }

        public static async Task<string> Register(MusicUser user)
        {
            var registerString = GenerateRegisterRequest(user);
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Api-Key:", ApiKey.MusicKey),
                new Tuple<string, string>("X-Api-Version:", "3")
            };
            var json = await PostRequestToMusicServer
                (registerString, MusicRegisterURL, headers);

            return json;
        }

        public static async Task<string> SearchMixSets(
            string searchString,
            SearchType type)
        {

            return "";
        }

        #endregion

        #region Generate Request String Methods
        public static string GenerateLoginRequest(MusicUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("login=");
            sb.Append(user.UserName);
            sb.Append("&password=");
            sb.Append(user.Password);
            sb.Append("&api_version=3");
            return sb.ToString();
        }

        public static string GenerateRegisterRequest(MusicUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("user[login]=");
            sb.Append(user.UserName);
            sb.Append("&user[password]=");
            sb.Append(user.Password);
            sb.Append("&user[email]=");
            sb.Append(user.Email);
            sb.Append("&user[agree_to_terms]=1");

            return sb.ToString();
        }
        #endregion


        #endregion

        #region Events

        #endregion
    }
}
