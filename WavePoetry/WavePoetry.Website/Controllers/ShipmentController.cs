using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WavePoetry.DataAccess;
using WavePoetry.Model;

namespace WavePoetry.Web.Controllers
{
    public class ShipmentController : Controller
    {
        private ShipmentData data;
        private ReviewData reviewData;

        public ShipmentController()
        {
            data = new ShipmentData();
            reviewData = new ReviewData();
        }

        public ActionResult Index(ShipmentSearch search)
        {
            search.Message = "No results found";
            //IEnumerable<ShipmentDetails> results = data.Search(search);
            //search.ResultsToMarkPending = results;
            return View(search);
        }

        [HttpPost]
        public ActionResult Index(ShipmentSearch search, string btnValue)
        {
            search.Message = "No results found";
            //IEnumerable<ShipmentDetails> results = data.Search(search);
            //search.ResultsToMarkPending = results;
            return View(search);
        }

        [HttpPost]
        public ActionResult CreatePending(ShipmentSearch search)
        {
            TempData["SuccessMessage"] = string.Format("\"{0}\" pending shipments were created.", 123);
            return View(new ShipmentSearch());
        }

        [HttpPost]
        public ActionResult MarkSent(ShipmentSearch search)
        {
            TempData["SuccessMessage"] = string.Format("\"{0}\" shipments were marked sent.", 123);
            return View(new ShipmentSearch());
        }

        [HttpPost]
        public ActionResult CreateCsv(ShipmentSearch search)
        {
            TempData["SuccessMessage"] = string.Format("\"{0}\" shipments were marked sent.", 123);
            return View(new ShipmentSearch());
        }

        public ActionResult Create()
        {
            var model = new Shipment();
            ParseQueryString(Request, model, true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Shipment model)
        {
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("Shipment was created.");

                if(model.Redirect == "title")
                    return RedirectToAction("Edit", "Title",  new { id = model.TitleId });
                if (model.Redirect == "contact")
                    return RedirectToAction("Edit", "Contact", new { id = model.ContactId });
                return RedirectToAction("Search");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = data.GetById(id);
            ParseQueryString(Request, model, false);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Shipment model)
        {
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Update(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("Shipment was updated.");

                if (model.Redirect == "title")
                    return RedirectToAction("Edit", "Title", new { id = model.TitleId });
                if (model.Redirect == "contact")
                    return RedirectToAction("Edit", "Contact", new { id = model.ContactId });
                return RedirectToAction("Search");
            }
            return View(model);
        }

        private void Validate(Shipment model)
        {
            if (model.ContactId < 1)
                ModelState.AddModelError("ContactName", "Please select a valid Contact from the auto complete choices");
            
            if (model.TitleId < 1)
                ModelState.AddModelError("TitleName", "Please select a valid Title from the auto complete choices");
                
            if (!ModelState.IsValid)
                ModelState.AddModelError(string.Empty, "Please fix the errors below.");
        }

        private void ParseQueryString(HttpRequestBase Request, Shipment model, bool isCreate)
        {
            model.Redirect = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                model.Redirect = "title";
                if (isCreate)
                {
                    model.TitleId = Convert.ToInt32(Request.QueryString["title"]);
                    model.TitleName = reviewData.GetTitleDisplayNameById(model.TitleId);
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["contact"]))
            {
                model.Redirect = "contact";

                if (isCreate)
                {
                    model.ContactId = Convert.ToInt32(Request.QueryString["contact"]);
                    model.ContactName = reviewData.GetContactDisplayNameById(model.ContactId);
                }
            }
        }
    }
}
