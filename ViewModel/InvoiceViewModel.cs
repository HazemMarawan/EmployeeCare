using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class InvoiceViewModel
    {
        public int id { get; set; }
        public int? payment_type_id { get; set; }
        public string string_payment_type { get; set; }
        public DateTime? payment_date { get; set; }
        public string string_payment_date { get; set; }
        public double? amount { get; set; }
        public int? bank_id { get; set; }
        public string string_bank_name { get; set; }
        public string invoice_number { get; set; }
        public int? employee_id { get; set; }
        public string string_employee_name { get; set; }
        public int? document_id { get; set; }
        public string string_document_name { get; set; }
        public DateTime? vacation_from { get; set; }
        public DateTime? vacation_to { get; set; }
        public string string_vacation_from { get; set; }
        public string string_vacation_to { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public string string_created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}