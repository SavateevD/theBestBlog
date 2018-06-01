using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using theBestBlog.Domain;
using theBestBlog.Domain.Objects;
using theBestBlog.WebUI.Models;
using theBestBlog.WebUI.Providers;

namespace theBestBlog.WebUI.Controllers

{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly IAuthProvider _authProvider;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IAuthProvider authProvider, IBlogRepository blogRepository)
        {
            _authProvider = authProvider;
            _blogRepository = blogRepository;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //if (_authProvider.IsLoggedIn)
            //    return RedirectToUrl(returnUrl);

            ViewBag.ReturnUrl = returnUrl;

            return View("Login_", new LoginModel());
        }
        
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && _authProvider.Login(model.UserName, model.Password))
            {
                return RedirectToUrl(returnUrl);
                
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
          // return Json(new { success = false, message = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Logout()
        {
            _authProvider.Logout();
            return RedirectToAction("Login", "Admin");
        }

        private ActionResult RedirectToUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

        //Datatable action
        public ActionResult Posts(JqGridViewModel jqParams)
        {
            //var posts = _blogRepository.Posts(jqParams.page - 1, jqParams.rows,
            //    jqParams.sidx, jqParams.sord == "asc");

            var totalPosts = _blogRepository.TotalPosts(false);
            var posts = _blogRepository.GetAllPosts();

            //var settings = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented
            //};
            //settings.Converters.Add(new NewtonsoftDateTimeConverter());
            //string data = JsonConvert.SerializeObject(posts, settings);

            string data = JsonConvert.SerializeObject(new { data = posts }, new NewtonsoftDateTimeConverter());

            //JObject jo = new JObject(new JProperty("data", new JArray(data)));
            //data = jo.ToString();
            //var v = new { data = data };

            //JObject o = new JObject();
            //o["data"] = data;

            //string json = o.ToString();

            //{ "data":[
            return Content(data, "application/json");

            //List<Employee> empList1 = new List<Employee>();
            //Employee emp = new Employee(1, "Alex", "boss", "Main", 11, 5);
            //empList1.Add(emp);
            //return Json(new { data = empList1 }, JsonRequestBehavior.AllowGet);

            //return Content(JsonConvert.SerializeObject(new
            //{
            //    page = jqParams.page,
            //    records = totalPosts,
            //    rows = posts,
            //    total = Math.Ceiling(Convert.ToDouble(totalPosts) / jqParams.rows)
            //}, new NewtonsoftDateTimeConverter()), "application/json");

        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {

            ViewBag.CatCollection = _blogRepository.GetAllCategories().ToList();
            ViewBag.TagCollection = _blogRepository.GetAllTags().ToList();

            if (id == 0)
            {
                return View(new Post());
            }
            else
            {
                return View(_blogRepository.PostById(id));
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(int Id, string Title, string ShortDescription, string Description, string Meta, string UrlSlug, string Category, string[] tags, bool Published, DateTime PostedOn)
        {
            Post post;
            IList<Tag> tagsList = new List<Tag>();
            IList<Tag> resultTagsList = new List<Tag>();
            tagsList = _blogRepository.GetAllTags().ToList();

            foreach (var tag in tagsList)
            {
                if (tags.Contains(tag.Name))
                resultTagsList.Add(tag);
            }

            if (Id == 0)
            {
                post = new Post()
                {
                    Title = Title,
                    ShortDescription = ShortDescription,
                    Description = Description,
                    Meta = Meta,
                    UrlSlug = UrlSlug,
                    Category = _blogRepository.GetAllCategories().Where(c => c.Id == Int32.Parse(Category)).FirstOrDefault(),
                    Published = Published,
                    PostedOn = DateTime.Now,
                    Modified = DateTime.Now,
                    Tags = resultTagsList
                };

                _blogRepository.AddOrUpdatePost(post);

                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                post = new Post()
                {
                    Id = Id,
                    Title = Title,
                    ShortDescription = ShortDescription,
                    Description = Description,
                    Meta = Meta,
                    UrlSlug = UrlSlug,
                    Category = _blogRepository.GetAllCategories().Where(c => c.Id == Int32.Parse(Category)).FirstOrDefault(),
                    Published = Published,
                    PostedOn = PostedOn,
                    Modified = DateTime.Now,
                    Tags = resultTagsList
                };

                _blogRepository.AddOrUpdatePost(post);




                return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _blogRepository.Delete(id);
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);

        }

    }
}