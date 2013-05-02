using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavePoetry.Model;

namespace WavePoetry.DataAccess
{
    public class ContactData
    {
        public IEnumerable<ContactDetails> LookupContact(string searchText, int take)
        {
            string firstname = searchText;
            string lastname = searchText;
            if (searchText.Contains(' '))
            {
                var names = searchText.Split(' ');
                firstname = names[0];
                lastname = names[1];
            }

            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            return dbContext.contacts.Where(c => c.firstname.StartsWith(firstname) || c.lastname.StartsWith(lastname))
                .Select(c2 => new ContactDetails
                {
                    DisplayName = c2.firstname + " " + c2.lastname,
                    Id = c2.id,
                    Organization = c2.is_primary ? c2.organization : c2.organization_alt
                });
        }

        public Contact GetById(int id)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var c = dbContext.contacts.FirstOrDefault(t => t.id == id);
            if (c == null)
                return null;
            var cats = new List<int>().ToArray();
            if (c.contact_contact_category.Count() > 0)
                cats = c.contact_contact_category.Select(cat => cat.contact_category_id).ToArray();

            Contact vm = new Contact
            {
                Id = c.id,
                DisplayName = c.firstname + " " + c.lastname + " (" + (c.is_primary ? c.organization : c.organization_alt) + ")",
                Email1 = c.email1,
                Email2 = c.email2,
                Is_email1_primary = c.is_email1_primary,
                Addressline1 = c.addressline1,
                Addressline1_alt = c.addressline1_alt,
                Addressline2 = c.addressline2,
                Addressline2_alt = c.addressline2_alt,
                City = c.city,
                City_alt = c.city_alt,
                Comp_all = c.comp_all,
                Comp_copies = c.comp_copies,
                Comp_pos = c.comp_pos,
                Country = c.country,
                Country_alt = c.country_alt,
                Desk_all = c.desk_all,
                Desk_copies = c.desk_copies,
                Desk_pos = c.desk_pos,
                Firstname = c.firstname,
                Galley_all = c.galley_all,
                Galley_copies = c.galley_copies,
                Galley_pos = c.galley_pos,
                Is_bad_alt = c.is_bad_alt,
                Is_bad = c.is_bad,
                Is_email2_primary = c.is_email2_primary,
                Review_copies = c.review_copies,
                Review_all = c.review_all,
                Review_pos = c.review_pos,
                Is_primary = c.is_primary,
                Is_primary_alt = c.is_primary_alt,
                Is_subscriber = c.is_subscriber,
                Lastname = c.lastname,
                LastUpdated = string.Format("{0} at {1}", c.contact_user.username, c.updatedat.ToShortDateString()),
                Notes = c.notes,
                Phone = c.phone,
                Organization = c.organization,
                Organization_alt = c.organization_alt,
                State = c.state,
                State_alt = c.state_alt,
                Sub_enddate = c.sub_enddate,
                Sub_number = c.sub_number,
                Sub_startdate = c.sub_startdate,
                Sub_type = c.sub_type,
                Title = c.title,
                Title_alt = c.title_alt,
                Website = c.website,
                Zip = c.zip,
                Zip_alt = c.zip_alt,
                SelectedCats = cats,
                Shipments = c.contact_shipment.Select(s => new Shipment
                {
                    Id = s.id,
                    TitleId = s.title_id,
                    TitleName = s.shipment_title.title1 + " (" + s.shipment_title.date_published.ToShortDateString() + ")",
                    DateSent = s.date_sent,
                    DateCreated = s.createdat,
                    Quantity = s.quantity,
                    Status = s.status,
                    Type = s.type,
                    ShouldFollowUp = s.should_followup,
                    FollowUpText = s.should_followup ? "yes" : "no"
                }),
                Reviews = c.contact_review.Select(r => new Review
                {
                    Id = r.id,
                    ReviewText = r.review_text.Length > 200 ? r.review_text.Substring(0, 200) + "..." : r.review_text,
                    TitleId = r.title_id,
                    TitleName = r.review_title.title1 + " (" + r.review_title.date_published.ToShortDateString() + ")",
                    DateReviewed = r.date_reviewed,
                    Venue = r.venue
                }),
                
            };

            return vm;
        }

