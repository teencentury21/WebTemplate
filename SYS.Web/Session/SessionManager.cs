using System;
using System.Web;

namespace SYS.Web.Session
{
    public sealed class SessionManager
    {
        private const string ERROR_INFO = "ErrorInfo";
        private const string IMAGECODE = "ImageCode";
        private const string Log_ByUser = "N";

        /// <summary>
        /// User Name       
        /// </summary>
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["u_name"] == null)
                {
                    return "";
                }
                return HttpContext.Current.Session["u_name"].ToString();
            }
            set
            {
                HttpContext.Current.Session["u_name"] = value;
            }
        }

        /// <summary>
        /// User role level
        /// </summary>        
        public static string UserRole
        {
            get
            {
                if (HttpContext.Current.Session["u_role"] == null)
                {
                    return "";
                }
                return HttpContext.Current.Session["u_role"].ToString();
            }
            set
            {
                HttpContext.Current.Session["u_role"] = value;
            }
        }
        /// <summary>
        /// Is user already login        
        /// </summary>
        public static string IsLogin
        {
            get
            {
                if (HttpContext.Current.Session["Login"] == null)
                {
                    return "";
                }
                return HttpContext.Current.Session["Login"].ToString();
            }
            set
            {
                HttpContext.Current.Session["Login"] = value;
            }
        }


        public static string getErrorID()
        {
            HttpContext.Current.Application.Lock();
            if (HttpContext.Current.Application["Error_ID"] == null)
            {
                return "";
            }
            HttpContext.Current.Application["Error_ID"] = (Convert.ToInt32(HttpContext.Current.Application["Error_ID"]) + 1).ToString();
            HttpContext.Current.Application.UnLock();

            return HttpContext.Current.Application["Error_ID"].ToString();
        }
        public static string ErrorInfo
        {
            get
            {
                if (HttpContext.Current.Session["ErrorInfo"] == null)
                {
                    return "unhandle error";
                }
                return HttpContext.Current.Session["ErrorInfo"].ToString();
            }
            set
            {
                HttpContext.Current.Session["ErrorInfo"] = value;
            }
        }

        //是否將 Log 按 User 存放同一個 Log 檔案, 方便 trace
        public static string LogByUser
        {
            get
            {
                if (HttpContext.Current.Session["LogByUser"] == null)
                {
                    return "N";
                }
                return HttpContext.Current.Session["LogByUser"].ToString();
            }
            set
            {
                HttpContext.Current.Session["LogByUser"] = value;
            }
        }

        //trace log 是否開啟
        public static string LogTrace
        {
            get
            {
                if (HttpContext.Current.Session["Log_Trace"] == null)
                {
                    return "N";
                }
                return HttpContext.Current.Session["Log_Trace"].ToString();
            }
            set
            {
                HttpContext.Current.Session["Log_Trace"] = value;
            }
        }

        //組織選單
        public static string OrgMap_DeptCode
        {
            get
            {
                if (HttpContext.Current.Session["OrgMap_DeptCode"] == null)
                {
                    return "";
                }
                return HttpContext.Current.Session["OrgMap_DeptCode"].ToString();
            }
            set
            {
                HttpContext.Current.Session["OrgMap_DeptCode"] = value;
            }
        }

        //sessiontimeout
        public static string SessionTimeout
        {
            get
            {
                if (HttpContext.Current.Session["SessionTimeout"] == null)
                {
                    return "";
                }
                return HttpContext.Current.Session["SessionTimeout"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SessionTimeout"] = value;
            }
        }

        //登入失敗等待時間
        public static string LoginFailWaitMinute
        {
            get
            {
                if (HttpContext.Current.Session["Login_Fail_WaitMinute"] == null)
                {
                    return "15"; //預設等待 15 分鐘
                }
                return HttpContext.Current.Session["Login_Fail_WaitMinute"].ToString();
            }
            set
            {
                HttpContext.Current.Session["Login_Fail_WaitMinute"] = value;
            }
        }
        //連續登入失敗允許次數
        public static string LoginFailLimitCount
        {
            get
            {
                if (HttpContext.Current.Session["Login_Fail_LimitCount"] == null)
                {
                    return "3"; //預設 3 次
                }
                return HttpContext.Current.Session["Login_Fail_LimitCount"].ToString();
            }
            set
            {
                HttpContext.Current.Session["Login_Fail_LimitCount"] = value;
            }
        }

        public static string getHttpContext(string i_Key)
        {
            if (HttpContext.Current.Session[i_Key] == null)
                return "";
            else
                return HttpContext.Current.Session[i_Key].ToString();
        }
        public static string getHttpContext(string i_Key, string i_default)
        {
            if (HttpContext.Current.Session[i_Key] == null)
                return i_default;
            else
                return HttpContext.Current.Session[i_Key].ToString();
        }

        public static string ImageCode
        {
            get
            {
                string f_return = "";
                try
                {
                    if (HttpContext.Current.Session["ImageCode"] == null)
                    {
                        f_return = "Get image code error";
                    }
                    else
                    {
                        f_return = HttpContext.Current.Session["ImageCode"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    f_return = "Get image code error";
                    // LogManager.WriteLog_Exception("SYSTEM", "SessionManager", "ImageCode Fail", ex);
                    throw new Exception("ImageCode Fail", ex);
                }
                return f_return;
            }
            set
            {
                HttpContext.Current.Session["ImageCode"] = value;
            }
        }

        public SessionManager()
        {
        }
    }
}
