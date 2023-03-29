using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model
{
    public class AppSettings
    {
        public static readonly string FileName = "appsettings.json";
        public string Stage { get; set; } = default;
        public Dictionary<string, Dictionary<string, string>> Connections { get; set; } = default;
    }
}
