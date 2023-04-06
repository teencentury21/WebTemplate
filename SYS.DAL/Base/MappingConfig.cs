using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.Base
{
    public class MappingConfig
    {
        public List<Mapping> Mappings { get; set; }
    }

    public class Mapping
    {
        public string RepositoryType { get; set; }
        public string ImplementationType { get; set; }
    }
}
