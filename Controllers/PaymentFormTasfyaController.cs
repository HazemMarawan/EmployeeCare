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
    public class PaymentFormTasfyaController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
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
                                       select new PaymentFormTasfyaViewModel
                                       {
                                           id = paymentform.id,
                                           employee_name = employee.name,
                                           document_name = document.name,
                                           created_at = paymentform.created_at,
                                           //decision_name = decision.title,
                                           employee_document_id = paymentform.employee_document_id,
                                           employee_id = employeeDocument.employee_id,
                                           //decision_id = paymentform.decision_id,
                                           //salary = paymentform.salary,
                                           //no_of_months = paymentform.no_of_months,
                                           //last_paid_installment = paymentform.last_paid_installment,
                                           //deduct_amount_from_takaful = paymentform.deduct_amount_from_takaful,
                                           //installment_need_deduct = paymentform.installment_need_deduct,
                                           //debt_need_deduct = paymentform.debt_need_deduct,
                                           //membership_subscription_deduct = paymentform.membership_subscription_deduct,
                                           //final_paid = paymentform.final_paid,
                                           notes = paymentform.notes,
                                           type = paymentform.type,
                                           approval_status = paymentform.approval_status,
                                           details = paymentform.details,
                                           subscription_date = employeeDocument.subscription_date,
                                           reason_est7kak = paymentform.reason_est7kak,
                                           date_est7kak = paymentform.date_est7kak,
                                           collected_installments = paymentform.collected_installments,
                                           record_no = paymentform.record_no,
                                           record_date = paymentform.record_date,
                                           last_installment = paymentform.last_installment,

                                       }).AsEnumerable().Where(s => s.approval_status == 1 && s.type == (int)PaymentFormTypes.Tasfya).Select(s => new PaymentFormTasfyaViewModel
                                       {
                                           id = s.id,
                                           employee_name = s.employee_name,
                                           document_name = s.document_name,
                                           created_at = s.created_at,
                                           //decision_name = s.decision_name,
                                           employee_document_id = s.employee_document_id,
                                           employee_id = s.employee_id,
                                           //decision_id = s.decision_id,
                                           //salary = s.salary,
                                           //no_of_months = s.no_of_months,
                                           //last_paid_installment = s.last_paid_installment,
                                           //deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                           //installment_need_deduct = s.installment_need_deduct,
                                           //debt_need_deduct = s.debt_need_deduct,
                                           //membership_subscription_deduct = s.membership_subscription_deduct,
                                           //final_paid = s.final_paid,
                                           notes = s.notes,
                                           type = s.type,
                                           approval_status = s.approval_status,
                                           string_created_at = s.created_at != null ? ((DateTime)s.created_at).ToString("yyyy-MM-dd") : "",
                                           subscription_date = s.subscription_date,
                                           string_subscription_date = s.subscription_date != null ? ((DateTime)s.subscription_date).ToString("yyyy-MM-dd") : "",
                                           reason_est7kak = s.reason_est7kak,
                                           date_est7kak = s.date_est7kak,
                                           string_date_est7kak = s.subscription_date != null ? ((DateTime)s.date_est7kak).ToString("yyyy-MM-dd") : "",
                                           collected_installments = s.collected_installments,
                                           record_no = s.record_no,
                                           record_date = s.record_date,
                                           string_record_date = s.record_date != null ? ((DateTime)s.record_date).ToString("yyyy-MM-dd") : "",
                                           last_installment = s.last_installment,
                                           details = s.details

                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    paymentFormData = paymentFormData.Where(m => m.notes.ToLower().Contains(searchValue.ToLower())
                        || m.id.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.employee_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.document_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.last_installment.ToString().ToLower().Contains(searchValue.ToLower())
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
            //ViewBag.decisions = db.Decisions.Select(d => new { d.id, d.title }).ToList();

            return View();
        }
        [HttpPost]
        public JsonResult savePaymentForm(PaymentFormViewModel paymentFormVM)
        {
            if (paymentFormVM.id == 0)
            {
                PaymentForm paymentForm = AutoMapper.Mapper.Map<PaymentFormViewModel, PaymentForm>(paymentFormVM);

                paymentForm.updated_at = DateTime.Now;
                paymentForm.created_at = DateTime.Now;

                db.PaymentForms.Add(paymentForm);
            }
            else
            {
                PaymentForm oldPaymentForm = db.PaymentForms.Find(paymentFormVM.id);

                oldPaymentForm.employee_document_id = paymentFormVM.employee_document_id;
                oldPaymentForm.last_installment = paymentFormVM.last_installment;
                oldPaymentForm.reason_est7kak = paymentFormVM.reason_est7kak;
                oldPaymentForm.subscription_date = paymentFormVM.subscription_date;
                oldPaymentForm.date_est7kak = paymentFormVM.date_est7kak;
                oldPaymentForm.collected_installments = paymentFormVM.collected_installments;
                oldPaymentForm.record_no = paymentFormVM.record_no;
                oldPaymentForm.record_date = paymentFormVM.record_date;
                oldPaymentForm.details = paymentFormVM.details;
                oldPaymentForm.type = (int)PaymentFormTypes.Tasfya;
                oldPaymentForm.approval_status = paymentFormVM.approval_status;
                oldPaymentForm.notes = paymentFormVM.notes;               
                oldPaymentForm.updated_at = DateTime.Now;

                db.Entry(oldPaymentForm).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deletePaymentForm(int id)
        {
            PaymentForm deletePaymentForm = db.PaymentForms.Find(id);
            db.PaymentForms.Remove(deletePaymentForm);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult View(int id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "PaymentFormTasfyaPart1.rpt"));
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
                                                                //decision_name = decision.title,
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
                                                                //birth_date = (DateTime)employee.birth_date,
                                                                string_birth_date = ((DateTime)employee.birth_date).ToString(),
                                                                string_subscription_date = paymentform.subscription_date != null ? (paymentform.subscription_date).ToString() : "",
                                                                reason_est7kak = paymentform.reason_est7kak,
                                                                //date_est7kak = (DateTime)paymentform.date_est7kak,
                                                                string_date_est7kak = paymentform.subscription_date != null ? (paymentform.date_est7kak).ToString() : "",
                                                                collected_installments = (double)(paymentform.collected_installments != null ? paymentform.collected_installments : 0),
                                                                //record_no = (int)(paymentform.record_no != null ? paymentform.record_no : 0),
                                                                //record_date = (DateTime)paymentform.record_date,
                                                                string_record_date = paymentform.record_date != null ? (paymentform.record_date).ToString() : "",
                                                                last_installment = paymentform.last_installment,
                                                                details = paymentform.details

                                                            }).Where(s => s.id == id).ToList();
                                                            //.AsEnumerable().Select(s => new PaymentFormReport
                                                            //{
                                                            //    id = s.id,
                                                            //    employee_name = s.employee_name,
                                                            //    document_name = s.document_name,
                                                            //    grade_name = s.grade_name,
                                                            //    destinaion_name = s.destinaion_name,
                                                            //    //created_at = s.created_at,
                                                            //    //decision_name = decision.title,
                                                            //    employee_document_id = s.employee_document_id,
                                                            //    employee_id = s.employee_id,
                                                            //    decision_id = s.decision_id,
                                                            //    salary = s.salary,
                                                            //    no_of_months = s.no_of_months,
                                                            //    last_paid_installment = s.last_paid_installment,
                                                            //    deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                                            //    installment_need_deduct = s.installment_need_deduct,
                                                            //    debt_need_deduct = s.debt_need_deduct,
                                                            //    membership_subscription_deduct = s.membership_subscription_deduct,
                                                            //    final_paid = s.final_paid,
                                                            //    notes = s.notes,
                                                            //    approval_status = s.approval_status,
                                                            //    managerial_fees = s.managerial_fees,
                                                            //    installments = s.installments,
                                                            //    cheque_cost = s.cheque_cost,
                                                            //    other_income = s.other_income,
                                                            //    total_deduction = s.total_deduction,
                                                            //    cheque_number = s.cheque_number,
                                                            //    modir_3am_elgam3ia = s.modir_3am_elgam3ia,
                                                            //    modir_elhesabat = s.modir_elhesabat,
                                                            //    amin_elsandok = s.amin_elsandok,
                                                            //    modir_edart_elwasika = s.modir_edart_elwasika,
                                                            //    national_id = s.national_id,
                                                            //    string_birth_date = s.birth_date != null ? s.birth_date.ToString() : "",
                                                            //    string_subscription_date = s.subscription_date != null ? s.subscription_date.ToString() : "",
                                                            //    reason_est7kak = s.reason_est7kak,
                                                            //    //date_est7kak = s.date_est7kak,
                                                            //    string_date_est7kak = s.date_est7kak != null ? s.date_est7kak.ToString() : "",
                                                            //    collected_installments = s.collected_installments,
                                                            //    record_no = s.record_no,
                                                            //    //record_date = s.record_date,
                                                            //    string_record_date = s.record_date != null ? s.record_date.ToString() : "",
                                                            //    last_installment = s.last_installment,
                                                            //    details = s.details
                                                            //});

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