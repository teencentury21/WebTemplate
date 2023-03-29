using SYS.BLL.Base;
using SYS.BLL.Entities;
using SYS.DAL.GAIA;
using SYS.DAL.Base;
using SYS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYS.BLL.Domain.GAIA.SSO;
using SYS.BLL.Entities.GAIA;

namespace SYS.BLL.Domain.GAIA
{
    public interface IGAIALogic : IDataDrivenLogic
    {
        // Repository
        IOmStaffRepository _OmStaffRepository { get; set; }
        IOmUserRepository _OmUserRepository { get; set; }

        EmpEntity GetEmpByEmpNo(string empNo);
        List<EmpEntity> GetEmpByName(string lang, string name);
        string GetGAIASSOredirectURL(string apiKey, string sourceSystem, string targetSystem, string mySystemSid, string targetPage);
    }
    internal class GAIALogic : DataDrivenLogic, IGAIALogic
    {
        public IOmStaffRepository _OmStaffRepository { get;set; }
        public IOmUserRepository _OmUserRepository { get;set; }

        public GAIALogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _OmStaffRepository = CreateSqlRepository<IOmStaffRepository>(Database.GAIADev);
            _OmUserRepository = CreateSqlRepository<IOmUserRepository>(Database.GAIADev);
        }
        public EmpEntity GetEmpByEmpNo(string empNo)
        {
            var result = new EmpEntity();
            try
            {      
                var omstaff = _OmStaffRepository.GetItemByEmpNo(empNo);

                if (omstaff != null)
                {
                    result.EmpId = omstaff.id;
                    result.EmpNo = omstaff.staff_no;
                    result.Name = omstaff.name;
                    result.EnName = omstaff.alias_name;
                    result.Email = omstaff.email;
                    result.Site = omstaff.site_code;
                    result.ExtNo = omstaff.ext_no;
                    result.IsDL = omstaff.email == "DL" ? "DL" : "IDL";
                    result.IsActive = omstaff.is_active;
                    var omuser = _OmUserRepository.GetItemByEmpName(omstaff.name);
                    if (omuser != null && omuser.user_name != "")
                    {
                        result.GAIAId = omuser.id;
                    }
                }
            }
            catch (Exception ex)
            {
                result.remark = ex.ToString();
            }

            return result;            
        }
        public List<EmpEntity> GetEmpByName(string lang, string name)
        {
            var result = new List<EmpEntity>();
            try
            {
                var staffs = _OmStaffRepository.GetEmpByName(lang, name);
                foreach (var item in staffs)
                {
                    var omuser = _OmUserRepository.GetItemByEmpName(item.name);
                    result.Add(new EmpEntity
                    {
                        EmpId = item.id,
                        EmpNo = item.staff_no,
                        Name = item.name,
                        EnName = item.alias_name,
                        Email = item.email,
                        Site = item.site_code,
                        ExtNo = item.ext_no,
                        IsDL = item.email == "DL" ? "DL" : "IDL",
                        IsActive = item.is_active,
                        GAIAId = omuser != null ? omuser.id : "",
                    });
                }
            }
            catch (Exception ex)
            {
                result.Add(new EmpEntity { remark = ex.ToString() });
            }
            return result;
        }
        public string GetGAIASSOredirectURL(string apiKey, string sourceSystem, string targetSystem, string mySystemSid, string targetPage)
        {
            //SSO service url
            string remoteUrl = "https://www.darfon.com.tw/gaia";

            // if not exists, please go regist flow.
            RemoteSSORouteService remoteSSORoute = new RemoteSSORouteService();
            /*
            1. 根據SSO網站（API/SSO）所需參數：APIKEY, SOURCESYSTEM, TARGETSYSTEM, TARGETURL，定義它們的值
            2. 從SSO網站（API/SSO）得到帶Token和加密參數的相對URL(SSO/RouteIn?參數)（因為apikey是不能暴露給用戶，所以在server端，使用webrequest請求）
             */
            SSOResult result = remoteSSORoute.GetRouteInUrl(remoteUrl, apiKey, sourceSystem, targetSystem, mySystemSid, "zh-TW", targetPage);

            if (!result.IsSuccessful)
            {
                return result.ErrorMessage;
            }
            else
            {
                string goToUrl = result.Data;

                //3. 目標網站URL和1,2步中得到的URL組成目標網站的RouteIn全路徑，進行跳轉
                //bot.ReplyMessage(ReplyToken, string.Format("{0}/{1}", "http://dfe-gaiadev.dty.darfon.com", goToUrl));
                var redirectURL = string.Format("{0}/{1}", remoteUrl, goToUrl);
                return redirectURL;
            }
        }
    }
}
