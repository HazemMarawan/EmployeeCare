using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.ViewModel
{
    public class PaymentFormViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public int? employee_document_id { get; set; }
        public int? employee_file_number { get; set; }
        public int? employee_id { get; set; }
        public string employee_name { get; set; }
        public string document_name { get; set; }
        public string decision_name { get; set; }
        public int? decision_id { get; set; }
        public double? salary { get; set; }
        public double? no_of_months { get; set; }
        public string last_paid_installment { get; set; }
        public string national_id { get; set; }
        public double? deduct_amount_from_takaful { get; set; }
        public double? installment_need_deduct { get; set; }
        public double? debt_need_deduct { get; set; }
        public double? membership_subscription_deduct { get; set; }
        public double? final_paid { get; set; }
        public string notes { get; set; }
        public string employee_national_id { get; set; }
        public int? approval_status { get; set; }
        public double? managerial_fees { get; set; }
        public double? installments { get; set; }
        public double? subscription { get; set; }
        public double? cheque_cost { get; set; }
        public double? other_income { get; set; }
        public double? total_deduction { get; set; }
        public string cheque_number { get; set; }
        public int? active { get; set; }
        public int? type { get; set; }
        public DateTime? record_date { get; set; }
        public int? record_number { get; set; }
        public double? collected_installments { get; set; }
        public DateTime? due_date { get; set; }
        public string last_installment { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string string_created_at { get; set; }
        public string details { get; set; }
        public DateTime? subscription_date { get; set; }
        public string string_subscription_date { get; set; }
        public string reason_est7kak { get; set; }
        public DateTime? date_est7kak { get; set; }
        public string string_date_est7kak { get; set; }
        public int? record_no { get; set; }


    }
}