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
    public class TaskController : Controller
    {
        private TaskService TaskService = new TaskService();
        private UserService UserService = new UserService();
        private ProjectService ProjectService = new ProjectService();

        // GET: Task
        public ActionResult Index()
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var currentUserTask = TaskService.GetAll().Where(a => a.Owner == owner);
            return View(currentUserTask.ToList());
        }

        public ActionResult TeamMembers(int id)
        {

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var members = TaskService.Get(o => o.TaskID == id).TeamMembers;

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

        // GET: Task/Details/5
        public ActionResult Details(int id)
        {

            Task Task = TaskService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Task == null || (Task.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Task);
        }

        // GET: Task/Create
        public ActionResult Create()
        {

            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var teamleaders = UserService.GetAll().Where(o => o.Owner == owner && o.UserType == UserType.TeamLeader);
            ViewBag.TeamLeaders = new SelectList(teamleaders, "UserID", "FullName");

            var projects = ProjectService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Projects = new SelectList(projects, "ProjectID", "ProjectName");

            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,TaskName,Description,ProjectCode,Plan,Goals,Requirement,Tools,StartDate,DeadLine,EstimatedTime,Complexity,TeamLeaderCode")] Task Task)
        {
            if (ModelState.IsValid)
            {
                Task.State = StateEnum.ToDo;
                Task.Owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                TaskService.Add(Task);
                TaskService.Commit();
                return RedirectToAction("Index");
            }

            return View(Task);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {

            var owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            var teamleaders = UserService.GetAll().Where(o => o.Owner == owner && o.UserType == UserType.TeamLeader);
            ViewBag.TeamLeaders = new SelectList(teamleaders, "UserID", "FullName");

            var projects = ProjectService.GetAll().Where(o => o.Owner == owner);
            ViewBag.Projects = new SelectList(projects, "ProjectID", "ProjectName");

            Task Task = TaskService.GetById(id);
            if (Task == null)
            {
                return HttpNotFound();
            }
            return View(Task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,TaskName,Description,ProjectCode,Plan,Goals,Requirement,Tools,EstimatedTime,Complexity,State,TeamLeaderCode")] Task Task)
        {
            if (ModelState.IsValid)
            {
                //TaskService.Entry(Task).State = EntityState.Modified;
                Task ancien = TaskService.GetById(Task.TaskID);
                ancien.TaskName = Task.TaskName;
                ancien.ProjectCode = Task.ProjectCode;
                ancien.TeamLeaderCode = Task.TeamLeaderCode;
                ancien.Description = Task.Description;
                ancien.Requirement = Task.Requirement;
                ancien.Plan = Task.Plan;
                ancien.Tools = Task.Tools;
                ancien.State = Task.State;
                ancien.Complexity = Task.Complexity;
                ancien.Goals = Task.Goals;
                ancien.EstimatedTime = Task.EstimatedTime;
                TaskService.Commit();
                return RedirectToAction("Index");
            }
            return View(Task);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {

            Task Task = TaskService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Task == null || (Task.Owner != owner))
            {
                return HttpNotFound();
            }
            return View(Task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Task Task = TaskService.GetById(id);
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            if (Task == null || (Task.Owner != owner))
            {
                return HttpNotFound();
            }
            TaskService.Delete(Task);
            TaskService.Commit();
            return RedirectToAction("Index");
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TaskService.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
