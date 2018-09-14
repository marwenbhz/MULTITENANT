using DOMAIN.Entities;
using SERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEBAPI.Models;

namespace WEBAPI.Controllers
{
    [AllowAnonymous]
    [Serializable]
    public class ProjectController : ApiController
    {

        ProjectService ProjectService = new ProjectService();
        TaskService TaskService = new TaskService();

        // GET: /Project?tenant-id
        // GET : www/Project?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetTasks")]
        public HttpResponseMessage GetTasks(int id, string tenant_id)
        {
            var Tasks = TaskService.GetAll().Where(o => o.Owner == tenant_id && o.ProjectCode == id);

            List<TaskContainer> nulledtasks = new List<TaskContainer>();

            foreach (Task t in Tasks)
            {
                TaskContainer tc = new TaskContainer();

                tc.TaskID = t.TaskID;
                tc.TaskName = t.TaskName;
                tc.Description = t.Description;
                tc.Plan = t.Plan;
                tc.Goals = t.Goals;
                tc.Requirement = t.Requirement;
                tc.Tools = t.Tools;
                tc.StartDate = t.StartDate;
                tc.DeadLine = t.DeadLine;
                tc.EstimatedTime = t.EstimatedTime;

                switch (t.Complexity)
                {
                    case ComplexityEnum.Easy:
                        tc.ComplexityString = "Easy";
                        break;

                    case ComplexityEnum.Hard:
                        tc.ComplexityString = "Hard";
                        break;

                    case ComplexityEnum.Medium:
                        tc.ComplexityString = "Medium";
                        break;

                    case ComplexityEnum.VeryHard:
                        tc.ComplexityString = "Very Hard";
                        break;
                }

                switch (t.State)
                {
                    case StateEnum.Doing:
                        tc.StateString = "Doing";
                        break;

                    case StateEnum.Done:
                        tc.StateString = "Done";
                        break;

                    case StateEnum.ToDo:
                        tc.StateString = "To Do";
                        break;
                }

                tc.Project = t.project.ProjectName;

                if (t.TeamLeader.UserType == UserType.TeamLeader)
                {

                    tc.TeamLeader = t.TeamLeader.FullName;

                }

                nulledtasks.Add(tc);

            }

            return Request.CreateResponse(HttpStatusCode.OK, nulledtasks);
                       
        }

        public HttpResponseMessage Get(string tenant_id)
        {
            

            List<ProjectContainer> nulledprojects = new List<ProjectContainer>();

            var projects = ProjectService.GetAll().Where(o => o.Owner == tenant_id);

            if (projects == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {


                foreach (Project p in projects )
                {
                    ProjectContainer pc = new ProjectContainer();
                    pc.ProjectID = p.ProjectID;
                    pc.ProjectName = p.ProjectName;
                    pc.Description = p.Description;
                    pc.Plan = p.Plan;
                    pc.Goals = p.Goals;
                    pc.StartDate = p.StartDate;
                    pc.DeadLine = p.DeadLine;
                    pc.EstimatedTime = p.EstimatedTime;
                    pc.Banque = p.banque.BanqueName;
                    pc.Category = p.category.CategoryName;
                    pc.Company = p.company.CompanyName;

                    nulledprojects.Add(pc);
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, nulledprojects);

                }
            
        }

        // GET: /Project?tenant-id/5
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            Project p = ProjectService.GetById(id);

            if (p == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect project id");

            }

            else if (p.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {

                ProjectContainer pc = new ProjectContainer();
                pc.ProjectID = p.ProjectID;
                pc.ProjectName = p.ProjectName;
                pc.Description = p.Description;
                pc.Plan = p.Plan;
                pc.Goals = p.Goals;
                pc.StartDate = p.StartDate;
                pc.DeadLine = p.DeadLine;
                pc.EstimatedTime = p.EstimatedTime;
                pc.Banque = p.banque.BanqueName;
                pc.Category = p.category.CategoryName;
                pc.Company = p.company.CompanyName;
                return Request.CreateResponse(HttpStatusCode.OK, pc);
            }
        }

        [HttpPost]
        // POST: Project?tenant_id
        public void Post(string tenant_id, [FromBody]Project project)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            project.Owner = tenant_id;
            ProjectService.Add(project);
            ProjectService.Commit();
        }

        // PUT: Project?tenant_id=x/5
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]Project newp)
        {

            Project oldp = ProjectService.GetById(id);

            if (oldp.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldp.ProjectName = newp.ProjectName;
                oldp.Description = newp.Description;
                oldp.Plan = newp.Plan;
                oldp.Goals = newp.Goals;
                oldp.StartDate = newp.StartDate;
                oldp.DeadLine = newp.DeadLine;
                oldp.EstimatedTime = newp.EstimatedTime;

                oldp.BanqueCode = newp.BanqueCode;
                oldp.CategoryCode = newp.CategoryCode;
                oldp.CompanyCode = newp.CompanyCode;

                ProjectService.Update(oldp);
                ProjectService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Project updated !");

        }

        // DELETE: Project?tenant_id=x/5
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            Project p = ProjectService.GetById(id);

            if (p.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                ProjectService.Delete(p);
                ProjectService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Project deleted !");
            }
        }
    }
}
