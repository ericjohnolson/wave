using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WavePoetry.Model
{
    public class Contact
    {
        public Contact()
        {
            Is_email1_primary = true;
            Is_primary = true;
            var subtypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Softcover", Value = "Softcover" },
                new SelectListItem { Text = "Hardcover", Value = "Hardcover" }
            };
            SubTypes = new SelectList(
                subtypes,
                "Text",
                "Text",
                null
               );
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Website { get; set; }
        [DisplayName("Primary Email")]
        public string Email1 { get; set; }
        public bool Is_email1_primary { get; set; }
        [DisplayName("Alt. Email")]
        public string Email2 { get; set; }
        public bool Is_email2_primary { get; set; }
        public string Notes { get; set; }
        [DisplayName("Is Subscriber")]
        public bool Is_subscriber { get; set; }
        [DisplayName("Subscription Start Date")]
        public Nullable<DateTime> Sub_startdate { get; set; }
        [DisplayName("Subscription End Date")]
        public Nullable<DateTime> Sub_enddate { get; set; }
        [DisplayName("Subscription #")]
        public Nullable<int> Sub_number { get; set; }
        public bool Galley_all { get; set; }
        public bool Galley_pos { get; set; }
        [DisplayName("Galley Copies")]
        public Nullable<int> Galley_copies { get; set; }
        public bool Review_all { get; set; }
        public bool Review_pos { get; set; }
        [DisplayName("Review Copies")]
        public Nullable<int> Review_copies { get; set; }
        public bool Desk_all { get; set; }
        public bool Desk_pos { get; set; }
        [DisplayName("Desk Copies")]
        public Nullable<int> Desk_copies { get; set; }
        public bool Comp_all { get; set; }
        public bool Comp_pos { get; set; }
        [DisplayName("Comp Copies")]
        public Nullable<int> Comp_copies { get; set; }
        [DisplayName("Address Line 1")]
        public string Addressline1 { get; set; }
        [DisplayName("Address Line 2")]
        public string Addressline2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public bool Is_primary { get; set; }
        public bool Is_bad { get; set; }
        [DisplayName("Address Line 1")]
        public string Addressline1_alt { get; set; }
        [DisplayName("Address Line 2")]
        public string Addressline2_alt { get; set; }
        [DisplayName("City")]
        public string City_alt { get; set; }
        [DisplayName("State")]
        public string State_alt { get; set; }
        [DisplayName("Zip")]
        public string Zip_alt { get; set; }
        [DisplayName("Country")]
        public string Country_alt { get; set; }
        [DisplayName("Organization")]
        public string Organization_alt { get; set; }
        [DisplayName("Title")]
        public string Title_alt { get; set; }
        public bool Is_primary_alt { get; set; }
        public bool Is_bad_alt { get; set; }
        [DisplayName("Subscription Type")]
        public string Sub_type { get; set; }
        public string LastUpdated { get; set; }
        public SelectList SubTypes { get; set; }
        public MultiSelectList Cats { get; set; }
        public int[] SelectedCats { get; set; }
        public IEnumerable<Shipment> Shipments { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        public void LoadCats(List<Category> list)
        {
            Cats = new MultiSelectList(
                list,
                "Id",
                "Name",
                SelectedCats
            );
        }
    }

    public class ContactDetails
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
    }

    public class ContactSearch
    {
        public ContactSearch()
        {
            SelectedCats = new List<int>().ToArray();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        [DisplayName("Follow Up")]
        public bool NeedsFollowUp { get; set; }
        public MultiSelectList Cats { get; set; }
        public int[] SelectedCats { get; set; }
        public IEnumerable<ContactDetails> Results { get; set; }

        public void LoadCats(List<Category> list)
        {
            Cats = new MultiSelectList(
                list,
                "Id",
                "Name",
                SelectedCats
            );
        }
    }
}
