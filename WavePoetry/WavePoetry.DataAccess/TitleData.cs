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
        wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
        public Title GetById(int id)
        {
            Dictionary<int, string> contactDictionary = new Dictionary<int, string>();
            foreach (var c in dbContext.contacts.Select(x => new { Id = x.id, Name = x.firstname + " " + x.lastname }).ToList())
                contactDictionary.Add(c.Id, c.Name);

            Title vm = new Title();
            var title = dbContext.titles.FirstOrDefault(t => t.id == id);
            if (title == null)
                return null;
            vm = new Title
            {
                Id = title.id,
                Name = title.title1,
                Notes = title.notes,
                Author = title.author,
                Edition = title.edition,
                ISBN = title.isbn,
                PubDate = title.date_published,
                Subtitle = title.subtitle,
                AuthorName = title.author > 0 ? title.author_contact.firstname + " " + title.author_contact.lastname : string.Empty,
                Reviews = title.title_review.Select(r => new Review
                {
                    Id = r.id,
                    ReviewText = r.review_text != null && r.review_text.Length > 200 ? r.review_text.Substring(0, 200) + "..." : r.review_text,
                    ReviewerId = r.reviewedby,
                    ReviewerName = r.reviewedby > 0 ? r.review_contact.firstname + " " + r.review_contact.lastname : string.Empty,
                    DateReviewed = r.date_reviewed,
                    Venue = r.venue
                }).ToList(),
                Shipments = title.title_shipment.Select(s => new Shipment
                {
                    Id = s.id,
                    ContactId = s.contact_id,
                    DateSent = s.date_sent,
                    DateCreated = s.createdat,
                    Quantity = s.quantity,
                    Status = s.status,
                    Type = s.type,
                    ShouldFollowUp = s.should_followup,
                    FollowUpText = s.should_followup ? "yes" : "no"
                }).ToList(),
                LastUpdated = string.Format("{0} at {1}", title.title_user.username, title.updatedat.ToShortDateString())
            };


            foreach (var s in vm.Shipments)
            {
                s.ContactName = contactDictionary[s.ContactId];
                if (s.Status == "Pending" && s.Type == "Comp")
                    vm.TotalCompPending += s.Quantity;
                if (s.Status == "Sent" && s.Type == "Comp")
                    vm.TotalCompSent += s.Quantity;
                if (s.Status == "Pending" && s.Type == "Desk")
                    vm.TotalDeskPending += s.Quantity;
                if (s.Status == "Sent" && s.Type == "Desk")
                    vm.TotalDeskSent += s.Quantity;
                if (s.Status == "Pending" && s.Type == "Donation")
                    vm.TotalDonationPending += s.Quantity;
                if (s.Status == "Sent" && s.Type == "Donation")
                    vm.TotalDonationSent += s.Quantity;
                if (s.Status == "Pending" && s.Type == "Galleys")
                    vm.TotalGalleyPending += s.Quantity;
                if (s.Status == "Sent" && s.Type == "Galleys")
                    vm.TotalGalleySent += s.Quantity;
                if (s.Status == "Pending" && s.Type == "Review")
                    vm.TotalReviewPending += s.Quantity;
                if (s.Status == "Sent" && s.Type == "Review")
                    vm.TotalReviewSent += s.Quantity;
            }
            return vm;
        }

        public IEnumerable<TitleDetails> Search(TitleSearch search)
        {
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

        public Title Insert(Title title, int createdBy)
        {
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

            var t = dbContext.titles.Add(newTitle);
            var validationErrors = dbContext.GetValidationErrors().ToList();
            string error = string.Empty;
            if (validationErrors.Count() > 0)
                error = "errors";
            dbContext.SaveChanges();
            title.Id = t.id;
            return title;

        }

        public IEnumerable<TitleDetails> LookupTitle(string searchText, int take)
        {
            var result = dbContext.titles.Where(c => c.title1.Contains(searchText))
               .Select(t => new TitleDetails
               {
                   Title = t.title1,
                   TitleId = t.id,
                   PubDate = t.date_published
               });

            return result;

        }

        public void Delete(int id)
        {
            var model = dbContext.titles.FirstOrDefault(x => x.id == id);
            if (model == null)
                return;

            foreach (var ship in dbContext.shipments.Where(s => s.title_id == id).ToList())
                dbContext.shipments.Remove(ship);
            foreach (var review in dbContext.reviews.Where(r => r.title_id == id).ToList())
                dbContext.reviews.Remove(review);

            dbContext.titles.Remove(model);

            dbContext.SaveChanges();
        }

        public List<ContactCsvLine> GetCsv(List<int> ids)
        {
            if (ids.Count == 0)
                return new List<ContactCsvLine>();

            return dbContext.shipments.Where(x => ids.Contains(x.id))
               .Select(s => new ContactCsvLine
               {
                   FirstName = s.shipment_contact.firstname,
                   LastName = s.shipment_contact.lastname,
                   AddressLine1 = s.shipment_contact.is_primary ? s.shipment_contact.addressline1 : s.shipment_contact.addressline1_alt,
                   AddressLine2 = s.shipment_contact.is_primary ? s.shipment_contact.addressline2 : s.shipment_contact.addressline2_alt,
                   State = s.shipment_contact.is_primary ? s.shipment_contact.state : s.shipment_contact.state_alt,
                   Zip = s.shipment_contact.is_primary ? s.shipment_contact.zip : s.shipment_contact.zip_alt,
                   Country = s.shipment_contact.is_primary ? s.shipment_contact.country : s.shipment_contact.country_alt,
                   Title = s.shipment_contact.is_primary ? s.shipment_contact.title : s.shipment_contact.title_alt,
                   City = s.shipment_contact.is_primary ? s.shipment_contact.city : s.shipment_contact.city_alt,
                   Organization = s.shipment_contact.is_primary ? s.shipment_contact.organization : s.shipment_contact.organization_alt,
                   SubNumber = s.shipment_contact.is_subscriber ? s.shipment_contact.sub_number : null,
                   PrimaryEmail = s.shipment_contact.email1,
                   AltEmail = s.shipment_contact.email2,
                   FollowUpTitleList = s.shipment_contact.contact_shipment.Where(cs => cs.should_followup).Select(cs2 => cs2.shipment_title.title1)
               }).ToList();
        }

        public int SetFollowUp(List<int> ids, bool shouldFollowUp, int updatedById)
        {
            int updatedCount = 0;
            var ships = dbContext.shipments.Where(s => ids.Contains(s.id));

            foreach (shipment s in ships)
            {
                s.should_followup = shouldFollowUp;
                s.updatedat = DateTime.Now;
                s.updatedby = updatedById;
                updatedCount++;
            }
            dbContext.SaveChanges();
            return updatedCount;
        }
    }
}
