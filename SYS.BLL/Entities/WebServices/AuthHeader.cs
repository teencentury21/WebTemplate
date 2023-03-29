using System.Web.Services.Protocols;

namespace SYS.BLL.Entities.WebServices
{
    public class AuthHeader : SoapHeader
    {
        public string UserName;
        public string Password;
    }
}
