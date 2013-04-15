using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WavePoetry.DataAccess;
using WavePoetry.Model;

namespace WavePoetry.Web.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        ContactData data;
        AdminData adminData;

        public ContactController()
        {
            data = new ContactData();
            adminData = new AdminData();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult LookupAuthor(string searchText, string maxResults)
        {
            int take = Convert.ToInt32(maxResults);
            IEnumerable<ContactDetails> authors = data.LookupContact(searchText, take);
            return Json(authors);
        }

        public ActionResult Create()
        {
            var model = new Contact();
            model.LoadCats(adminData.GetAllContactCats());
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Contact model)
        {
            Validate(model);
            if (ModelState.IsValid)
            {
                data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}, {1}\" was created.", model.Lastname, model.Firstname );
                return RedirectToAction("Index");
            }

            model.LoadCats(adminData.GetAllContactCats());
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = data.GetById(id);
            model.LoadCats(adminData.GetAllContactCats());
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Contact model)
        {
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Update(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}, {1}\" was updated.", model.Lastname, model.Firstname);
                return RedirectToAction("Index");
            }

            model.LoadCats(adminData.GetAllContactCats());
            return View(model);
        }

        public ActionResult Index(ContactSearch search)
        {
            search.LoadCats(adminData.GetAllContactCats());
            var results = data.Search(search);
            search.Results = results;
            return View(search);
        }

        private void Validate(Contact model)
        {
            //if (model.Author < 1)
            //{
            //    ModelState.AddModelError("AuthorName", "Please select a valid Author from the auto complete choices");
            //    ModelState.AddModelError(string.Empty, "Please fix the errors below.");
            //}
        }
    }
}
