using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class a3da2LeganViewModel
    {
        public int id { get; set; }
        public string cheque_number { get; set; }

        public int? employee_id { get; set; }

        public string status { get; set; }
        public DateTime date { get; set; }
        public DateTime date_submitted { get; set; }
        public string string_date { get; set; }
        public string string_date_submitted { get; set; }
        public string bank_name { get; set; }
        public string employee_name { get; set; }

        public int? legan_mosa3dat_id { get; set; }
        public int? bank_id { get; set; }

        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}