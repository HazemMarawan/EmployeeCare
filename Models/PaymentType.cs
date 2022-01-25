﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class PaymentType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int? from_to { get; set; }
        public string code { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}