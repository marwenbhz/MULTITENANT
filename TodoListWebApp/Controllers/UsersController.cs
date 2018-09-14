using DOMAIN.Entities;
using Microsoft.Owin.Security;
using SERVICES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;


namespace TodoListWebApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {


        private UserService UserService = new UserService();
        private TenantsService TenantsService = new TenantsService();
        private TeamService TeamService = new TeamService();
        private TaskService TaskService = new TaskService();

        // GET: Users
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUser = UserService.GetAll().Where(a => a.Owner == owner);
            return View(currentUser.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {

            User User = UserService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (User == null || (User.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // GET: Users/AddTask/5
        public ActionResult AddTask(int id)
        {

            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var tasks = TaskService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Tasks = new SelectList(tasks, "TaskID", "TaskName");

            User User = UserService.GetById(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // POST: Users/AddTask/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask (User User)
        {
                            
                User ancien = UserService.GetById(User.UserID);
                ancien.TaskCode = User.TaskCode;
                 UserService.Commit();
                

            return RedirectToAction("Index");
                        
        }



        // GET: Users/Create
        public ActionResult Create()
        {
            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var teams = TeamService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Teams = new SelectList(teams, "TeamID", "TeamName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FullName,Bio,Country,UPN,UserName,UserType,TeamCode")] User User)
        {
            /* string idp = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            int i = TenantsService.Get(o => o.IssValue == idp).ID;
            string t = TenantsService.Get(o => o.IssValue == idp).IssValue;
            */

            if (ModelState.IsValid)
            {
                
                //User.TenantID = t;
                //User.TenantCode = 2;
                User.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                UserService.Add(User);
                UserService.Commit();
                return RedirectToAction("Index");
            }

            return View(User);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var teams = TeamService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Teams = new SelectList(teams, "TeamID", "TeamName");
            User User = UserService.GetById(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FullName,Bio,Country,UPN,UserName,UserType,TeamCode")] User User)
        {
            if (ModelState.IsValid)
            {
                //UserService.Entry(User).State = EntityState.Modified;

                User ancien = UserService.GetById(User.UserID);
                ancien.FullName = User.FullName;
                ancien.Bio = User.Bio;
                ancien.Country = User.Country;
                ancien.UserType = User.UserType;
                ancien.TeamCode = User.TeamCode;

                UserService.Commit();
                return RedirectToAction("Index");
            }
            return View(User);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {

            User User = UserService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (User == null || (User.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            User User = UserService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (User == null || (User.Owner != owner))
            {
                return HttpNotFound();
            }
            UserService.Delete(User);
            UserService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
