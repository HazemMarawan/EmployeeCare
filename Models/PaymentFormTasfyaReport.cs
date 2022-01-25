using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class PaymentFormTasfyaReport
    {
        [Key]
        public int id { get; set; }
        public DateTime? subscription_date { get; set; }
        public double? salary { get; set; }
        public double? discount_percentage { get; set; }
        public double? reserved_months { get; set; }
        public double? total { get; set; }
        [ForeignKey("PaymentForm")]
        public int? payment_form_id { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}