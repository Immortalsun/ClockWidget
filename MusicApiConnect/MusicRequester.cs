using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneHelper.MusicApiConnect
{
    public class MusicRequester
    {
        #region Fields

        public static string MusicRequestURL = "http://8tracks.com/mixes/1.json";
        public static string MusicLoginURL = "https://8tracks.com/sessions.json";
        public static string MusicRegisterURL = "https://8tracks.com/users.json";
        #endregion

        #region Properties
        public bool LoginSuccessful { get; set; }
        #endregion

        #region Constructors

        #endregion

        #region Methods

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

            WebResponse response = await request.GetResponseAsync();

            requestStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(requestStream);

            var responsejson = reader.ReadToEnd();

            reader.Close();
            requestStream.Close();
            response.Close();

            return responsejson;
        }


        public async Task<string> Login(MusicUser user)
        {
            var loginString = GenerateLoginRequest(user);
            var json = await PostRequestToMusicServer(loginString, MusicLoginURL);
            return json;
        }

        public async Task<string> Register(MusicUser user)
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

        public string GenerateLoginRequest(MusicUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("login=");
            sb.Append(user.UserName);
            sb.Append("&password=");
            sb.Append(user.Password);
            sb.Append("&api_version=3");
            return sb.ToString();
        }

        public string GenerateRegisterRequest(MusicUser user)
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

        #region Events

        #endregion
    }
}
