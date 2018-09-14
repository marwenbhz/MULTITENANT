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
    public class BanqueController : ApiController
    {

        BanqueService BanqueService = new BanqueService();
        ProjectService ProjectService = new ProjectService();
        

        // GET: /Banque?tenant-id
        // GET : www/Banque?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetProjects")]
        public HttpResponseMessage GetProjects(int id, string tenant_id)
        {
            var Projects = ProjectService.GetAll().Where(o => o.Owner == tenant_id && o.BanqueCode == id);

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

            List<BanqueContainer> nulledbanks = new List<BanqueContainer>();

            var Banques = BanqueService.GetAll().Where(o => o.Owner == tenant_id);

            if (Banques == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {

                foreach (Banque b in Banques)
                {

                    BanqueContainer bc = new BanqueContainer();
                    bc.ID = b.ID;
                    bc.BanqueName = b.BanqueName;
                    bc.Description = b.Description;
                    nulledbanks.Add(bc);

                }

                return Request.CreateResponse(HttpStatusCode.OK, nulledbanks);

            }

        }

        // GET: /Banque/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            Banque b = BanqueService.GetById(id);

            if (b == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect bank id");

            }

            else if (b.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {

                BanqueContainer bc = new BanqueContainer();
                bc.ID = b.ID;
                bc.BanqueName = b.BanqueName;
                bc.Description = b.Description;
                
                return Request.CreateResponse(HttpStatusCode.OK, bc);

            }
        }

        [HttpPost]
        // POST: Banque?tenant_id
        public void Post(string tenant_id, [FromBody]Banque Banque)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            Banque.Owner = tenant_id;
            BanqueService.Add(Banque);
            BanqueService.Commit();
        }

        // PUT: Banque/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]Banque newb)
        {

            Banque oldb = BanqueService.GetById(id);

            if (oldb.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldb.BanqueName = newb.BanqueName;
                oldb.Description = newb.Description;
                BanqueService.Update(oldb);
                BanqueService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Banque updated !");

        }

        // DELETE: Banque/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            Banque b = BanqueService.GetById(id);

            if (b.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                BanqueService.Delete(b);
                BanqueService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Banque deleted !");
            }
        }
    }
}
