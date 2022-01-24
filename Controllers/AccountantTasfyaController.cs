using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using EmployeeCare.Auth;
using EmployeeCare.Enum;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;

namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class AccountantTasfyaController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        // GET: Accountant
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                var from_date = Request.Form.GetValues("columns[0][search][value]")[0];
                var to_date = Request.Form.GetValues("columns[1][search][value]")[0];
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                // Getting all data    
                var paymentFormData = (from paymentform in db.PaymentForms
                                       join employeeDocument in db.EmployeeDocuments on paymentform.employee_document_id equals employeeDocument.id
                                       join employee in db.Employees on employeeDocument.employee_id equals employee.id
                                       join document in db.Documents on employeeDocument.document_id equals document.id
                                       //join decision in db.Decisions on paymentform.decision_id equals decision.id
                                       select new PaymentFormViewModel
                                       {
                                           id = paymentform.id,
                                           employee_name = employee.name,
                                           document_name = document.name,
                                           created_at = paymentform.created_at,
                                           //decision_name = decision.title,
                                           employee_document_id = paymentform.employee_document_id,
                                           employee_id = employeeDocument.employee_id,
                                           decision_id = paymentform.decision_id,
                                           salary = paymentform.salary,
                                           no_of_months = paymentform.no_of_months,
                                           last_paid_installment = paymentform.last_paid_installment,
                                           deduct_amount_from_takaful = paymentform.deduct_amount_from_takaful,
                                           installment_need_deduct = paymentform.installment_need_deduct,
                                           debt_need_deduct = paymentform.debt_need_deduct,
                                           membership_subscription_deduct = paymentform.membership_subscription_deduct,
                                           final_paid = paymentform.final_paid,
                                           notes = paymentform.notes,
                                           collected_installments = paymentform.collected_installments,
                                           approval_status = paymentform.approval_status,
                                           managerial_fees = paymentform.managerial_fees == null ? db.Deductions.Select(m => m.managerial_fees).FirstOrDefault().Value : paymentform.managerial_fees,
                                           installments = paymentform.installments == null ? db.Deductions.Select(m => m.installments).FirstOrDefault().Value : paymentform.installments,
                                           subscription = paymentform.subscription == null ? db.Deductions.Select(m => m.subscription).FirstOrDefault().Value : paymentform.subscription,
                                           other_income = paymentform.other_income == null ? db.Deductions.Select(m => m.other_income).FirstOrDefault().Value : paymentform.other_income,
                                           cheque_cost = paymentform.cheque_cost == null ? (int)db.Deductions.Select(m => m.cheque_cost).FirstOrDefault().Value : (int)paymentform.cheque_cost,
                                           total_deduction = (double)db.Deductions.Select(m => m.total_deduction).FirstOrDefault().Value,
                                           cheque_number = paymentform.cheque_number,
                                           type = paymentform.type
                                       }).AsEnumerable().Select(s => new PaymentFormViewModel
                                       {
                                           id = s.id,
                                           employee_name = s.employee_name,
                                           document_name = s.document_name,
                                           created_at = s.created_at,
                                           decision_name = s.decision_name,
                                           employee_document_id = s.employee_document_id,
                                           employee_id = s.employee_id,
                                           decision_id = s.decision_id,
                                           salary = s.salary,
                                           collected_installments= s.collected_installments,
                                           no_of_months = s.no_of_months,
                                           last_paid_installment = s.last_paid_installment,
                                           deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                           installment_need_deduct = s.installment_need_deduct,
                                           debt_need_deduct = s.debt_need_deduct,
                                           membership_subscription_deduct = s.membership_subscription_deduct,
                                           final_paid = s.final_paid,
                                           notes = s.notes,
                                           approval_status = s.approval_status,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd"),
                                           managerial_fees = s.managerial_fees,
                                           installments = s.installments,
                                           other_income = s.other_income,
                                           cheque_cost = s.cheque_cost,
                                           total_deduction = s.total_deduction,
                                           cheque_number = s.cheque_number,
                                           subscription = s.subscription,
                                           type = s.type
                                       }).Where(s => s.approval_status == 2 && s.type == (int)PaymentFormTypes.Tasfya);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    paymentFormData = paymentFormData.Where(m => m.notes.ToLower().Contains(searchValue.ToLower())
                        || m.id.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.employee_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.document_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.decision_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.salary.ToString().ToLower().Contains(searchValue.ToLower())
                       );
                }

                //total number of rows count     
                var displayResult = paymentFormData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = paymentFormData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            ViewBag.employees = db.Employees.Select(d => new { d.id, d.name }).ToList();
            ViewBag.decisions = db.Decisions.Select(d => new { d.id, d.title }).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult saveAccountant(PaymentFormViewModel paymentFormVM)
        {

            PaymentForm oldPaymentForm = db.PaymentForms.Find(paymentFormVM.id);

            oldPaymentForm.approval_status = paymentFormVM.approval_status;
            oldPaymentForm.managerial_fees = paymentFormVM.managerial_fees;
            oldPaymentForm.installments = paymentFormVM.installments;
            oldPaymentForm.subscription = paymentFormVM.subscription;
            oldPaymentForm.cheque_cost = paymentFormVM.cheque_cost;
            oldPaymentForm.other_income = paymentFormVM.other_income;
            oldPaymentForm.total_deduction = paymentFormVM.managerial_fees + paymentFormVM.installments + paymentFormVM.cheque_cost + paymentFormVM.other_income;
            oldPaymentForm.cheque_number = paymentFormVM.cheque_number;
            oldPaymentForm.updated_at = DateTime.Now;

            db.Entry(oldPaymentForm).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult View(int id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "PaymentFormFinal.rpt"));
            List<Setting> settings = db.Settings.ToList();
            List<PaymentFormReport> paymentFormViewModel = (from paymentform in db.PaymentForms
                                                            join employeeDocument in db.EmployeeDocuments on paymentform.employee_document_id equals employeeDocument.id
                                                            join employee in db.Employees on employeeDocument.employee_id equals employee.id
                                                            join destination in db.Destinations on employee.destination_id equals destination.id
                                                            join grade in db.Grades on employee.grade_id equals grade.id
                                                            join document in db.Documents on employeeDocument.document_id equals document.id
                                                            join decision in db.Decisions on paymentform.decision_id equals decision.id
                                                            select new PaymentFormReport
                                                            {
                                                                id = paymentform.id,
                                                                employee_name = employee.name,
                                                                document_name = document.name,
                                                                collected_installments = (double)paymentform.collected_installments,
                                                                grade_name = grade.name,
                                                                destinaion_name = destination.name,
                                                                created_at = (DateTime)paymentform.created_at,
                                                                decision_name = decision.title,
                                                                employee_document_id = (int)paymentform.employee_document_id,
                                                                employee_id = (int)employeeDocument.employee_id,
                                                                decision_id = (int)paymentform.decision_id,
                                                                salary = (double)paymentform.salary,
                                                                no_of_months = (double)paymentform.no_of_months,
                                                                last_paid_installment = paymentform.last_paid_installment,
                                                                deduct_amount_from_takaful = (double)paymentform.deduct_amount_from_takaful,
                                                                installment_need_deduct = (double)paymentform.installment_need_deduct,
                                                                debt_need_deduct = (double)paymentform.debt_need_deduct,
                                                                membership_subscription_deduct = (double)paymentform.membership_subscription_deduct,
                                                                final_paid = (double)paymentform.final_paid,
                                                                notes = paymentform.notes,
                                                                approval_status = (int)paymentform.approval_status,
                                                                managerial_fees = (double)paymentform.managerial_fees,
                                                                gneh_managerial_fees = (int)paymentform.managerial_fees,
                                                                ersh_managerial_fees = (double)((paymentform.managerial_fees - (int)paymentform.managerial_fees) * 100),
                                                                installments = (int)paymentform.installments,
                                                                gneh_installments = (int)paymentform.installments,
                                                                ersh_installments = (double)((paymentform.installments - (int)paymentform.installments) * 100),
                                                                cheque_cost = (double)paymentform.cheque_cost,
                                                                gneh_cheque_cost = (int)paymentform.cheque_cost,
                                                                ersh_cheque_cost = (double)((paymentform.cheque_cost - (int)paymentform.cheque_cost) * 100),
                                                                other_income = (double)paymentform.other_income,
                                                                gneh_other_income = (int)paymentform.other_income,
                                                                ersh_other_income = (double)((paymentform.other_income - (int)paymentform.other_income) * 100),
                                                                total_deduction = (double)paymentform.total_deduction,
                                                                gneh_total_deduction = (int)paymentform.total_deduction,
                                                                ersh_total_deduction = (double)((paymentform.total_deduction - (int)paymentform.total_deduction) * 100),
                                                                subscription = (double)paymentform.subscription,
                                                                gneh_subscription = (int)paymentform.subscription,
                                                                ersh_subscription = (double)((paymentform.subscription - (int)paymentform.subscription) * 100),
                                                                cheque_number = paymentform.cheque_number,
                                                                modir_3am_elgam3ia = db.Settings.Where(set => set.name == "modir_3am_elgam3ia").FirstOrDefault().value,
                                                                modir_elhesabat = db.Settings.Where(set => set.name == "modir_elhesabat").FirstOrDefault().value,
                                                                amin_elsandok = db.Settings.Where(set => set.name == "amin_elsandok").FirstOrDefault().value,
                                                                modir_edart_elwasika = db.Settings.Where(set => set.name == "modir_edart_elwasika").FirstOrDefault().value,
                                                                national_id = employee.national_id,
                                                                string_birth_date = employee.birth_date != null ? ((DateTime)employee.birth_date).ToString() : "",
                                                                final_paid_after_deduction = (double)(paymentform.final_paid - paymentform.total_deduction)

                                                            }).Where(s => s.id == id).AsEnumerable().Select(s => new PaymentFormReport
                                                            {
                                                                id = s.id,
                                                                employee_name = s.employee_name,
                                                                document_name = s.document_name,
                                                                grade_name = s.grade_name,
                                                                destinaion_name = s.destinaion_name,
                                                                created_at = s.created_at,
                                                                decision_name = s.decision_name,
                                                                employee_document_id = s.employee_document_id,
                                                                employee_id = s.employee_id,
                                                                decision_id = s.decision_id,
                                                                collected_installments = s.collected_installments,
                                                                salary = s.salary,
                                                                no_of_months = s.no_of_months,
                                                                last_paid_installment = s.last_paid_installment,
                                                                deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                                                installment_need_deduct = s.installment_need_deduct,
                                                                debt_need_deduct = s.debt_need_deduct,
                                                                membership_subscription_deduct = s.membership_subscription_deduct,
                                                                final_paid = s.final_paid,
                                                                notes = s.notes,
                                                                approval_status = s.approval_status,
                                                                managerial_fees = s.managerial_fees,
                                                                gneh_managerial_fees = s.gneh_managerial_fees,
                                                                ersh_managerial_fees = s.ersh_managerial_fees,
                                                                installments = s.installments,
                                                                gneh_installments = s.gneh_installments,
                                                                ersh_installments = s.ersh_installments,
                                                                cheque_cost = s.cheque_cost,
                                                                gneh_cheque_cost = s.gneh_cheque_cost,
                                                                ersh_cheque_cost = s.ersh_cheque_cost,
                                                                other_income = s.other_income,
                                                                gneh_other_income = s.gneh_other_income,
                                                                ersh_other_income = s.ersh_other_income,
                                                                total_deduction = s.total_deduction,
                                                                gneh_total_deduction = s.gneh_total_deduction,
                                                                ersh_total_deduction = s.ersh_total_deduction,
                                                                subscription = s.subscription,
                                                                gneh_subscription = s.gneh_subscription,
                                                                ersh_subscription = s.ersh_subscription,
                                                                cheque_number = s.cheque_number,
                                                                modir_3am_elgam3ia = s.modir_3am_elgam3ia,
                                                                modir_elhesabat = s.modir_elhesabat,
                                                                amin_elsandok = s.amin_elsandok,
                                                                modir_edart_elwasika = s.modir_edart_elwasika,
                                                                national_id = s.national_id,
                                                                string_birth_date = !String.IsNullOrEmpty(s.string_birth_date) ? s.string_birth_date.Split(' ')[0] : "-",
                                                                final_paid_after_deduction = s.final_paid_after_deduction,
                                                                final_paid_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.final_paid.ToString())),
                                                                final_paid_after_deduction_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.final_paid_after_deduction.ToString())),
                                                                collected_installments_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.collected_installments.ToString()))
                                                            }).ToList();

            rd.SetDataSource(paymentFormViewModel);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
            rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "PaymentForm" + paymentFormViewModel[0].employee_name + DateTime.Now.ToString() + ".pdf");
        }

        public ActionResult ViewTasfya(int id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "PaymentFormTasfyaFinal.rpt"));
            List<Setting> settings = db.Settings.ToList();


            List<PaymentFormReport> paymentFormViewModel = (from paymentform in db.PaymentForms
                                                            join employeeDocument in db.EmployeeDocuments on paymentform.employee_document_id equals employeeDocument.id
                                                            join employee in db.Employees on employeeDocument.employee_id equals employee.id
                                                            join destination in db.Destinations on employee.destination_id equals destination.id
                                                            join grade in db.Grades on employee.grade_id equals grade.id
                                                            join document in db.Documents on employeeDocument.document_id equals document.id
                                                            //join decision in db.Decisions on paymentform.decision_id equals decision.id
                                                            select new PaymentFormReport
                                                            {
                                                                id = paymentform.id,
                                                                employee_name = employee.name,
                                                                document_name = document.name,
                                                                grade_name = grade.name,
                                                                destinaion_name = destination.name,
                                                                created_at = (DateTime)paymentform.created_at,
                                                                employee_document_id = (int)(paymentform.employee_document_id != null ? paymentform.employee_document_id : 0),
                                                                employee_id = (int)(employeeDocument.employee_id != null ? employeeDocument.employee_id : 0),
                                                                decision_id = (int)(paymentform.decision_id != null ? paymentform.decision_id : 0),
                                                                salary = (double)(paymentform.salary != null ? paymentform.salary : 0),
                                                                no_of_months = (double)(paymentform.no_of_months != null ? paymentform.no_of_months : 0),
                                                                last_paid_installment = paymentform.last_paid_installment,
                                                                deduct_amount_from_takaful = (double)(paymentform.deduct_amount_from_takaful != null ? paymentform.deduct_amount_from_takaful : 0),
                                                                installment_need_deduct = (double)(paymentform.installment_need_deduct != null ? paymentform.installment_need_deduct : 0),
                                                                debt_need_deduct = (double)(paymentform.debt_need_deduct != null ? paymentform.debt_need_deduct : 0),
                                                                membership_subscription_deduct = (double)(paymentform.membership_subscription_deduct != null ? paymentform.membership_subscription_deduct : 0),
                                                                final_paid = (double)(paymentform.final_paid != null ? paymentform.final_paid : 0),
                                                                notes = paymentform.notes,
                                                                approval_status = (int)(paymentform.approval_status != null ? paymentform.approval_status : 0),
                                                                managerial_fees = (double)(paymentform.managerial_fees != null ? paymentform.managerial_fees : 0),
                                                                installments = (int)(paymentform.installments != null ? paymentform.installments : 0),
                                                                cheque_cost = (double)(paymentform.cheque_cost != null ? paymentform.cheque_cost : 0),
                                                                other_income = (double)(paymentform.other_income != null ? paymentform.other_income : 0),
                                                                total_deduction = (double)(paymentform.total_deduction != null ? paymentform.total_deduction : 0),
                                                                cheque_number = paymentform.cheque_number,
                                                                modir_3am_elgam3ia = db.Settings.Where(set => set.name == "modir_3am_elgam3ia").FirstOrDefault().value,
                                                                modir_elhesabat = db.Settings.Where(set => set.name == "modir_elhesabat").FirstOrDefault().value,
                                                                amin_elsandok = db.Settings.Where(set => set.name == "amin_elsandok").FirstOrDefault().value,
                                                                modir_edart_elwasika = db.Settings.Where(set => set.name == "modir_edart_elwasika").FirstOrDefault().value,
                                                                national_id = employee.national_id,
                                                                string_birth_date = employee.birth_date != null ? ((DateTime)employee.birth_date).ToString() : "",
                                                                string_subscription_date = paymentform.subscription_date != null ? ((DateTime)paymentform.subscription_date).ToString() : "",
                                                                reason_est7kak = paymentform.reason_est7kak,
                                                                string_date_est7kak = paymentform.date_est7kak != null ? ((DateTime)paymentform.date_est7kak).ToString() : "",
                                                                collected_installments = (double)(paymentform.collected_installments != null ? paymentform.collected_installments : 0),
                                                                string_record_date = paymentform.record_date != null ? ((DateTime)paymentform.record_date).ToString() : "",
                                                                last_installment = paymentform.last_installment,
                                                                details = paymentform.details,
                                                                gneh_managerial_fees = (int)paymentform.managerial_fees,
                                                                ersh_managerial_fees = (double)((paymentform.managerial_fees - (int)paymentform.managerial_fees) * 100),
                                                                gneh_installments = (int)paymentform.installments,
                                                                ersh_installments = (double)((paymentform.installments - (int)paymentform.installments) * 100),
                                                                gneh_cheque_cost = (int)paymentform.cheque_cost,
                                                                ersh_cheque_cost = (double)((paymentform.cheque_cost - (int)paymentform.cheque_cost) * 100),
                                                                gneh_other_income = (int)paymentform.other_income,
                                                                ersh_other_income = (double)((paymentform.other_income - (int)paymentform.other_income) * 100),
                                                                gneh_total_deduction = (int)paymentform.total_deduction,
                                                                ersh_total_deduction = (double)((paymentform.total_deduction - (int)paymentform.total_deduction) * 100),
                                                                subscription = (double)paymentform.subscription,
                                                                gneh_subscription = (int)paymentform.subscription,
                                                                ersh_subscription = (double)((paymentform.subscription - (int)paymentform.subscription) * 100),
                                                                final_paid_after_deduction = (double)(paymentform.collected_installments - paymentform.total_deduction)

                                                            }).Where(s => s.id == id).AsEnumerable().Select(s => new PaymentFormReport
                                                            {
                                                                id = s.id,
                                                                employee_name = s.employee_name,
                                                                document_name = s.document_name,
                                                                grade_name = s.grade_name,
                                                                destinaion_name = s.destinaion_name,
                                                                created_at = s.created_at,
                                                                employee_document_id = s.employee_document_id,
                                                                employee_id = s.employee_id,
                                                                decision_id = s.decision_id,
                                                                salary = s.salary,
                                                                no_of_months = s.no_of_months,
                                                                last_paid_installment = s.last_paid_installment,
                                                                deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                                                installment_need_deduct = s.installment_need_deduct,
                                                                debt_need_deduct = s.debt_need_deduct,
                                                                membership_subscription_deduct = s.membership_subscription_deduct,
                                                                final_paid = s.final_paid,
                                                                notes = s.notes,
                                                                approval_status = s.approval_status,
                                                                managerial_fees = s.managerial_fees,
                                                                installments = s.installments,
                                                                cheque_cost = s.cheque_cost,
                                                                other_income = s.other_income,
                                                                total_deduction = s.total_deduction,
                                                                cheque_number = s.cheque_number,
                                                                modir_3am_elgam3ia = s.modir_3am_elgam3ia,
                                                                modir_elhesabat = s.modir_elhesabat,
                                                                amin_elsandok = s.amin_elsandok,
                                                                modir_edart_elwasika = s.modir_edart_elwasika,
                                                                national_id = s.national_id,
                                                                string_birth_date = s.string_birth_date != null ? s.string_birth_date.Split(' ')[0] : "",
                                                                string_subscription_date = s.string_subscription_date != null ? s.string_subscription_date.Split(' ')[0] : "",
                                                                reason_est7kak = s.reason_est7kak,
                                                                string_date_est7kak = s.string_date_est7kak != null ? s.string_date_est7kak.Split(' ')[0] : "",
                                                                collected_installments = s.collected_installments,
                                                                string_record_date = s.string_record_date != null ? s.string_record_date.Split(' ')[0] : "",
                                                                last_installment = s.last_installment,
                                                                details = s.details,
                                                                gneh_managerial_fees = s.gneh_managerial_fees,
                                                                ersh_managerial_fees = s.ersh_managerial_fees,
                                                                gneh_installments = s.gneh_installments,
                                                                ersh_installments = s.ersh_installments,
                                                                gneh_cheque_cost = s.gneh_cheque_cost,
                                                                ersh_cheque_cost = s.ersh_cheque_cost,
                                                                gneh_other_income = s.gneh_other_income,
                                                                ersh_other_income = s.ersh_other_income,
                                                                gneh_total_deduction = s.gneh_total_deduction,
                                                                ersh_total_deduction = s.ersh_total_deduction,
                                                                subscription = s.subscription,
                                                                gneh_subscription = s.gneh_subscription,
                                                                ersh_subscription = s.ersh_subscription,
                                                                final_paid_after_deduction = s.final_paid_after_deduction,
                                                                final_paid_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.final_paid.ToString())),
                                                                final_paid_after_deduction_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.final_paid_after_deduction.ToString())),
                                                                collected_installments_tafkit = Tafkit.ConvertToText(Decimal.Parse(s.collected_installments.ToString()))
                                                            }).ToList();

            rd.SetDataSource(paymentFormViewModel);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
            rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "PaymentFormTasfya" + paymentFormViewModel[0].employee_name + DateTime.Now.ToString() + ".pdf");
        }



    }
}