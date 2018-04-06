using System.Net;
using System.Text;

namespace Reptitle.BusinessLogic
{
    public class DownloadData
    {
        public static string GetDownloadData(string url)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            return Encoding.UTF8.GetString(MyWebClient.DownloadData(url));
        }
    }
}
