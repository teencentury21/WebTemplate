using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.Constants
{
    public class CodeBase
    {
        /// <summary>
        /// 產生英文（大小寫）與數字。  為了避免誤判，通常不使用英文 Oo 與 Ll、大寫的I，但保留數字的 "零"。
        /// 因為數字出現的機率較少，所以我加入兩次數字
        /// </summary>
        public const string CodeBaseCaptcha = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz123456789";
        /// <summary>
        /// 0-9 A-Z
        /// </summary>
        public const string CodeBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// without IOQU
        /// </summary>
        public const string CodeBase32 = "0123456789ABCDEFGHJKLMNPRSTVWXYZ";
        public const string CodeBase16 = "0123456789ABCDEF";
        public const string CodeBase10 = "0123456789";

    }
}
