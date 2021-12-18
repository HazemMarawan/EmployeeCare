using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class PaymentForm
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }

        [ForeignKey("EmployeeDocument")]
        public int? employee_document_id { get; set; }
        public EmployeeDocument EmployeeDocument { get; set; }

        [ForeignKey("Decision")]
        public int? decision_id { get; set; }
        public Decision Decision { get; set; }
        public double? salary { get; set; }
        public double? no_of_months { get; set; }
        public double? last_paid_installment { get; set; }
        public double? deduct_amount_from_takaful { get; set; }
        public double? installment_need_deduct { get; set; }
        public double? debt_need_deduct { get; set; }
        public double? membership_subscription_deduct { get; set; }
        public double? final_paid { get; set; }
        public string notes { get; set; }
        public int? approval_status { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}