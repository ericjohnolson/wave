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
    public class TitleController : Controller
    {
        private TitleData data;

        public TitleController()
        {
            data = new TitleData();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult LookupTitle(string searchText, string maxResults)
        {
            int take = Convert.ToInt32(maxResults);
            IEnumerable<TitleDetails> titles = data.LookupTitle(searchText, take);
            var titles2 = titles.Select(t => new TitleDetails
            {
                Title = t.Title + " (" + t.PubDate.ToShortDateString() + ")",
                TitleId = t.TitleId,
                PubDate = t.PubDate
            });
            return Json(titles2);
        }

        [AllowAnonymous]
        public JsonResult LookupTitle(string term)
        {
            int take = 20;
            IEnumerable<TitleDetails> titles = data.LookupTitle(term, take);

            return Json(titles.Select(t => new TitleDetails
            {
                Title = t.Title + " (" + t.PubDate.ToShortDateString() + ")",
                TitleId = t.TitleId,
                PubDate = t.PubDate
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var model = new Title { PubDate = DateTime.Now.Date };
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Title model)
        {
            LoadLists(model);
            Validate(model);
            if (ModelState.IsValid)
            {
                Title saved = data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}\" was created.", model.Name);
                return RedirectToAction("Edit", new { id = saved.Id });
            }

            return View(model);
        }

        public ActionResult DeleteTitle(int id)
        {
            data.Delete(id);
            TempData["SuccessMessage"] = "Title was deleted.";
            return RedirectToAction("Search", "Title");
        }

        public ActionResult Edit(int id)
        {
            var model = data.GetById(id);
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Title model)
        {
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Update(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}\" was updated.", model.Name);
            }

            var saved = data.GetById(model.Id);
            LoadLists(saved);
            return View(saved);
        }

        public ActionResult Search(TitleSearch search)
        {
            var results = data.Search(search);
            search.Results = results;
            return View(search);
        }

        private void Validate(Title model)
        {
            if (model.Author < 1)
            {
                ModelState.AddModelError("Author", "Please select a valid Author from the auto complete choices");
                ModelState.AddModelError(string.Empty, "Please fix the errors below.");
            }
        }

        private void LoadLists(Title model)
        {
            var contactId = model.Author.ToString();
            var contactName = string.IsNullOrEmpty(model.AuthorName) ? "Search for a Contact" : model.AuthorName;
            var contacts = new List<SelectListItem> { new SelectListItem { Text = contactName, Value = contactId } };
            model.ContactSelects = new SelectList(contacts, "Value", "Text", null);
        }
    }
}
