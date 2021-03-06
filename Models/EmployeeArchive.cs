using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class EmployeeArchive
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Employee")]
        public int? employee_id { get; set; }
        public Employee Employee { get; set; }
        public string path { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}