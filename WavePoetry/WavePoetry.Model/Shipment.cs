using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WavePoetry.Model
{
    public class Shipment
    {
        public Shipment()
        {
            Quantity = 1;
            var statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pending", Value = "Pending" },
                new SelectListItem { Text = "Sent", Value = "Sent" }
            };
            StatusTypes = new SelectList(
                statuses,
                "Text",
                "Text",
                null
               );

            var types = new List<SelectListItem>
            {
                new SelectListItem { Text = "Galleys", Value = "Galleys" },
                new SelectListItem { Text = "Review", Value = "Review" },
                new SelectListItem { Text = "Desk", Value = "Desk" },
                new SelectListItem { Text = "Comp", Value = "Comp" },
                new SelectListItem { Text = "Donation", Value = "Donation" }
            };
            Types = new SelectList(
                types,
                "Text",
                "Text",
                null
               );
        }
        public int Id { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int TitleId { get; set; }
        [Required, DisplayName("Title")]
        public string TitleName { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int ContactId { get; set; }
        [Required, DisplayName("Contact")]
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

    }


    public class ShipmentCsvLine
    {

    }

    public class ShipmentSearch
    {
        public ShipmentSearch()
        {
            var types = new List<SelectListItem>
            {
                new SelectListItem { Text = "Galleys", Value = "Galleys" },
                new SelectListItem { Text = "Review", Value = "Review" },
                new SelectListItem { Text = "Desk", Value = "Desk" },
                new SelectListItem { Text = "Comp", Value = "Comp" },
                new SelectListItem { Text = "Donation", Value = "Donation" }
            };
            Types = new SelectList(
                types,
                "Text",
                "Text",
                null
               );

            var titles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Type to find", Value = "Type to find" }
            };
            AvailableTitles = new MultiSelectList(
                titles,
                "Text",
                "Text",
                null
               );
        }
        public string SearchType { get; set; }
        public IEnumerable<ShipmentDetails> ResultsToMarkSent { get; set; }
        public IEnumerable<ShipmentDetails> ResultsToMarkPending { get; set; }
        public string SelectedType { get; set; }
        public SelectList Types { get; set; }
        public int[] SelectedTitles { get; set; }
        public MultiSelectList AvailableTitles { get; set; }
        public string Message { get; set; }
    }

    public class ShipmentDetails
    {
        public string SortName { get; set; }
        public int ContactId { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public List<TitleToSend> Titles { get; set; }
    }

    public class TitleToSend
    {
        public string TitleName { get; set; }
        public int Quantity { get; set; }
    }
}