        public IEnumerable<ContactDetails> Search(ContactSearch search)
        {
            if (string.IsNullOrEmpty(search.LastName) && string.IsNullOrEmpty(search.FirstName) && string.IsNullOrEmpty(search.Organization) && string.IsNullOrEmpty(search.City) &&
                search.SelectedCats.Count() == 0 && !search.NeedsFollowUp && string.IsNullOrEmpty(search.TypeAlways) && string.IsNullOrEmpty(search.TypePossible))
                return new List<ContactDetails>();

            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            return dbContext.contacts.Where(x => 
                // OR Search
                ((string.IsNullOrEmpty(search.LastName) && string.IsNullOrEmpty(search.FirstName) && string.IsNullOrEmpty(search.Organization) && string.IsNullOrEmpty(search.City)) ||
                 x.lastname.StartsWith(search.LastName) || x.firstname.StartsWith(search.FirstName) || x.organization.StartsWith(search.Organization) || x.organization_alt.StartsWith(search.Organization) || x.city.StartsWith(search.City) || x.city_alt.StartsWith(search.City)) 
                 // Cat search
                 && (search.SelectedCats.Count() == 0 || x.contact_contact_category.Where(c => search.SelectedCats.Contains(c.contact_category_id)).Count() > 0) 
                 // Follow up search
                 && (!search.NeedsFollowUp || x.contact_shipment.Where(s => s.should_followup).Count() > 0)
                 // Always Search
                 && (string.IsNullOrEmpty(search.TypeAlways) || ((search.TypeAlways != "Galleys" || x.galley_all) &&
                    (search.TypeAlways != "Review" || x.review_all) &&
                    (search.TypeAlways != "Desk" || x.desk_all) &&
                    (search.TypeAlways != "Comp" || x.comp_all)))
                 // Possible Search
                 && (string.IsNullOrEmpty(search.TypePossible) || ((search.TypePossible != "Galleys" || x.galley_pos) &&
                    (search.TypePossible != "Review" || x.review_pos) &&
                    (search.TypePossible != "Desk" || x.desk_pos) &&
                    (search.TypePossible != "Comp" || x.comp_pos)))
                )
                .Select(s => new ContactDetails
                {
                    City = s.is_primary ? s.city : s.city_alt,
                    DisplayName = s.lastname + ", " + s.firstname,
                    Organization = s.is_primary ? s.organization : s.organization_alt,
                    Id = s.id
                });
        }

