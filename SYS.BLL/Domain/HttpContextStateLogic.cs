using SYS.BLL.Base;
using SYS.DAL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Domain
{
    public interface IHttpContextStateLogic : IDataDrivenLogic
    {
        void SetState(string Key, string Value);
        string GetState(string Key);
    }
    internal class HttpContextStateLogic : DataDrivenLogic, IHttpContextStateLogic
    {
        public HttpContextStateLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {

        }
        public void SetState(string Key, string Value)
        {
            System.Web.HttpContext.Current.Application[Key] = Value;
        }
        public string GetState(string Key)
        {
            if (System.Web.HttpContext.Current.Application[Key] == null) return null;
            return System.Web.HttpContext.Current.Application[Key].ToString();
        }
    }


}
