using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WavePoetry.DataAccess;
using WavePoetry.Model;

namespace WavePoetry.Website.Controllers
{
    public class AdminController : Controller
    {
        private AdminData data;

        public AdminController()
        {
            data = new AdminData();
        }
        
        /// <summary>
        /// USERS
        /// </summary>
        /// <returns></returns>
        public ActionResult ListUsers()
        {
            List<AdminUser> users = data.GetAllUsers();
            return View(users);
        }
        public ActionResult CreateUser()
        {
            return View(new AdminUser());
        }
        [HttpPost]
        public ActionResult CreateUser(AdminUser user)
        {
            if (ModelState.IsValid)
            {
                data.CreateUser(user, (Session["LoggedInUser"] as user).id);
                return RedirectToAction("ListUsers");
            }
            return View(user);
        }
        public ActionResult EditUser(int id)
        {
            var user = data.GetUserById(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(AdminUser user)
        {
            if (ModelState.IsValid)
            {
            data.UpdateUser(user, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListUsers");
            }
            return View(user);
        }

        public ActionResult DeleteUser(int id)
        {
            data.DeleteUser(id, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListUsers");
        }

        /// <summary>
        /// AWARD CATS
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAwardCats()
        {
            List<Category> cats = data.GetAllAwardCats();
            return View(cats);
        }
        public ActionResult CreateAward()
        {
            return View(new Category());
        }
        [HttpPost]
        public ActionResult CreateAward(Category cat)
        {
            if (!ModelState.IsValid)
                return View(cat);

            data.CreateAwardCat(cat, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListAwardCats");
        }
        public ActionResult EditAward(int id)
        {
            var category = data.GetAwardCatById(id);
            return View(category);
        }
        [HttpPost]
        public ActionResult EditAward(Category cat)
        {
            if (!ModelState.IsValid)
                return View(cat);
            data.UpdateAwardCat(cat, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListAwardCats");
        }

        /// <summary>
        /// CONTACT CATS
        /// </summary>
        /// <returns></returns>
        public ActionResult ListContactCats()
        {
            List<Category> cats = data.GetAllContactCats();
            return View(cats);
        }
        public ActionResult CreateContact()
        {
            return View(new Category());
        }
        [HttpPost]
        public ActionResult CreateContact(Category cat)
        {
            if (!ModelState.IsValid)
                return View(cat);
            data.CreateContactCat(cat, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListContactCats");
        }

        public ActionResult DeleteContactCategory(int id)
        {
            data.DeleteContactCat(id);
            return RedirectToAction("ListContactCats");
        }
        public ActionResult EditContact(int id)
        {
            var c = data.GetContactCatById(id);
            return View(c);
        }
        [HttpPost]
        public ActionResult EditContact(Category cat)
        {
            if (!ModelState.IsValid)
                return View(cat);
            data.UpdateContactCat(cat, (Session["LoggedInUser"] as user).id);
            return RedirectToAction("ListContactCats");
        }
    }
}
