using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class EmployeeDocumentViewModel
    {
        public int id { get; set; }
        public int? document_number { get; set; }
        public int? employee_id { get; set; }
        public string employee_name { get; set; }
        public int? document_id { get; set; }
        public string document_name { get; set; }
        public DateTime? subscription_date { get; set; }
        public string string_subscription_date { get; set; }
        public float? percentage { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}