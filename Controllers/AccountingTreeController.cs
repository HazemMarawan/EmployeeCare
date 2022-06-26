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

        public JsonResult GetData(int id=-1)
        {

            var items = db.AccountingTrees.Where(a => a.active == 1).Select(a => new AccountingTreeViewModel
            {
                id = a.id,
                name = a.name,
                code = a.code,
                level1 = a.level1,
                level2 = a.level2,
                type = a.type,
                parent_id = a.parent_id,
                range_from = a.range_from,
                range_to = a.range_to

            });

            if (id == -1)
                items = items.Where(a => a.parent_id == null);
            else
                items = items.Where(a => a.parent_id == id);

            // set items in here

            return new JsonResult { Data = items.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetRoot()
        {

            List<AccountingTreeViewModel> items = GetTree();

            return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetChildren(int id)
        {
            List<AccountingTreeViewModel> items = GetTree(id);

            return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public List<AccountingTreeViewModel> GetTree()
        {
            var items = new List<AccountingTreeViewModel>();
            items = db.AccountingTrees.Where(a => a.active == 1 && a.parent_id == null).Select(a => new AccountingTreeViewModel
            {
                id = a.id,
                name = a.name,
                code = a.code,
                level1 = a.level1,
                level2 = a.level2,
                type = a.type,
                parent_id = a.parent_id,
                range_from = a.range_from,
                range_to = a.range_to

            }).ToList();
            // set items in here

            return items;
        }

        public List<AccountingTreeViewModel> GetTree(int id)
        {
            var items = new List<AccountingTreeViewModel>();
            items = db.AccountingTrees.Where(a => a.active == 1 && a.parent_id == id).Select(a => new AccountingTreeViewModel
            {
                id = a.id,
                name = a.name,
                code = a.code,
                level1 = a.level1,
                level2 = a.level2,
                type = a.type,
                parent_id = a.parent_id,
                range_from = a.range_from,
                range_to = a.range_to

            }).ToList();
            // set items in here

            return items;
        }
    }
}