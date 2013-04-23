using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavePoetry.Model;

namespace WavePoetry.DataAccess
{
    public class TitleData
    {
        public Title GetById(int id)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var title = dbContext.titles.FirstOrDefault(t => t.id == id);
            if (title == null)
                return null;
            Title vm = new Title
            {
                Id = title.id,
                Name = title.title1,
                Notes = title.notes,
                Author = title.author,
                Edition = title.edition,
                ISBN = title.isbn,
                PubDate = title.date_published,
                Subtitle = title.subtitle,
                AuthorName = title.author_contact.firstname + " " + title.author_contact.lastname,
                Reviews = title.title_review.Select(r => new Review 
                {
                    Id = r.id,
                    ReviewText = r.review_text.Length > 200 ? r.review_text.Substring(0, 200) + "..." : r.review_text,
                    ReviewerId = r.reviewedby,
                    ReviewerName = r.review_contact.firstname + " " + r.review_contact.lastname,
                    DateReviewed = r.date_reviewed,
                    Venue = r.venue
                }),
                Shipments = title.title_shipment.Select(s => new Shipment
                {
                    Id = s.id,
                    ContactId = s.contact_id,
                    ContactName = s.shipment_contact.firstname + " " + s.shipment_contact.lastname,
                    DateSent = s.date_sent,
                    DateCreated = s.createdat,
                    Quantity = s.quantity,
                    Status = s.status,
                    Type = s.type,
                    ShouldFollowUp = s.should_followup,
                    FollowUpText = s.should_followup ? "yes" : "no"
                }),
                LastUpdated = string.Format("{0} at {1}", title.title_user.username, title.updatedat.ToShortDateString())
            };

            vm.TotalCompPending = vm.Shipments.Where(s => s.Status == "Pending" && s.Type == "Comp").Sum(s => s.Quantity);
            vm.TotalCompSent = vm.Shipments.Where(s => s.Status == "Sent" && s.Type == "Comp").Sum(s => s.Quantity);
            vm.TotalDeskPending = vm.Shipments.Where(s => s.Status == "Pending" && s.Type == "Desk").Sum(s => s.Quantity);
            vm.TotalDeskSent = vm.Shipments.Where(s => s.Status == "Sent" && s.Type == "Desk").Sum(s => s.Quantity);
            vm.TotalDonationPending = vm.Shipments.Where(s => s.Status == "Pending" && s.Type == "Donation").Sum(s => s.Quantity);
            vm.TotalDonationSent = vm.Shipments.Where(s => s.Status == "Sent" && s.Type == "Donation").Sum(s => s.Quantity);
            vm.TotalGalleyPending = vm.Shipments.Where(s => s.Status == "Pending" && s.Type == "Galleys").Sum(s => s.Quantity);
            vm.TotalGalleySent = vm.Shipments.Where(s => s.Status == "Sent" && s.Type == "Galleys").Sum(s => s.Quantity);
            vm.TotalReviewPending = vm.Shipments.Where(s => s.Status == "Pending" && s.Type == "Review").Sum(s => s.Quantity);
            vm.TotalReviewSent = vm.Shipments.Where(s => s.Status == "Sent" && s.Type == "Review").Sum(s => s.Quantity);

            return vm;
        }

        public IEnumerable<TitleDetails> Search(TitleSearch search)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            return dbContext.titles.Where(t => t.title1.Contains(search.Title) || t.isbn == search.Isbn ||
                t.author_contact.lastname.Contains(search.AuthorLastName) || t.author_contact.firstname.Contains(search.AuthorFirstName))
                .Select(title => new TitleDetails
                {
                    Author = title.author_contact.lastname + ", " + title.author_contact.firstname,
                    PubDate = title.date_published,
                    Title = title.title1,
                    TitleId = title.id
                });
        }

        public void Update(Title title, int updatedby)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var existingTitle = dbContext.titles.First(t => t.id == title.Id);
            {
                existingTitle.isbn = title.ISBN;
                existingTitle.notes = title.Notes;
                existingTitle.subtitle = title.Subtitle;
                existingTitle.title1 = title.Name;
                existingTitle.edition = title.Edition;
                existingTitle.date_published = title.PubDate;
                existingTitle.author = title.Author;
                existingTitle.updatedat = DateTime.Now;
                existingTitle.updatedby = updatedby;
                dbContext.SaveChanges();
            }
        }

        public void Insert(Title title, int createdBy)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1(); 
            title newTitle = new title
            {
                title1 = title.Name,
                author = title.Author,
                createdat = DateTime.Now,
                createdby = createdBy,
                updatedat = DateTime.Now,
                updatedby = createdBy,
                date_published = title.PubDate,
                edition = title.Edition,
                isbn = title.ISBN,
                notes = title.Notes,
                subtitle = title.Subtitle
            };

            dbContext.titles.Add(newTitle);
            dbContext.SaveChanges();
        }

        public IEnumerable<TitleDetails> LookupTitle(string searchText, int take)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var result = dbContext.titles.Where(c => c.title1.Contains(searchText))
                .Select(t => new TitleDetails
                {
                    Title = t.title1,
                    TitleId = t.id,
                    PubDate = t.date_published
                });
            
            return result; 
        }
    }
}
