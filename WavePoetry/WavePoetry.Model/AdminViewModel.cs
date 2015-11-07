using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WavePoetry.Model
{
    public class AdminUser
    {
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public static class AdminLists
    { 
        public static List<KeyValuePair<string, string>> GetShipmentTypes()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Galleys", "Galleys"),
                new KeyValuePair<string, string>("Review", "Review"),
                new KeyValuePair<string, string>("Desk", "Desk"),
                new KeyValuePair<string, string>("Comp", "Comp"),
                new KeyValuePair<string, string>("Donation", "Donation"),
                new KeyValuePair<string, string>("Award", "Award")
            };
        }

        public static List<KeyValuePair<string, string>> GetShipmentStatuses()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Pending", "Pending"),
                new KeyValuePair<string, string>("Sent", "Sent")
            };
        }
        
    }

}
