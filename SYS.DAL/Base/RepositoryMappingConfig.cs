using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.Base
{
    public class RepositoryMappingConfig
    {
        public List<RepositoryMapping> Mappings { get; set; }
    }

    public class RepositoryMapping
    {
        public string RepositoryType { get; set; }
        public string ImplementationType { get; set; }
    }
}
