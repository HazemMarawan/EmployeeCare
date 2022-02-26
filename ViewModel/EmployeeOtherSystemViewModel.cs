using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class EmployeeOtherSystemViewModel
    {
        public int id { get; set; }
        public int? employee_id { get; set; }
        public int? other_system_id { get; set; }
        public string other_system_name { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public List<int> other_systems { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}