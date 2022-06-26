using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeCare.Auth;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;

namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class AccountingTreeController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        // GET: AccountingTree
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult GetTree(string id)
        {

            var items = db.AccountingTrees.Where(a => a.active == 1).Select(a => new JsTreeViewModel
            {
                id = a.id,
                text = a.name,
                parent = a.parent_id != null ? a.parent_id.ToString() : "#",
                parent_id = a.parent_id,
                children = db.AccountingTrees.Where(s=>s.parent_id == a.id).Any(),
            });

            if (id != "#")
            {
                int currentId = Convert.ToInt32(id);
                items = items.Where(a => a.parent_id == currentId);
            }
            else
                items = items.Where(a => a.parent_id == null);


            List<JsTreeViewModel> treeData = items.ToList();

            return Json(treeData , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveAccountingTree(AccountingTreeViewModel accountingTreeViewModel)
        {
            if (accountingTreeViewModel.id == 0)
            {
                AccountingTree accountingTree = AutoMapper.Mapper.Map<AccountingTreeViewModel, AccountingTree>(accountingTreeViewModel);

                accountingTree.created_by = Convert.ToInt32(Session["id"].ToString());
                accountingTree.updated_at = DateTime.Now;
                accountingTree.created_at = DateTime.Now;
                accountingTree.active = 1;

                db.AccountingTrees.Add(accountingTree);
            }
            else
            {
                AccountingTree oldaccountingTree = db.AccountingTrees.Find(accountingTreeViewModel.id);

                oldaccountingTree.name = accountingTreeViewModel.name;
                oldaccountingTree.updated_at = DateTime.Now;

                db.Entry(oldaccountingTree).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }


    }
}