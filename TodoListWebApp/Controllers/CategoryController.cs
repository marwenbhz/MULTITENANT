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
    public class CategoryController : Controller
    {
        private CategoryService CategoryService = new CategoryService();

        // GET: Category
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserCategory = CategoryService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserCategory.ToList());
        }

        public ActionResult Projects(int id)
        {

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var projects = CategoryService.Get(o => o.CategoryID == id).Projects;

            if (projects != null)
            {

                foreach (Project p in projects)
                {
                    if (p.Owner != owner)

                    {
                        projects.Remove(p);
                    }
                }

                return View(projects.ToList());
            }
            return RedirectToAction("Index");
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {

            Category Category = CategoryService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Category == null || (Category.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,CategoryDescription")] Category Category)
        {
            if (ModelState.IsValid)
            {
                Category.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                CategoryService.Add(Category);
                CategoryService.Commit();
                return RedirectToAction("Index");
            }

            return View(Category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {

            Category Category = CategoryService.GetById(id);
            if (Category == null)
            {
                return HttpNotFound();
            }
            return View(Category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,CategoryDescription")] Category Category)
        {
            if (ModelState.IsValid)
            {
                //CategoryService.Entry(Category).State = EntityState.Modified;
                Category ancien = CategoryService.GetById(Category.CategoryID);
                ancien.CategoryName = Category.CategoryName;
                ancien.CategoryDescription = Category.CategoryDescription;
                CategoryService.Commit();
                return RedirectToAction("Index");
            }
            return View(Category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {

            Category Category = CategoryService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Category == null || (Category.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Category Category = CategoryService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Category == null || (Category.Owner != owner))
            {
                return HttpNotFound();
            }
            CategoryService.Delete(Category);
            CategoryService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CategoryService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
