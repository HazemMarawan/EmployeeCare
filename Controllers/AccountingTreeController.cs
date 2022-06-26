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
                oldaccountingTree.code = accountingTreeViewModel.code;
                oldaccountingTree.level1 = accountingTreeViewModel.level1;
                oldaccountingTree.level2 = accountingTreeViewModel.level2;
                oldaccountingTree.range_from = accountingTreeViewModel.range_from;
                oldaccountingTree.range_to = accountingTreeViewModel.range_to;
                oldaccountingTree.level = accountingTreeViewModel.level;
                oldaccountingTree.mden_da2en = accountingTreeViewModel.mden_da2en;
                oldaccountingTree.updated_at = DateTime.Now;

                db.Entry(oldaccountingTree).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult deleteAccountingTree(int id)
        {
            db.AccountingTrees.Where(a => a.parent_id == id).ToList().ForEach(a => db.AccountingTrees.Remove(a));
            db.SaveChanges();

            AccountingTree accountingTree =  db.AccountingTrees.Find(id);
            db.AccountingTrees.Remove(accountingTree);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getAcctountTree(int id)
        {

            AccountingTreeViewModel accountingTree = db.AccountingTrees.Where(s => s.active == 1 && s.id == id).Select(s => new AccountingTreeViewModel
            {
                id = s.id,
                name = s.name,
                code = s.code,
                level1 = s.level1,
                level2 = s.level2,
                range_from = s.range_from,
                range_to = s.range_to,
                level = s.level,
                type = s.type,
                mden_da2en = s.mden_da2en,
                parent_id = s.parent_id

            }).FirstOrDefault();

            return Json(new { accountingTree = accountingTree, message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}