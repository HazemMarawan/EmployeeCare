using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class JsTreeViewModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public int? parent_id { get; set; }
        public bool children { get; set; }
        public string icon { get; set; }
    }
}