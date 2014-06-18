using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavePoetry.Model;

namespace WavePoetry.DataAccess
{
    public class ShipmentData
    {
        wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
        /// <summary>
        /// GET CONTACTS MARK PENDING 
        /// </summary>
        public IEnumerable<ShipmentDetails> SearchForContacts(ShipmentSearch search)
        {
            return dbContext.contacts.Where(c =>
                   (search.SelectedType != "Galleys" || c.galley_all) &&
                   (search.SelectedType != "Review" || c.review_all) &&
                   (search.SelectedType != "Desk" || c.desk_all) &&
                   (search.SelectedType != "Comp" || c.comp_all)
               )
               .Select(c => new ShipmentDetails
               {
                   SortName = c.lastname + ", " + c.firstname,
                   City = c.is_primary ? c.city : c.city_alt,
                   ContactId = c.id,
                   Organization = c.is_primary ? c.organization : c.organization_alt
               });

        }

        public int CreatePendingShipments(ShipmentSearch search, int userid)
        {
            int shipments = 0;

            var contacts = dbContext.contacts.Where(c =>
                   (search.SelectedType != "Galleys" || c.galley_all) &&
                   (search.SelectedType != "Review" || c.review_all) &&
                   (search.SelectedType != "Desk" || c.desk_all) &&
                   (search.SelectedType != "Comp" || c.comp_all) &&
                   c.contact_shipment.Where(s => search.TitlesToCreateShipmentsFor.Contains(s.title_id) &&
                       s.type == search.SelectedType && s.status == "Pending").FirstOrDefault() == null
               );

            if (contacts == null || contacts.Count() == 0)
                return shipments;

            foreach (int titleId in search.TitlesToCreateShipmentsFor)
            {
                foreach (contact c in contacts)
                {
                    shipment ship = new shipment
                    {
                        contact_id = c.id,
                        title_id = titleId,
                        quantity = GetQuantityForSearchType(c, search),
                        status = "Pending",
                        type = search.SelectedType,
                        createdat = DateTime.Now,
                        createdby = userid,
                        updatedat = DateTime.Now,
                        updatedby = userid
                    };
                    dbContext.shipments.Add(ship);
                    shipments++;
                }
            }
            dbContext.SaveChanges();
            return shipments;

        }

        /// <summary>
        /// GET SHIPMENTS MARK SENT
        /// </summary>
        public IEnumerable<ShipmentDetails> SearchForShipments(ShipmentSearch search)
        {
            if (search.SelectedTitles.Count() == 0)
                return new List<ShipmentDetails>();

            return dbContext.contacts.Where(c => c.contact_shipment.Where(
               s => search.SelectedTitles.Contains(s.title_id) && s.status == "Pending" && s.type == search.SelectedType).Count() > 0)
               .Select(c => new ShipmentDetails
               {
                   SortName = c.lastname + ", " + c.firstname,
                   City = c.is_primary ? c.city : c.city_alt,
                   ContactId = c.id,
                   Organization = c.is_primary ? c.organization : c.organization_alt,
                   Titles = c.contact_shipment.Where(
                       s => search.SelectedTitles.Contains(s.title_id) && s.status == "Pending" && s.type == search.SelectedType)
                   .Select(s => new TitleToSend
                   {
                       TitleId = s.title_id,
                       TitleName = s.shipment_title.title1,
                       Quantity = s.quantity
                   })
               });

        }

        public List<ShipmentCsvLine> GetPendingShipments(ShipmentSearch search)
        {
            if (search.SelectedTitles.Count() == 0)
                throw new ArgumentNullException();

            return dbContext.contacts.Where(c => c.contact_shipment.Where(
               s => search.SelectedTitles.Contains(s.title_id) && s.status == "Pending" && s.type == search.SelectedType).Count() > 0)
               .Select(c => new ShipmentCsvLine
               {
                   FirstName = c.firstname,
                   LastName = c.lastname,
                   SubNumber = c.is_subscriber ? c.sub_number : null,
                   AddressLine1 = c.is_primary ? c.addressline1 : c.addressline1_alt,
                   AddressLine2 = c.is_primary ? c.addressline2 : c.addressline2_alt,
                   State = c.is_primary ? c.state : c.state_alt,
                   Zip = c.is_primary ? c.zip : c.zip_alt,
                   Country = c.is_primary ? c.country : c.country_alt,
                   Title = c.is_primary ? c.title : c.title_alt,
                   City = c.is_primary ? c.city : c.city_alt,
                   Organization = c.is_primary ? c.organization : c.organization_alt,
                   TitleList = c.contact_shipment.Where(
                       s => search.SelectedTitles.Contains(s.title_id) && s.status == "Pending" && s.type == search.SelectedType)
                       .Select(s => new TitleToSend
                       {
                           TitleId = s.title_id,
                           TitleName = s.shipment_title.title1,
                           Quantity = s.quantity
                       }),
                   PrimaryEmail = c.email1,
                   AltEmail = c.email2,
                   FollowUpShipments = c.contact_shipment.Where(s => s.should_followup).Select(s => s.id)
               }).ToList();

        }

