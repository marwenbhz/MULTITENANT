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
    public class TeamController : ApiController
    {

        TeamService TeamService = new TeamService();
        UserService UserService = new UserService();

        // GET: /Team?tenant-id
        // GET : www/Team?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetMembers")]
        public HttpResponseMessage GetMembers(int id, string tenant_id)
        {
            var Members = UserService.GetAll().Where(o => o.Owner == tenant_id && o.TeamCode == id);

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

            List<TeamContainer> nulledteams = new List<TeamContainer>();

            var Teams = TeamService.GetAll().Where(o => o.Owner == tenant_id);


            if (Teams == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {


                foreach (Team t in Teams)
                {
                    TeamContainer tc = new TeamContainer();
                    tc.TeamID = t.TeamID;
                    tc.TeamName = t.TeamName;
                    tc.TeamDescription = t.TeamDescription;
                    nulledteams.Add(tc);
                }
                



                return Request.CreateResponse(HttpStatusCode.OK, nulledteams);

            }

        }

        // GET: /Team/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            Team t = TeamService.GetById(id);

            if (t == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect team id");

            }

            else if (t.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {

                TeamContainer tc = new TeamContainer();
                tc.TeamID = t.TeamID;
                tc.TeamName = t.TeamName;
                tc.TeamDescription = t.TeamDescription;
                return Request.CreateResponse(HttpStatusCode.OK, tc);
                 }
        }

        [HttpPost]
        // POST: Team?tenant_id
        public void Post(string tenant_id, [FromBody]Team Team)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            Team.Owner = tenant_id;
            TeamService.Add(Team);
            TeamService.Commit();
        }

        // PUT: Team/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]Team newc)
        {

            Team oldc = TeamService.GetById(id);

            if (oldc.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldc.TeamName = newc.TeamName;
                oldc.TeamDescription = newc.TeamDescription;
                TeamService.Update(oldc);
                TeamService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Team updated !");

        }

        // DELETE: Team/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            Team c = TeamService.GetById(id);

            if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                TeamService.Delete(c);
                TeamService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Team deleted !");
            }
        }
    }
}
