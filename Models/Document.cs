using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace EmployeeCare.Models
{
    public class Document
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        [ForeignKey("Destination")]
        public int? destination_id { get; set; }
        public Destination Destination { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}