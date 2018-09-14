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
    public class TaskController : ApiController
    {

        TaskService TaskService = new TaskService();
        UserService UserService = new UserService();

        // GET: /Task?tenant-id
        // GET : www/Task?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetMembers")]
        public HttpResponseMessage GetMembers(int id, string tenant_id)
        {
            var Members = UserService.GetAll().Where(o => o.Owner == tenant_id && o.TaskCode == id);

            List<UserContainer> nulledusers = new List<UserContainer>();

                foreach (User u in Members)
                {
                    UserContainer uc = new UserContainer();
                    uc.UserID = u.UserID;
                    uc.Bio = u.Bio;
                    uc.FullName = u.FullName;

                switch (u.Country)
                {
                    case Countries.Tunisia:
                        uc.Country = "Tunisia";
                        break;

                    case Countries.France:
                        uc.Country = "France";
                        break;

                    case Countries.Belgium:
                        uc.Country = "Belgium";
                        break;

                    case Countries.UK:
                        uc.Country = "UK";
                        break;

                    case Countries.USA:
                        uc.Country = "USA";
                        break;
                }

                uc.UPN = u.UPN;

                    switch (u.UserType)
                    {
                        case UserType.TeamLeader:
                            uc.UserType = "Team Leader";
                            break;

                        case UserType.TeamMember:
                            uc.UserType = "Team Member";
                            break;

                    }

                uc.TeamName = u.team.TeamName;
                uc.Username = u.Username;

                if (u.MemberTask == null)
                {
                    uc.TaskName = "No task affected";
                }

                else
                {
                    uc.TaskName = u.MemberTask.TaskName;
                }

                nulledusers.Add(uc);
                }

                return Request.CreateResponse(HttpStatusCode.OK, nulledusers);

            }


        public HttpResponseMessage Get(string tenant_id)
        {

            List<TaskContainer> nulledtasks = new List<TaskContainer>();

            var Tasks = TaskService.GetAll().Where(o => o.Owner == tenant_id);

            if (Tasks == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {


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

        }

        // GET: /Task/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            DOMAIN.Entities.Task t = TaskService.GetById(id);

            if (t == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect task id");

            }

            else if (t.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {

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
                return Request.CreateResponse(HttpStatusCode.OK, tc);
            }
        }

        [HttpPost]
        // POST: Task?tenant_id
        public void Post(string tenant_id, [FromBody]DOMAIN.Entities.Task t)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            t.Owner = tenant_id;
            TaskService.Add(t);
            TaskService.Commit();
        }

        // PUT: Task/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]DOMAIN.Entities.Task newc)
        {

            DOMAIN.Entities.Task oldc = TaskService.GetById(id);

            if (oldc.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldc.TaskName = newc.TaskName;
                oldc.Description = newc.Description;
                oldc.Plan = newc.Plan;
                oldc.Goals = newc.Goals;
                oldc.Requirement = newc.Requirement;
                oldc.Tools = newc.Tools;
                oldc.StartDate = newc.StartDate;
                oldc.DeadLine = newc.DeadLine;
                oldc.EstimatedTime = newc.EstimatedTime;
                oldc.Complexity = newc.Complexity;
                oldc.State = newc.State;

                oldc.ProjectCode = newc.ProjectCode;

                TaskService.Update(oldc);
                TaskService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Task updated !");

        }

        // DELETE: Task/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            DOMAIN.Entities.Task c = TaskService.GetById(id);

            if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                TaskService.Delete(c);
                TaskService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Task deleted !");
            }
        }
    }
}
