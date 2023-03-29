using SYS.BLL.Entities.GAIA;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace SYS.BLL.Domain.GAIA.SSO
{
    public class RemoteSSORouteService
    {
        private T Deserialize<T>(string sJson) where T : class
        {
            if (String.IsNullOrEmpty(sJson))
                return default(T);

            JavaScriptSerializer ds = new JavaScriptSerializer();
            T obj = ds.Deserialize<T>(sJson);
            return obj;
        }

        private string Serialize<T>(T obj)
        {
            if (obj == null)
                return null;
            JavaScriptSerializer ds = new JavaScriptSerializer();
            return ds.Serialize(obj);
        }

        public SSOResult GetRouteInUrl(string remoteServerUrl, string APIKey, string sourceSystem, string targetSystem, string sid, string cultureCode, string targetUrl)
        {
            //URL gene 
            Uri namedUri = new Uri(string.Format("{7}/Api/SSO/gaia?APIKey={0}&sourceSystem={1}&targetSystem={2}&sid={3}&cultureCode={4}&targetUrl={5}&expiredTime={6}",
                APIKey, sourceSystem, targetSystem, sid, cultureCode, targetUrl, DateTime.UtcNow.AddDays(1).ToString("yyyy/MM/dd hh:mm:ss"), remoteServerUrl), UriKind.Absolute);

            HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(namedUri.AbsoluteUri);
            request.Method = "Get";
            request.ContentType = "application/json";

            string srcString = string.Empty;
            using (WebResponse wr = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new System.IO.StreamReader(wr.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    srcString = reader.ReadToEnd();
                }
            }

            SSOResult ssoResult;
            if (string.IsNullOrEmpty(srcString))
            {
                ssoResult = new SSOResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "connection to sso server error",
                    Data = String.Empty
                };
            }
            else
            {
                ssoResult = Deserialize<SSOResult>(srcString);
            }
            return ssoResult;

        }

        internal SSOResult GetRouteInUrl(string remoteUrl, string apiKey, string line, string gAIA, object mySystemSid, string v, string targetPage)
        {
            throw new NotImplementedException();
        }
    }
}