        public IEnumerable<ContactDetails> Search(string city, string first, string last,  string org)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            return dbContext.contacts.Where(x =>
                 x.lastname ==last && x.firstname == first && x.city == city &&
                 x.organization == org
                )
                .Select(s => new ContactDetails
                {
                    Id = s.id
                });
        }

        public List<ContactCsvLine> SearchCsv(ContactSearch search)
        {
            if (string.IsNullOrEmpty(search.LastName) && string.IsNullOrEmpty(search.FirstName) && string.IsNullOrEmpty(search.Organization) && string.IsNullOrEmpty(search.City) &&
                search.SelectedCats.Count() == 0 && !search.NeedsFollowUp && string.IsNullOrEmpty(search.TypeAlways) && string.IsNullOrEmpty(search.TypePossible))
                return new List<ContactCsvLine>();

            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            return dbContext.contacts.Where(x =>
                // OR Search
                ((string.IsNullOrEmpty(search.LastName) && string.IsNullOrEmpty(search.FirstName) && string.IsNullOrEmpty(search.Organization) && string.IsNullOrEmpty(search.City)) ||
                 x.lastname.StartsWith(search.LastName) || x.firstname.StartsWith(search.FirstName) || x.organization.StartsWith(search.Organization) || x.organization_alt.StartsWith(search.Organization) || x.city.StartsWith(search.City) || x.city_alt.StartsWith(search.City))
                    // Cat search
                 && (search.SelectedCats.Count() == 0 || x.contact_contact_category.Where(c => search.SelectedCats.Contains(c.contact_category_id)).Count() > 0)
                    // Follow up search
                 && (!search.NeedsFollowUp || x.contact_shipment.Where(s => s.should_followup).Count() > 0)
                    // Always Search
                 && (string.IsNullOrEmpty(search.TypeAlways) || ((search.TypeAlways != "Galleys" || x.galley_all) &&
                    (search.TypeAlways != "Review" || x.review_all) &&
                    (search.TypeAlways != "Desk" || x.desk_all) &&
                    (search.TypeAlways != "Comp" || x.comp_all)))
                    // Possible Search
                 && (string.IsNullOrEmpty(search.TypePossible) || ((search.TypePossible != "Galleys" || x.galley_pos) &&
                    (search.TypePossible != "Review" || x.review_pos) &&
                    (search.TypePossible != "Desk" || x.desk_pos) &&
                    (search.TypePossible != "Comp" || x.comp_pos)))
                )
                .Select(c => new ContactCsvLine
                {
                    FirstName = c.firstname,
                    LastName = c.lastname,
                    AddressLine1 = c.is_primary ? c.addressline1 : c.addressline1_alt,
                    AddressLine2 = c.is_primary ? c.addressline2 : c.addressline2_alt,
                    State = c.is_primary ? c.state : c.state_alt,
                    Zip = c.is_primary ? c.zip : c.zip_alt,
                    Country = c.is_primary ? c.country : c.country_alt,
                    Title = c.is_primary ? c.title : c.title_alt,
                    City = c.is_primary ? c.city : c.city_alt,
                    Organization = c.is_primary ? c.organization : c.organization_alt,
                    SubNumber = c.is_subscriber ? c.sub_number : null
                }).ToList();
        }

        public void Update(Contact c2, int updatedby)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var c = dbContext.contacts.First(t => t.id == c2.Id);
            {
                c.firstname = c2.Firstname;
                c.lastname = c2.Lastname;
                c.website = c2.Website;
                c.email1 = c2.Email1;
                c.is_email1_primary = c2.Is_email1_primary;
                c.email2 = c2.Email2;
                c.notes = c2.Notes;
                c.phone = c2.Phone;
                c.is_subscriber = c2.Is_subscriber;
                c.sub_startdate = c2.Sub_startdate;
                c.sub_enddate = c2.Sub_enddate;
                c.sub_number = c2.Sub_number;
                c.galley_all = c2.Galley_all;
                c.galley_pos = c2.Galley_pos;
                c.galley_copies = c2.Galley_copies;
                c.review_all = c2.Review_all;
                c.review_pos = c2.Review_pos;
                c.review_copies = c2.Review_copies;
                c.desk_all = c2.Desk_all;
                c.desk_pos = c2.Desk_pos;
                c.desk_copies = c2.Desk_copies;
                c.comp_all = c2.Comp_all;
                c.comp_pos = c2.Comp_pos;
                c.comp_copies = c2.Comp_copies;
                c.addressline1 = c2.Addressline1;
                c.addressline2 = c2.Addressline2;
                c.city = c2.City;
                c.state = c2.State;
                c.zip = c2.Zip;
                c.country = c2.Country;
                c.title = c2.Title;
                c.organization = c2.Organization;
                c.is_primary = c2.Is_primary;
                c.is_bad = c2.Is_bad;
                c.addressline1_alt = c2.Addressline1_alt;
                c.addressline2_alt = c2.Addressline2_alt;
                c.city_alt = c2.City_alt;
                c.state_alt = c2.State_alt;
                c.zip_alt = c2.Zip_alt;
                c.country_alt = c2.Country_alt;
                c.organization_alt = c2.Organization_alt;
                c.title_alt = c2.Title_alt;
                c.is_bad_alt = c2.Is_bad_alt;
                c.sub_type = c2.Sub_type;
                c.updatedby = updatedby;
                c.updatedat = DateTime.Now;
                dbContext.SaveChanges();
            }

            InsertCategories(c2, dbContext);
        }


        public int Insert(Contact c2, int createdBy)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            contact newContact = new contact
            {
                firstname = c2.Firstname,
                lastname = c2.Lastname,
                website = c2.Website,
                email1 = c2.Email1,
                is_email1_primary = c2.Is_email1_primary,
                email2 = c2.Email2,
                notes = c2.Notes,
                phone = c2.Phone,
                is_subscriber = c2.Is_subscriber,
                sub_startdate = c2.Sub_startdate,
                sub_enddate = c2.Sub_enddate,
                sub_number = c2.Sub_number,
                galley_all = c2.Galley_all,
                galley_pos = c2.Galley_pos,
                galley_copies = c2.Galley_copies,
                review_all = c2.Review_all,
                review_pos = c2.Review_pos,
                review_copies = c2.Review_copies,
                desk_all = c2.Desk_all,
                desk_pos = c2.Desk_pos,
                desk_copies = c2.Desk_copies,
                comp_all = c2.Comp_all,
                comp_pos = c2.Comp_pos,
                comp_copies = c2.Comp_copies,
                addressline1 = c2.Addressline1,
                addressline2 = c2.Addressline2,
                city = c2.City,
                state = c2.State,
                zip = c2.Zip,
                country = c2.Country,
                title = c2.Title,
                organization = c2.Organization,
                is_primary = c2.Is_primary,
                is_bad = c2.Is_bad,
                addressline1_alt = c2.Addressline1_alt,
                addressline2_alt = c2.Addressline2_alt,
                city_alt = c2.City_alt,
                state_alt = c2.State_alt,
                zip_alt = c2.Zip_alt,
                country_alt = c2.Country_alt,
                organization_alt = c2.Organization_alt,
                title_alt = c2.Title_alt,
                is_bad_alt = c2.Is_bad_alt,
                sub_type = c2.Sub_type,
                createdat = DateTime.Now,
                createdby = createdBy,
                updatedat = DateTime.Now,
                updatedby = createdBy
            };

            var savedContact = dbContext.contacts.Add(newContact);
            dbContext.SaveChanges();
            c2.Id = savedContact.id;
            InsertCategories(c2, dbContext);
            return c2.Id;
        }

        private void InsertCategories(Contact contact, wavepoetry2Entities1 dbContext)
        {
            var existingCats = dbContext.contact_to_category.Where(c => c.contact_id == contact.Id);
            foreach (var cat in existingCats)
                dbContext.contact_to_category.Remove(cat);



            foreach (var id in contact.SelectedCats)
                dbContext.contact_to_category.Add(new contact_to_category { contact_id = contact.Id, contact_category_id = id });
            dbContext.SaveChanges();

        }
    }
}
