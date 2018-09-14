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
    public class TeamController : Controller
    {
        private TeamService TeamService = new TeamService();
        private UserService UserService = new UserService();

        // GET: Team
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserTeam = TeamService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserTeam.ToList());
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {

            Team Team = TeamService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Team == null || (Team.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Team);
        }

        // GET: Team/Members

        public ActionResult Members (int id)
        {
            
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var members = TeamService.Get(o => o.TeamID == id).Users;
            
            if (members != null)
            { 

            foreach (User m in members)
            {
                if (m.Owner != owner)

                {
                    members.Remove(m);
                }
            }
                 
            return View(members.ToList());
             }
            return RedirectToAction("Index"); 
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamID,TeamName,TeamLeaderCode,TeamDescription,teamleader.AccountID")] Team Team)
        {
            if (ModelState.IsValid)
            {
                Team.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                TeamService.Add(Team);
                TeamService.Commit();
                return RedirectToAction("Index");
            }

            return View(Team);
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {

            Team Team = TeamService.GetById(id);
            if (Team == null)
            {
                return HttpNotFound();
            }
            return View(Team);
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamID,TeamName,TeamLeaderCode,TeamDescription")] Team Team)
        {
            if (ModelState.IsValid)
            {
                Team ancien = TeamService.GetById(Team.TeamID);
                ancien.TeamName = Team.TeamName;
                ancien.TeamDescription = Team.TeamDescription;
                //TeamService.Entry(Team).State = EntityState.Modified;
                TeamService.Commit();
                return RedirectToAction("Index");
            }
            return View(Team);
        }

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {

            Team Team = TeamService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Team == null || (Team.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Team Team = TeamService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Team == null || (Team.Owner != owner))
            {
                return HttpNotFound();
            }
            TeamService.Delete(Team);
            TeamService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TeamService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
