using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class Setting
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string nice_name { get; set; }
        public string value { get; set; }
    }
}