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
    public class CompanyController : ApiController
    {

        CompanyService CompanyService = new CompanyService();
        ProjectService ProjectService = new ProjectService();

        // GET: /Company?tenant-id
        // GET : www/Company?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetProjects")]
        public HttpResponseMessage GetProjects(int id, string tenant_id)
        {
            var Projects = ProjectService.GetAll().Where(o => o.Owner == tenant_id && o.CompanyCode == id);

            List<ProjectContainer> nulledprojects = new List<ProjectContainer>();

            foreach (Project p in Projects)
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

        public HttpResponseMessage Get(string tenant_id)
        {

            List<CompanyContainer> nulledcompany = new List<CompanyContainer>();

            var Companys = CompanyService.GetAll().Where(o => o.Owner == tenant_id);

            if (Companys == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {

                foreach (Company c in Companys)
                {
                    CompanyContainer cc = new CompanyContainer();
                    cc.CompanyID = c.CompanyID;
                    cc.CompanyName = c.CompanyName;
                    cc.Address = c.Address;
                    cc.PhoneNumber = c.PhoneNumber;

                    switch (c.Country)
                    {
                        case Countries.Tunisia:
                        cc.Country = "Tunisia";
                        break;

                        case Countries.France:
                        cc.Country = "France";
                        break;

                        case Countries.Belgium:
                        cc.Country = "Belgium";
                        break;

                        case Countries.UK:
                        cc.Country = "UK";
                        break;

                        case Countries.USA:
                        cc.Country = "USA";
                        break;
                    }

                   
                    nulledcompany.Add(cc);


                }

                return Request.CreateResponse(HttpStatusCode.OK, nulledcompany);

            }

        }

        // GET: /Company/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            Company c = CompanyService.GetById(id);

            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect company id");

            }

            else if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {


                CompanyContainer cc = new CompanyContainer();
                cc.CompanyID = c.CompanyID;
                cc.CompanyName = c.CompanyName;
                cc.Address = c.Address;
                cc.PhoneNumber = c.PhoneNumber;

                switch (c.Country)
                {
                    case Countries.Tunisia:
                        cc.Country = "Tunisia";
                        break;

                    case Countries.France:
                        cc.Country = "France";
                        break;

                    case Countries.Belgium:
                        cc.Country = "Belgium";
                        break;

                    case Countries.UK:
                        cc.Country = "UK";
                        break;

                    case Countries.USA:
                        cc.Country = "USA";
                        break;
                }

                return Request.CreateResponse(HttpStatusCode.OK, cc);

            }
        }

        [HttpPost]
        // POST: Company?tenant_id
        public void Post(string tenant_id, [FromBody]Company Company)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            Company.Owner = tenant_id;
            CompanyService.Add(Company);
            CompanyService.Commit();
        }

        // PUT: Company/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]Company newc)
        {

            Company oldc = CompanyService.GetById(id);

            if (oldc.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldc.CompanyName = newc.CompanyName;
                oldc.Address = newc.Address;
                oldc.PhoneNumber = newc.PhoneNumber;
                oldc.Country = newc.Country;
                CompanyService.Update(oldc);
                CompanyService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Company updated !");

        }

        // DELETE: Company/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            Company c = CompanyService.GetById(id);

            if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                CompanyService.Delete(c);
                CompanyService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Company deleted !");
            }
        }
    }
}
