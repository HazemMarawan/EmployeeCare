using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class AccountingTreeViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string level1 { get; set; }
        public string level2 { get; set; }
        public string range_from { get; set; }
        public string range_to { get; set; }
        public string level { get; set; }
        public int? type { get; set; }
        public int? mden_da2en { get; set; }

        public int? parent_id { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}