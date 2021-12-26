using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class DecuctionViewModel
    {
        public int id { get; set; }
        public double? managerial_fees { get; set; }
        public int? installments { get; set; }
        public int? cheque_cost { get; set; }
        public double? other_income { get; set; }
        public double? total_deduction { get; set; }
    }
}