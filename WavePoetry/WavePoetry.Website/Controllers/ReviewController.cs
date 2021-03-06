﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WavePoetry.DataAccess;
using WavePoetry.Model;

namespace WavePoetry.Web.Controllers
{
    public class ReviewController : Controller
    {
        private ReviewData data;

        public ReviewController()
        {
            data = new ReviewData();
        }

        public ActionResult Create()
        {
            var model = new Review();
            ParseQueryString(Request, model, true);
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Review model)
        {
            LoadLists(model);
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("Review was created.");

                if(model.Redirect == "title")
                    return RedirectToAction("Edit", "Title",  new { id = model.TitleId });
                if (model.Redirect == "contact")
                    return RedirectToAction("Edit", "Contact", new { id = model.ReviewerId });
                return RedirectToAction("Search");
            }

            return View(model);
        }

        public ActionResult DeleteReview(int id)
        {
            data.Delete(id);
            TempData["SuccessMessage"] = "Review was deleted.";
            return RedirectToAction("Search");
        }

        public ActionResult Edit(int id)
        {
            var model = data.GetById(id);
            ParseQueryString(Request, model, false);
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Review model)
        {
            LoadLists(model);
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Update(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("Review was updated.");

                if (model.Redirect == "title")
                    return RedirectToAction("Edit", "Title", new { id = model.TitleId });
                if (model.Redirect == "contact")
                    return RedirectToAction("Edit", "Contact", new { id = model.ReviewerId });
                return RedirectToAction("Search");
            }
            return View(model);
        }

        public ActionResult Search(ReviewSearch search)
        {
            var results = data.Search(search);
            search.Results = results;
            return View(search);
        }

        private void Validate(Review model)
        {
            if (model.ReviewerId < 1)
                ModelState.AddModelError("ReviewerId", "Please select a valid Contact from the auto complete choices");

            if (model.TitleId < 1)
                ModelState.AddModelError("TitleId", "Please select a valid Title from the auto complete choices");

            if (!ModelState.IsValid)
                ModelState.AddModelError(string.Empty, "Please fix the errors below.");
        }

        private void ParseQueryString(HttpRequestBase Request, Review model, bool isCreate)
        {
            model.Redirect = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                model.Redirect = "title";
                if (isCreate)
                {
                    model.TitleId = Convert.ToInt32(Request.QueryString["title"]);
                    model.TitleName = data.GetTitleDisplayNameById(model.TitleId);
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["contact"]))
            {
                model.Redirect = "contact";

                if (isCreate)
                {
                    model.ReviewerId = Convert.ToInt32(Request.QueryString["contact"]);
                    model.ReviewerName = data.GetContactDisplayNameById(model.ReviewerId);
                }
            }
        }

        private void LoadLists(Review model)
        {
            var titleId = model.TitleId.ToString();
            var titleName = string.IsNullOrEmpty(model.TitleName) ? "Search for a Title" : model.TitleName;
            var contactId = model.ReviewerId.ToString();
            var contactName = string.IsNullOrEmpty(model.ReviewerName) ? "Search for a Contact" : model.ReviewerName;
            var contacts = new List<SelectListItem> { new SelectListItem { Text = contactName, Value = contactId } };
            var titles = new List<SelectListItem> { new SelectListItem { Text = titleName, Value = titleId } };
            model.Contacts = new SelectList(contacts, "Value", "Text", null);
            model.Titles = new SelectList(titles, "Value", "Text", null);
        }
    }
}
