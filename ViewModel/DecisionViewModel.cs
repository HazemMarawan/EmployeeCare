using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class DecisionViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public int? destination_id { get; set; }
        public string destination_name { get; set; }
        public string title { get; set; }
        public DateTime? decision_date { get; set; }
        public string string_decision_date { get; set; }
        public int? decision_type { get; set; }
        public string notes { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public string string_created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public List<string> decision_documents { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
    }
}