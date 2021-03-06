﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LINQtoCSV;

namespace WavePoetry.Model
{
    public class Contact
    {
        public Contact()
        {
            SelectedCats = new List<int>().ToArray();
            Is_email1_primary = true;
            Is_primary = true;
            Galley_copies = 1;
            Desk_copies = 1;
            Comp_copies = 1;
            Review_copies = 1;
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
        public string Phone { get; set; }
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
            var types = AdminLists.GetShipmentTypes();
            types.Remove(new KeyValuePair<string, string>("Donation", "Donation"));
            types.Insert(0, new KeyValuePair<string,string>("", "Any"));
            Types = new SelectList(
                types,
                "Key",
                "Value",
                null
               );
            SelectedCats = new List<int>().ToArray();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Notes { get; set; }
        [DisplayName("Follow Up")]
        public bool NeedsFollowUp { get; set; }
        public string TypePossible { get; set; }
        public string TypeAlways { get; set; }
        public MultiSelectList Cats { get; set; }
        public int[] SelectedCats { get; set; }
        public SelectList Types { get; set; }
        public Nullable<DateTime> Sub_enddate { get; set; }
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

        public bool HasCriteria()
        {
            return !(string.IsNullOrEmpty(this.LastName) && string.IsNullOrEmpty(this.FirstName) && string.IsNullOrEmpty(this.Organization) && string.IsNullOrEmpty(this.City) &&
                this.SelectedCats.Count() == 0 && !this.NeedsFollowUp && string.IsNullOrEmpty(this.TypeAlways) && string.IsNullOrEmpty(this.TypePossible) &&
                string.IsNullOrEmpty(this.Zip) && string.IsNullOrEmpty(this.Address) && string.IsNullOrEmpty(this.Notes) && !this.Sub_enddate.HasValue);
        }
    }

    public class ContactCsvLine
    {
        [CsvColumn(FieldIndex = 1)]
        public string FirstName { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public string LastName { get; set; }
        [CsvColumn(FieldIndex = 3)]
        public string Organization { get; set; }
        [CsvColumn(FieldIndex = 4)]
        public string Title { get; set; }
        [CsvColumn(FieldIndex = 5)]
        public string AddressLine1 { get; set; }
        [CsvColumn(FieldIndex = 6)]
        public string AddressLine2 { get; set; }
        [CsvColumn(FieldIndex = 7)]
        public string City { get; set; }
        [CsvColumn(FieldIndex = 8)]
        public string State { get; set; }
        [CsvColumn(FieldIndex = 9)]
        public string Zip { get; set; }
        [CsvColumn(FieldIndex = 10)]
        public string Country { get; set; }
        [CsvColumn(FieldIndex = 11)]
        public string SubscriberNumber { get; set; }
        [CsvColumn(FieldIndex = 12)]
        public string PrimaryEmail { get; set; }
        [CsvColumn(FieldIndex = 13)]
        public string AltEmail { get; set; }
        [CsvColumn(FieldIndex = 14)]
        public string TitlesForFollowUp { get; set; }

        public Nullable<int> SubNumber { get; set; }
        public IEnumerable<string> FollowUpTitleList { get; set; }
        public int Id { get; set; }
    }
}
