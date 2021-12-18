using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class DecisionDocument
    {
        [Key]
        public int id { get; set; }
        public string parh { get; set; }

        [ForeignKey("Decision")]
        public int? decision_id { get; set; }
        public Decision Decision { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}