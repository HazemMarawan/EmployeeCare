using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class Employee
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }

        [ForeignKey("Destination")]
        public int? destination_id { get; set; }
        public Destination Destination { get; set; }
        public int? employee_file_number { get; set; }
        public string name { get; set; }

        [ForeignKey("Grade")]
        public int? grade_id { get; set; }
        public Grade Grade { get; set; }
        public string national_id { get; set; }
        public string email { get; set; }
        public int? status { get; set; }
        public int? membership_status { get; set; }
        public string bank_account_number { get; set; }

        [ForeignKey("Bank")]
        public int? bank_id { get; set; }
        public Bank Bank { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual ICollection<PaymentForm> PaymentForms { get; set; }
        public virtual ICollection<EmployeeArchive> EmployeeArchives { get; set; }

    }
}