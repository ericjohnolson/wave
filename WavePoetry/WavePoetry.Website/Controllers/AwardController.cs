using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WavePoetry.Web.Controllers
{
    public class AwardController : Controller
    {
        //private TitleData data;

        //public TitleController()
        //{
        //    data = new TitleData();
        //}

        //public ActionResult Create()
        //{
        //    var model = new Title { PubDate = DateTime.Now.Date };
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Create(Title model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        data.Insert(model, (Session["LoggedInUser"] as user).id);
        //        TempData["SuccessMessage"] = string.Format("\"{0}\" was created.", model.Name);
        //        return RedirectToAction("Search");
        //    }
        //    if (model.Author < 1)
        //    {
        //        ModelState.AddModelError("AuthorName", "Please select a valid Author from the auto complete choices");
        //        ModelState.AddModelError(string.Empty, "Please fix the errors below.");
        //    }

        //    return View(model);
        //}

        //public ActionResult Edit(int id)
        //{
        //    var model = data.GetById(id); 
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(Title model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        data.Update(model, (Session["LoggedInUser"] as user).id);
        //        TempData["SuccessMessage"] = string.Format("\"{0}\" was updated.", model.Name);
        //        return RedirectToAction("Search");
        //    }
        //    return View(model);
        //}

        //public ActionResult Search(TitleSearch search)
        //{
        //    var results = data.Search(search);
        //    search.Results = results;
        //    return View(search);
        //}
    }
}
