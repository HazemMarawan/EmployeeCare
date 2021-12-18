using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class DocumentViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? destination_id { get; set; }
        public string destination_name { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string string_created_at { get; set; }
    }
}