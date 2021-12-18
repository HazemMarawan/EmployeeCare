using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class Decision
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }

        [ForeignKey("Destination")]
        public int? destination_id { get; set; }
        public Destination Destination { get; set; }
        public string title { get; set; }
        public DateTime? decision_date { get; set; }
        public int? destination_type { get; set; }
        public string notes { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public virtual ICollection<DecisionDocument> DecisionDocuments { get; set; }
    }
}