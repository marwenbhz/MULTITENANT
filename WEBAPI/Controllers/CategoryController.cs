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
    public class CategoryController : ApiController
    {

        CategoryService CategoryService = new CategoryService();
        ProjectService ProjectService = new ProjectService();

        // GET: /Category?tenant-id
        // GET : www/Category?tenant_id=4ea20902-065a-45dc-a166-de8110439f55

        [ActionName("GetProjects")]
        public HttpResponseMessage GetProjects(int id, string tenant_id)
        {
            var Projects = ProjectService.GetAll().Where(o => o.Owner == tenant_id && o.CategoryCode == id);

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

            List <CategoryContainer> nulledcategoy = new List<CategoryContainer>();

            var Categorys = CategoryService.GetAll().Where(o => o.Owner == tenant_id);

            if (Categorys == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No content or wrong tenant id");
            }

            else {

                foreach (Category c in Categorys)
                {
                    CategoryContainer cc = new CategoryContainer();
                    cc.CategoryID = c.CategoryID;
                    cc.CategoryName = c.CategoryName;
                    cc.CategoryDescription = c.CategoryDescription;
                    nulledcategoy.Add(cc);
                }

                return Request.CreateResponse(HttpStatusCode.OK, nulledcategoy);

            }

        }

        // GET: /Category/5?tenant-id
        public HttpResponseMessage Get(string tenant_id, int id)
        {

            Category c = CategoryService.GetById(id);

            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect category id");

            }

            else if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else {

                CategoryContainer cc = new CategoryContainer();
                cc.CategoryID = c.CategoryID;
                cc.CategoryName = c.CategoryName;
                cc.CategoryDescription = c.CategoryDescription;
                return Request.CreateResponse(HttpStatusCode.OK, cc);
            } }

        [HttpPost]
        // POST: Category?tenant_id
        public void Post(string tenant_id, [FromBody]Category Category)
        {
            //ID = AutoIncrement & Owner = tenant_id.

            Category.Owner = tenant_id;
            CategoryService.Add(Category);
            CategoryService.Commit();
        }

        // PUT: Category/5?tenant_id
        public HttpResponseMessage Put(string tenant_id, int id, [FromBody]Category newc)
        {

            Category oldc = CategoryService.GetById(id);

            if (oldc.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                //ID & Owner are the same on update.

                oldc.CategoryName = newc.CategoryName;
                oldc.CategoryDescription = newc.CategoryDescription;
                CategoryService.Update(oldc);
                CategoryService.Commit();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Category updated !");

        }

        // DELETE: Category/5?tenant_id
        public HttpResponseMessage Delete(string tenant_id, int id)
        {

            Category c = CategoryService.GetById(id);

            if (c.Owner != tenant_id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not allowed, check your tenant id");
            }

            else
            {
                CategoryService.Delete(c);
                CategoryService.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "Category deleted !");
            }
        }
    }
}
