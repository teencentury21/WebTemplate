using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Base
{
    public class LogicMappingConfig
    {
        public List<LogicMapping> Mappings { get; set; }
    }
    public class LogicMapping
    {
        public string LogicType { get; set; }
        public string ImplementationType { get; set; }
    }
}
