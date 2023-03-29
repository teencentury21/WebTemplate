using System;
using System.Runtime.Serialization;

namespace SYS.BLL.Entities.GAIA
{
    [DataContract]
    public class SSOResult
    {
        [DataMember]
        public bool IsSuccessful
        {
            get;
            set;
        }

        [DataMember]
        public string ErrorMessage
        {
            get;
            set;
        }

        [DataMember]
        public string Data
        {
            get;
            set;
        }
    }
}
