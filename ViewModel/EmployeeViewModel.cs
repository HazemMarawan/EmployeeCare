using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class EmployeeViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public int? destination_id { get; set; }
        public string destination_name { get; set; }
        public int? employee_file_number { get; set; }
        public string name { get; set; }
        public int? grade_id { get; set; }
        public string grade_name { get; set; }
        public string national_id { get; set; }
        public string email { get; set; }
        public int? status { get; set; }
        public int? membership_status { get; set; }
        public string bank_account_number { get; set; }
        public int? bank_id { get; set; }
        public string bank_name { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string string_created_at { get; set; }
    }
}