        public int MarkShipmentsSent(ShipmentSearch search, int userid)
        {
            int count = 0;
            if (search.SelectedTitles.Count() == 0)
                throw new ArgumentNullException();

            var ships = dbContext.shipments.Where(s => search.SelectedTitles.Contains(s.title_id) && s.status == "Pending" && s.type == search.SelectedType);

            foreach (shipment s in ships)
            {
                s.date_sent = DateTime.Now;
                s.should_followup = search.MarkAsFollowUp;
                s.updatedat = DateTime.Now;
                s.updatedby = userid;
                s.status = "Sent";
                count++;
            }
            dbContext.SaveChanges();
            return count;

        }
        /// <summary>
        /// CRUD 
        /// </summary>
        public Shipment GetById(int id)
        {
            var model = dbContext.shipments.FirstOrDefault(x => x.id == id);
            if (model == null)
                return null;
            Shipment shipment = new Shipment
            {
                Id = model.id,
                TitleId = model.title_id,
                TitleName = model.shipment_title.title1,
                TitlePubDate = model.shipment_title.date_published,
                ContactId = model.contact_id,
                ContactName = model.shipment_contact.firstname + " " + model.shipment_contact.lastname + " (" +
                    (model.shipment_contact.is_primary ? model.shipment_contact.organization : model.shipment_contact.organization_alt) + ")",
                DateSent = model.date_sent,
                Quantity = model.quantity,
                ShouldFollowUp = model.should_followup,
                Status = model.status,
                Type = model.type,
                LastUpdated = string.Format("{0} at {1}", model.shipment_user.username, model.updatedat.ToShortDateString())
            };
            shipment.TitleName = shipment.TitleName + " (" + shipment.TitlePubDate.ToShortDateString() + ")";
            return shipment;

        }


        public void Update(Shipment model, int updatedby)
        {
            var obj = dbContext.shipments.First(x => x.id == model.Id);
            {
                obj.title_id = model.TitleId;
                obj.contact_id = model.ContactId;
                obj.date_sent = model.DateSent;
                obj.quantity = model.Quantity;
                obj.should_followup = model.ShouldFollowUp;
                obj.status = model.Status;
                obj.type = model.Type;
                obj.updatedat = DateTime.Now;
                obj.updatedby = updatedby;
                dbContext.SaveChanges();
            }

        }

        public void Insert(Shipment model, int createdBy)
        {

            var obj = new shipment
                {

                    title_id = model.TitleId,
                    contact_id = model.ContactId,
                    date_sent = model.DateSent,
                    quantity = model.Quantity,
                    should_followup = model.ShouldFollowUp,
                    status = model.Status,
                    type = model.Type,
                    createdat = DateTime.Now,
                    createdby = createdBy,
                    updatedat = DateTime.Now,
                    updatedby = createdBy,
                };

            dbContext.shipments.Add(obj);
            dbContext.SaveChanges();

        }

        public void Insert(List<Shipment> list, int createdBy)
        {
            foreach (var model in list)
            {
                var obj = new shipment
                {

                    title_id = model.TitleId,
                    contact_id = model.ContactId,
                    date_sent = model.DateSent,
                    quantity = model.Quantity,
                    should_followup = model.ShouldFollowUp,
                    status = model.Status,
                    type = model.Type,
                    createdat = DateTime.Now,
                    createdby = createdBy,
                    updatedat = DateTime.Now,
                    updatedby = createdBy,
                };

                dbContext.shipments.Add(obj);
            }
            dbContext.SaveChanges();

        }

        public void Delete(int id)
        {
            var model = dbContext.shipments.FirstOrDefault(x => x.id == id);
            if (model == null)
                return;

            dbContext.shipments.Remove(model);
            dbContext.SaveChanges();

        }

        private int GetQuantityForSearchType(contact c, ShipmentSearch search)
        {
            if (search.SelectedType == "Galleys")
                return c.galley_copies ?? 1;
            if (search.SelectedType == "Review")
                return c.review_copies ?? 1;
            if (search.SelectedType == "Desk")
                return c.desk_copies ?? 1;
            if (search.SelectedType == "Comp")
                return c.comp_copies ?? 1;
            throw new ArgumentNullException();
        }

    }
}
