﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class a3da2Legan
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Employee")]
        public int? employee_id { get; set; }
        public Employee Employee { get; set; }

        public string status { get; set; }
        public string cheque_number { get; set; }

        public DateTime date { get; set; }
        public DateTime date_submitted { get; set; }

        [ForeignKey("leganMosa3dat")]
        public int? legan_mosa3dat_id { get; set; }
        public leganMosa3dat leganMosa3dat { get; set; }

        [ForeignKey("Bank")]
        public int? bank_id { get; set; }
        public Bank Bank { get; set; }

        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}