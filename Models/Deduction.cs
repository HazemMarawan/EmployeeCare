using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class Deduction
    {
        [Key]
        public int id { get; set; }
        public double? managerial_fees { get; set; }
        public double? installments { get; set; }
        public double? subscription { get; set; }
        public double? cheque_cost { get; set; }
        public double? other_income { get; set; }
        public double? total_deduction { get; set; }
    }
}