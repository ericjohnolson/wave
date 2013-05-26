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
    public class Review
    {
        public Review()
        {
            DateReviewed = DateTime.Now.Date;
        }
        public int Id { get; set; }
        [Required, DisplayName("Reviewed Date")]
        public DateTime DateReviewed { get; set; }
        public string Venue { get; set; }
        [AllowHtml]
        public string ReviewText { get; set; }
        public string OriginalLink { get; set; }
        public string CopyLink { get; set; }
        [Required, DisplayName("Reviewed By")]
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        [Required, DisplayName("Title")]
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string Redirect { get; set; }
        public DateTime TitlePubDate { get; set; }
        public SelectList Contacts { get; set; }
        public SelectList Titles { get; set; }
    }

    public class ReviewDetails
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public string Title { get; set; }
        public int ReviewerId { get; set; }
        public string Reviewer { get; set; }
        public string Venue { get; set; }
        public DateTime ReviewedDate { get; set; }
    }

    public class ReviewSearch
    {
        public ReviewSearch()
        {
        }
        public string Title { get; set; }
        public string Venue { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> End { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }
        public IEnumerable<ReviewDetails> Results { get; set; }
    }
}
