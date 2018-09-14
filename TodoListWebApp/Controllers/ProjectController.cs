using DOMAIN.Entities;
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
    public class ProjectController : Controller
    {
        private ProjectService ProjectService = new ProjectService();
        private CategoryService CategoryService = new CategoryService();
        private CompanyService CompanyService = new CompanyService();
        private BanqueService BanqueService = new BanqueService();
        
        // GET: Project
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserProject = ProjectService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserProject.ToList());
        }

        public ActionResult GetAllTasks(int id)
        {

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var tasks = ProjectService.Get(o => o.ProjectID == id).Tasks;

            if (tasks != null)
            {

                foreach (Task t in tasks)
                {
                    if (t.Owner != owner)

                    {
                        tasks.Remove(t);
                    }
                }

                return View(tasks.ToList());
            }
            return RedirectToAction("Index");
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {

            Project Project = ProjectService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Project == null || (Project.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var categories = CategoryService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");

            var companies = CompanyService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Companies = new SelectList(companies, "CompanyID", "CompanyName");

            var banques = BanqueService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Banques = new SelectList(banques, "ID", "BanqueName");

            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectName,CategoryCode,CompanyCode,TeamLeaderCode,BanqueCode,Description,Plan,Goals,StartDate,DeadLine,EstimatedTime")] Project Project)
        {

            
            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            if (ModelState.IsValid)
            {
                Project.Owner = owner;
                ProjectService.Add(Project);
                ProjectService.Commit();
                return RedirectToAction("Index");
            }

            return View(Project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {

            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var categories = CategoryService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");

            var companies = CompanyService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Companies = new SelectList(companies, "CompanyID", "CompanyName");

            var banques = BanqueService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Banques = new SelectList(banques, "ID", "BanqueName");

            Project Project = ProjectService.GetById(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,CategoryCode,CompanyCode,TeamLeaderCode,BanqueCode,Description,Plan,Goals,StartDate,DeadLine,EstimatedTime")] Project Project)
        {
            if (ModelState.IsValid)
            {
                Project ancien = ProjectService.GetById(Project.ProjectID);
                ancien.ProjectName = Project.ProjectName;
                ancien.CategoryCode = Project.CategoryCode;
                ancien.CompanyCode = Project.CompanyCode;
                ancien.BanqueCode = Project.BanqueCode;
                ancien.Description = Project.Description;
                ancien.Plan = Project.Plan;
                ancien.Goals = Project.Goals;
                ancien.EstimatedTime = Project.EstimatedTime;

                //ProjectService.Entry(Project).State = EntityState.Modified;
                ProjectService.Commit();
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {

            Project Project = ProjectService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Project == null || (Project.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Project Project = ProjectService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Project == null || (Project.Owner != owner))
            {
                return HttpNotFound();
            }
            ProjectService.Delete(Project);
            ProjectService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ProjectService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
