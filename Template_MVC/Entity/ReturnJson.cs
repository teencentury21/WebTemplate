using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Template_MVC.Entity
{
    public class ReturnJson
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public object item { get; set; }
    }
}