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
    public class UserController : ApiController
    {

        UserService UserService = new UserService();

        // GET: /User?tenant-id
        // GET : www/User?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        public HttpResponseMessage Get(string tenant_id)
        {

            
            List<UserContainer> nulledusers = new List<UserContainer>();

            var Users = UserService.GetAll().Where(o => o.Owner == tenant_id);

            if (Users == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {

                foreach (User u in Users)
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

        }

        // GET: /User/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            User u = UserService.GetById(id);

            if (u == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect user id");

            }

            else if (u.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
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


                return Request.CreateResponse(HttpStatusCode.OK, uc);
        }
        }

        [HttpPost]
        // POST: User?tenant_id
        public void Post(string tenant_id, [FromBody]User User)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            User.Owner = tenant_id;
            UserService.Add(User);
            UserService.Commit();
        }

        // PUT: User/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]User newc)
        {

            User oldc = UserService.GetById(id);

            if (oldc.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner & Username are the same on update.

                oldc.TaskCode = newc.TaskCode;
                UserService.Update(oldc);
                UserService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "User updated !");

        }

        // DELETE: User/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            User c = UserService.GetById(id);

            if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                UserService.Delete(c);
                UserService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "User deleted !");
            }
        }
    }
}
