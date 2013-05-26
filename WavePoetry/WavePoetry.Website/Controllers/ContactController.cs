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
            var contacts = authors.Select(t => new ContactDetails
            {
                DisplayName = t.DisplayName + " (" + t.Organization + ")",
                Id = t.Id,
                Organization = t.Organization
            });
            return Json(contacts);
        }

        [AllowAnonymous]
        public JsonResult LookupAuthor(string term)
        {
            int take = 20;
            IEnumerable<ContactDetails> authors = data.LookupContact(term, take);
            var contacts = authors.Select(t => new ContactDetails
            {
                DisplayName = t.DisplayName + " (" + t.Organization + ")",
                Id = t.Id,
                Organization = t.Organization
            });
            return Json(contacts, JsonRequestBehavior.AllowGet);
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
            if (ModelState.IsValid)
            {
                int modelId = data.Insert(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}, {1}\" was created.", model.Lastname, model.Firstname);
                return RedirectToAction("Edit", new { id = modelId });
            }

            model.LoadCats(adminData.GetAllContactCats());
            return View(model);
        }

        public ActionResult DeleteContact(int id)
        {
            data.Delete(id);
            TempData["SuccessMessage"] = "Contact was deleted.";
            return RedirectToAction("Index", "Contact");
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

            if (ModelState.IsValid)
            {
                data.Update(model, (Session["LoggedInUser"] as user).id);
                TempData["SuccessMessage"] = string.Format("\"{0}, {1}\" was updated.", model.Lastname, model.Firstname);
            }

            var saved = data.GetById(model.Id);
            saved.LoadCats(adminData.GetAllContactCats());
            return View(saved);
        }

        public ActionResult Index(ContactSearch search, string btnValue)
        {
            search.LoadCats(adminData.GetAllContactCats());
            if (btnValue == "export")
                return CreateCsv(search);

            var results = data.Search(search);
            search.Results = results;
            return View(search);
        }

        public FileResult CreateCsv(ContactSearch search)
        {
            List<ContactCsvLine> items = data.SearchCsv(search);
            
            foreach (var item in items)
                item.SubscriberNumber = item.SubNumber.HasValue ? item.SubNumber.Value.ToString() : "";

            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                EnforceCsvColumnAttribute = true
            };

            CsvContext cc = new CsvContext();
            using (MemoryStream ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    cc.Write(items, tw, outputFileDescription);
                }

                return File(ms.ToArray(), "text/csv", "ContactList.csv");
            }
        }

    }
}
