using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LINQtoCSV;

namespace WavePoetry.Model
{
    public class Shipment
    {
        public Shipment()
        {
            Quantity = 1;
            var statuses = AdminLists.GetShipmentStatuses();
            StatusTypes = new SelectList(
                statuses,
                "Key",
                "Value",
                null
               );

            var types = AdminLists.GetShipmentTypes();
            Types = new SelectList(
                types,
                "Key",
                "Value",
                null
               );
        }

        public int Id { get; set; }
        [Required, DisplayName("Title")]
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        [Required, DisplayName("Contact")]
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Type { get; set; }
        [DisplayName("Date Sent")]
        public Nullable<System.DateTime> DateSent { get; set; }
        public DateTime DateCreated { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }
        [DisplayName("Follow Up?")]
        public bool ShouldFollowUp { get; set; }
        public Nullable<int> AwardId { get; set; }
        public string AwardName { get; set; }
        public string Redirect { get; set; }
        public string LastUpdated { get; set; }
        public SelectList StatusTypes { get; set; }
        public SelectList Types { get; set; }
        public DateTime TitlePubDate { get; set; }
        public string FollowUpText { get; set; }
        public SelectList Contacts { get; set; }
        public SelectList Titles { get; set; }
    }

    public class ShipmentCsvLine
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
        public string Titles { get; set; }
        [CsvColumn(FieldIndex = 13)]
        public string PrimaryEmail { get; set; }
        [CsvColumn(FieldIndex = 14)]
        public string AltEmail { get; set; }
        [CsvColumn(FieldIndex = 15)]
        public string FollowUp { get; set; }
        
        public IEnumerable<TitleToSend> TitleList { get; set; }
        public Nullable<int> SubNumber { get; set; }
        public IEnumerable<int> FollowUpShipments { get; set; }
    }

    public class ShipmentSearch
    {
        public ShipmentSearch()
        {
            var types = AdminLists.GetShipmentTypes();
            Types = new SelectList(
                types,
                "Key",
                "Value",
                null
               );

            var titles = new List<SelectListItem>();
            AvailableTitles = new MultiSelectList(
                titles,
                "Text",
                "Text",
                null
               );
        }
        public bool HideOptions { get; set; }
        public string SearchType { get; set; }
        public IEnumerable<ShipmentDetails> ShipmentResults { get; set; }
        public IEnumerable<ShipmentDetails> ContactResults { get; set; }
        [Required]
        public string SelectedType { get; set; }
        public SelectList Types { get; set; }
        public int[] SelectedTitles { get; set; }
        public string SelectedTitlesCsv { get; set; }
        public int[] TitlesToCreateShipmentsFor { get; set; }
        public MultiSelectList AvailableTitles { get; set; }
        public string Message { get; set; }
        public bool MarkAsFollowUp { get; set; }
    }

    public class ShipmentDetails
    {
        public string SortName { get; set; }
        public int ContactId { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public IEnumerable<TitleToSend> Titles { get; set; }
    }

    public class TitleToSend
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public int Quantity { get; set; }
    }
}
