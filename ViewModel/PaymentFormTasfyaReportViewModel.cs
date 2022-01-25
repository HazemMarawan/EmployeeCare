using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class PaymentFormTasfyaReportViewModel
    {
        public int id { get; set; }
        public DateTime? subscription_date { get; set; }
        public double? salary { get; set; }
        public double? discount_percentage { get; set; }
        public double? reserved_months { get; set; }
        public double? total { get; set; }
        public int? payment_form_id { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string string_created_at { get; set; }
        public string string_subscription_date { get; set; }
    }
}