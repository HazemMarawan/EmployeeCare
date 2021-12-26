using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class Invoice
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("PaymentType")]
        public int? payment_type_id { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? payment_date { get; set; }
        public double? amount { get; set; }

        [ForeignKey("Bank")]
        public int? bank_id { get; set; }
        public Bank Bank { get; set; }
        public string invoice_number { get; set; }

        [ForeignKey("Employee")]
        public int? employee_id { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("Document")]
        public int? document_id { get; set; }
        public Document Document { get; set; }
        public DateTime? vacation_from { get; set; }
        public DateTime? vacation_to { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}