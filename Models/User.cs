using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeCare.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int? gender { get; set; }
        public DateTime? birthDate { get; set; }
        public string image { get; set; }
        public int? type { get; set; }
        public int? users_access { get; set; }
        public int? primary_data_access { get; set; }
        public int? payment_forms_management_access { get; set; }
        public int? manage_inventory_access { get; set; }
        public int? accountants_access { get; set; }
        public int? paid_off_access { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}