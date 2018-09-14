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
    public class BanquesController : Controller
    {


        private BanqueService BanqueService = new BanqueService();
            
         // GET: Banques
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserBanque = BanqueService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserBanque.ToList());
        }

        public ActionResult Projects(int id)
        {

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var projects = BanqueService.Get(o => o.ID == id).MyProjects;

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

        // GET: Banques/Details/5
        public ActionResult Details(int id)
        {

            Banque banque = BanqueService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (banque == null || (banque.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(banque);
        }

        // GET: Banques/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BanqueName,Description")] Banque banque)
        {
            if (ModelState.IsValid)
            {
               
                banque.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                BanqueService.Add(banque);
               BanqueService.Commit();
               return RedirectToAction("Index");
            }

            return View(banque);
        }

        // GET: Banques/Edit/5
        public ActionResult Edit(int id)
        {

            Banque banque = BanqueService.GetById(id);
            if (banque == null)
            {
                return HttpNotFound();
            }
            return View(banque);
        }

        // POST: Banques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BanqueName,Description")] Banque banque)
        {
            if (ModelState.IsValid)
            {
                //BanqueService.Entry(banque).State = EntityState.Modified;
                BanqueService.Commit();
                return RedirectToAction("Index");
            }
            return View(banque);
        }

        // GET: Banques/Delete/5
        public ActionResult Delete(int id)
        {

            Banque banque = BanqueService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (banque == null || (banque.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(banque);
        }

        // POST: Banques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Banque banque = BanqueService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (banque == null || (banque.Owner != owner))
            {
                return HttpNotFound();
            }
            BanqueService.Delete(banque);
            BanqueService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BanqueService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
