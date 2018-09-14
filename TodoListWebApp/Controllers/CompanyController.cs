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
    public class CompanyController : Controller
    {
        private CompanyService CompanyService = new CompanyService();

        // GET: Company
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserCompany = CompanyService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserCompany.ToList());
        }

        public ActionResult Projects(int id)
        {

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var projects = CompanyService.Get(o => o.CompanyID == id).Projects;

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

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {

            Company Company = CompanyService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Company == null || (Company.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyID,CompanyName,Address,PhoneNumber,Country")] Company Company)
        {
            if (ModelState.IsValid)
            {
                Company.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                CompanyService.Add(Company);
                CompanyService.Commit();
                return RedirectToAction("Index");
            }

            return View(Company);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {

            Company Company = CompanyService.GetById(id);
            if (Company == null)
            {
                return HttpNotFound();
            }
            return View(Company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyID,CompanyName,Address,PhoneNumber,Country")] Company Company)
        {
            if (ModelState.IsValid)
            {
                //CompanyService.Entry(Company).State = EntityState.Modified;
                Company ancien = CompanyService.GetById(Company.CompanyID);
                ancien.CompanyName = Company.CompanyName;
                ancien.Address = Company.Address;
                ancien.PhoneNumber = Company.PhoneNumber;
                ancien.Country = Company.Country;
                CompanyService.Commit();
                return RedirectToAction("Index");
            }
            return View(Company);
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {

            Company Company = CompanyService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Company == null || (Company.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Company Company = CompanyService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Company == null || (Company.Owner != owner))
            {
                return HttpNotFound();
            }
            CompanyService.Delete(Company);
            CompanyService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CompanyService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
