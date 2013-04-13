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

        public ActionResult Create()
        {
            var model = new Title();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Title model)
        {
            if (ModelState.IsValid)
            {
                data.Insert(model, 1);
                TempData["SuccessMessage"] = string.Format("\"{0}\" was created.", model.Name);
                return RedirectToAction("Search");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = data.GetById(id); 
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Title model)
        {
            if (ModelState.IsValid)
            {
                data.Update(model, 1);
                TempData["SuccessMessage"] = string.Format("\"{0}\" was updated.", model.Name);
                return RedirectToAction("Search");
            }
            return View(model);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult LookupVenue(string searchText, string maxResults)
        //{
        //    int take = Convert.ToInt32(maxResults);
        //    IEnumerable<VenueDetails> venues = venueData.LookupVenue(searchText, take);
        //    return Json(venues);
        //}

        public ActionResult Search(TitleSearch search)
        {
            var results = data.Search(search);
            search.Results = results;
            return View(search);
        }

    }
}
