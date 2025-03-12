using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace HttpLinkTittles.Client
{
    public class HttpClientHeader
    {
    
       private readonly HttpClient httpClient = new HttpClient() { };

        Dictionary<string, string> dicts = new Dictionary<string, string>() { };  // No used

        public HttpClientHeader()
        {

            httpClient.Timeout = TimeSpan.FromSeconds(3);

        }


        

        public async Task<string> HttpClientReques(string urlInput) {

            dicts.Clear();

            string[] urls = urlInput.Split('\n');
            string result = "";
            string title = "";

            foreach (string url in urls)
            {
                if (url.Length < 5)
                {
                    continue;
                }

                if (!url.Contains("http://") && !url.Contains("https://"))
                {
                    continue;
                }

                title = await GetHttpClientMessage(url);

                result += $"### {title}\n";
                result += url + "\n\n";

                Debug.WriteLine($"HttpClientReques: {title}\n");

            }

            return result;

        }


        public async Task<string> GetHttpClientMessage(string url)
        {


            string result = "";


            try
            {


                HttpResponseMessage response = await httpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"{jsonResponse}\n");
                string[] urlArray;


                result = ParseWeiXinMessage(jsonResponse);

                if (result.Length < 5)
                {

                    result = ParseCnblogMessage(jsonResponse);


                }

                if (result.Length < 5)
                {


                    if (url.Contains("gitee.com") || url.Contains("github.com"))
                    {

                        urlArray = url.Split(new char[] { '/' });
                        if (urlArray.Length > 0)
                        {

                            result = urlArray[urlArray.Length - 1];
                        }

                    }

                }




            }
            catch (Exception e) {


                Debug.WriteLine("Exception: " + e.Message);
            
            }


            return result;


        }



        private string ParseWeiXinMessage(string jsonResponse)
        {

            Regex reg = new Regex("<meta property=\"og:title\" content=\"[＜＞\\/\\（\\）\\(\\)-—【】\\[\\]?？~+#\\%：.，。、！!\\*\\-\\s\u4e00-\u9fa5_a-zA-Z0-9]+\" />"); //接受所有匹配到的项 MatchCollection
            var result = reg.Match(jsonResponse);

            if (result.Length < 5)
            {


                return "";
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result.ToString());

            string value = doc.SelectSingleNode("/meta").Attributes["content"]?.Value;

            return value;
        }



        private string ParseCnblogMessage(string jsonResponse)
        {


            Regex reg = new Regex("<title>[/【】:：（）？\\(\\)#、·.-\\[\\]\\*\\-\\—\\s\u4e00-\u9fa5_a-zA-Z0-9]+</title>"); //接受所有匹配到的项 MatchCollection
            var result = reg.Match(jsonResponse);

            if (result.Length < 5)
            {


                return "";
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result.ToString());

            XmlNode node = doc?.SelectSingleNode("/title");   // 
            string value = node?.InnerText;


            return value;

        }


        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


    }



    public static class ShowResponseMessage
    {

        public static void WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response is null)
            {
                return;
            }

            var request = response.RequestMessage;
            Debug.Write($"{request?.Method} ");
            Debug.Write($"{request?.RequestUri} ");
            Debug.WriteLine($"HTTP/{request?.Version}");
        }



    }
}
