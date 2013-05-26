using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavePoetry.Model;

namespace WavePoetry.DataAccess
{
    public class AdminData
    {
        wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
        /// <summary>
        /// USERS
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public user DoLogin(string username, string password)
        {
                return dbContext.users.Where(u => !u.is_deleted).FirstOrDefault(u => u.username == username && u.password == password);
        }

        public List<Model.AdminUser> GetAllUsers()
        {
                return dbContext.users.Where(u=> !u.is_deleted).Select(u => new AdminUser
                {
                    UserId = u.id,
                    IsAdmin = u.is_admin,
                    Username = u.username,
                    Password = u.password
                }).ToList();
        }

        public void DeleteUser(int id, int updatedBy)
        {
                var oldUser = dbContext.users.First(u => u.id == id);
                {
                    oldUser.is_deleted = true;
                    oldUser.updatedat = DateTime.Now;
                    oldUser.updatedby = updatedBy;
                    dbContext.SaveChanges();
                }
            
        }

        public void UpdateUser(AdminUser user, int updatedBy)
        {
                var oldUser = dbContext.users.First(u => u.id == user.UserId);
                {
                    oldUser.username = user.Username;
                    oldUser.is_admin = user.IsAdmin;
                    oldUser.password = user.Password;
                    oldUser.updatedat = DateTime.Now;
                    oldUser.updatedby = updatedBy;
                    dbContext.SaveChanges();
                }
            
        }

        public void CreateUser(AdminUser user, int createdBy)
        {
                user u = new user
                    {
                        username = user.Username,
                        is_admin = user.IsAdmin,
                        is_deleted = false,
                        password = user.Password,
                        createdat = DateTime.Now,
                        createdby = createdBy,
                        updatedat = DateTime.Now,
                        updatedby = createdBy
                    };

                dbContext.users.Add(u);
                dbContext.SaveChanges();
            
        }

        public AdminUser GetUserById(int id)
        {
                var u = dbContext.users.FirstOrDefault(t => t.id == id);
                if (u == null)
                    return null;
                return new AdminUser
                {
                    Username = u.username,
                    UserId = u.id,
                    Password = u.password,
                    IsAdmin = u.is_admin
                };
            
        }

        public user GetUserByName(string userName)
        {
                return dbContext.users.FirstOrDefault(t => t.username == userName);
            
            
        }

        /// <summary>
        /// CONTACT CATS
        /// </summary>
        public List<Model.Category> GetAllContactCats()
        {
                return dbContext.contact_categories.Select(u => new Category
                    {
                        Id = u.id,
                        Name = u.name
                    }).ToList();
            
        }

        public Category CreateContactCat(Category cat, int createdBy)
        {
                contact_categories c = new contact_categories
                    {
                        name = cat.Name
                    };

                var newCat = dbContext.contact_categories.Add(c);
                dbContext.SaveChanges();
                cat.Id = newCat.id;
                return cat;
            
        }


        public void UpdateContactCat(Category cat, int p)
        {
                var c = dbContext.contact_categories.First(x => x.id == cat.Id);
                {
                    c.name = cat.Name;
                    dbContext.SaveChanges();
                }
            
        }

        public Category GetContactCatById(int id)
        {
                var cat = dbContext.contact_categories.FirstOrDefault(c => c.id == id);
                if (cat == null)
                    return null;
                return new Category
                {
                    Id = cat.id,
                    Name = cat.name
                };
            
        }

        /// <summary>
        /// AWARD CATS
        /// </summary>
        /// <returns></returns>
        public List<Model.Category> GetAllAwardCats()
        {
                return dbContext.award_categories.Select(u => new Category
                    {
                        Id = u.id,
                        Name = u.name
                    }).ToList();
            
        }

        public void UpdateAwardCat(Category cat, int p)
        {
                var c = dbContext.award_categories.First(x => x.id == cat.Id);
                {
                    c.name = cat.Name;
                    dbContext.SaveChanges();
                }
            
        }

        public Category GetAwardCatById(int id)
        {
                var cat = dbContext.award_categories.FirstOrDefault(c => c.id == id);
                if (cat == null)
                    return null;
                return new Category
                {
                    Id = cat.id,
                    Name = cat.name
                };
            
        }

        public void CreateAwardCat(Category cat, int p)
        {
                award_categories c = new award_categories
                    {
                        name = cat.Name
                    };

                dbContext.award_categories.Add(c);
                dbContext.SaveChanges();
            
        }

        public void DeleteContactCat(int id)
        {
            var model = dbContext.contact_categories.FirstOrDefault(x => x.id == id);
            if (model == null)
                return;

            var contactsToCats = dbContext.contact_to_category.Where(x => x.contact_category_id == id).ToList();

            foreach(var c in contactsToCats)
                dbContext.contact_to_category.Remove(c);

            dbContext.contact_categories.Remove(model);

            dbContext.SaveChanges();
        }
    }
}
