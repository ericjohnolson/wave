using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LINQtoCSV;
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
            return View(search);
        }

        [HttpPost]
        public ActionResult Index(ShipmentSearch search, string btnValue)
        {
            search.HideOptions = false;
            if (btnValue == "Find Marked Contacts")
            {
                search.ContactResults = data.SearchForContacts(search);
                if (search.ContactResults.Count() == 0)
                    search.Message = "No contacts matching your search found.";
                else
                    search.HideOptions = true;
            }
            else if (btnValue == "Find Pending Shipments")
            {
                search.SelectedTitlesCsv = String.Join(",", search.SelectedTitles);
                search.ShipmentResults = data.SearchForShipments(search);
                if (search.ShipmentResults.Count() == 0)
                    search.Message = "No pending shipments matching your search found.";
                else
                    search.HideOptions = true;
            }
            else
                search.Message = "Unknown action";

            return View(search);
        }

        [HttpPost]
        public ActionResult CreatePending(ShipmentSearch search)
        {
            int total = data.CreatePendingShipments(search, (Session["LoggedInUser"] as user).id);
            TempData["SuccessMessage"] = string.Format("\"{0}\" pending shipments were created.", total);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult MarkSent(ShipmentSearch search)
        {
            search.SelectedTitles = search.SelectedTitlesCsv.Split(',').Select(int.Parse).ToArray();
            int total = data.MarkShipmentsSent(search, (Session["LoggedInUser"] as user).id);
            TempData["SuccessMessage"] = string.Format("\"{0}\" shipments were marked sent.", total);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public FileResult CreateCsv(ShipmentSearch search)
        {
            search.SelectedTitles = search.SelectedTitlesCsv.Split(',').Select(int.Parse).ToArray();
            List<ShipmentCsvLine> items = data.GetPendingShipments(search);

            foreach (ShipmentCsvLine item in items)
            {
                item.SubscriberNumber = item.SubNumber.HasValue ? item.SubNumber.Value.ToString() : "";
                foreach (var title in item.TitleList)
                    item.Titles += title.TitleName + " (" + title.Quantity + "),";
            }

            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                //SeparatorChar = '\t', // tab delimited
                EnforceCsvColumnAttribute = true
                // FirstLineHasColumnNames = true, // no column names in first record
                // FileCultureName = "en-US" // use formats used in The Netherlands
            };

            CsvContext cc = new CsvContext();
            using (MemoryStream ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    cc.Write(items, tw, outputFileDescription);
                }

                return File(ms.ToArray(), "text/csv", "PendingShipments.csv");
            }
        }

        public ActionResult Create()
        {
            var model = new Shipment();
            ParseQueryString(Request, model, true);
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Shipment model)
        {
            LoadLists(model);
            Validate(model);

            if (ModelState.IsValid)
            {
                data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("Shipment was created.");

                if (model.Redirect == "title")
                    return RedirectToAction("Edit", "Title", new { id = model.TitleId });
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
            LoadLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Shipment model)
        {
            LoadLists(model);
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

        public ActionResult DeleteShipment(int id, string type, int contactid, int titleid)
        {
            data.Delete(id);
            if (type == "title")
                return RedirectToAction("Edit", "Title", new { id = titleid });
            if (type == "contact")
                return RedirectToAction("Edit", "Contact", new { id = contactid });
            return RedirectToAction("Index", "Contact");
        }

        private void Validate(Shipment model)
        {
            if (model.ContactId < 1)
                ModelState.AddModelError("ContactId", "Please select a valid Contact from the auto complete choices");

            if (model.TitleId < 1)
                ModelState.AddModelError("TitleId", "Please select a valid Title from the auto complete choices");

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

        private void LoadLists(Shipment model)
        {
            var titleId = model.TitleId.ToString();
            var titleName = string.IsNullOrEmpty(model.TitleName) ? "Search for a Title" : model.TitleName;
            var contactId = model.ContactId.ToString();
            var contactName = string.IsNullOrEmpty(model.ContactName) ? "Search for a Contact" : model.ContactName;
            var contacts = new List<SelectListItem> { new SelectListItem { Text = contactName, Value = contactId } };
            var titles = new List<SelectListItem> { new SelectListItem { Text = titleName, Value = titleId } };
            model.Contacts = new SelectList(contacts, "Value", "Text", null);
            model.Titles = new SelectList(titles, "Value", "Text", null);
        }
    }
}
