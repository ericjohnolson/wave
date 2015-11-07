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
    public class Title
    {
        public Title()
        {
            var eds = new List<SelectListItem>
            {
                new SelectListItem { Text = "Softcover", Value = "Softcover" },
                new SelectListItem { Text = "Hardcover", Value = "Hardcover" }
            };
            Editions = new SelectList(
                eds,
                "Text",
                "Text",
                null
               );
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Subtitle { get; set; }
        [Required, DisplayName("Author")]
        public int Author { get; set; }
        public string AuthorName { get; set; }
        [Required]
        public string Edition { get; set; }
        public string ISBN { get; set; }
        [Required, DisplayName("Published Date")]
        public DateTime PubDate { get; set; }
        public string Notes { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Shipment> Shipments { get; set; }
        public List<Contact> Contacts { get; set; }
        public SelectList Editions { get; set; }
        public string LastUpdated { get; set; }
        public SelectList ContactSelects { get; set; }

        // Shipment Summary
        public int TotalPending { get; set; }
        public int TotalSent { get; set; }
        public int TotalGalleyPending { get; set; }
        public int TotalGalleySent { get; set; }
        public int TotalReviewPending { get; set; }
        public int TotalReviewSent { get; set; }
        public int TotalDeskPending { get; set; }
        public int TotalDeskSent { get; set; }
        public int TotalCompPending { get; set; }
        public int TotalCompSent { get; set; }
        public int TotalDonationPending { get; set; }
        public int TotalDonationSent { get; set; }
        public int TotalAwardPending { get; set; }
        public int TotalAwardSent { get; set; }

        // Exporting Shipments
        public string ExportIds { get; set; }
        public bool IsExporting { get; set; }
    }

    public class TitleDetails
    {
        public int TitleId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PubDate { get; set; }
    }

    public class TitleSearch
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public IEnumerable<TitleDetails> Results { get; set; }
    }

}
