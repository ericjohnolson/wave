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
        public IEnumerable<ShipmentDetails> Search(ShipmentSearch search)
        {
            throw new NotImplementedException();
        }

        public Shipment GetById(int id)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
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
                ContactName = model.shipment_contact.firstname + " " + model.shipment_contact.lastname,
                DateSent = model.date_sent,
                Quantity = model.quantity,
                ShouldFollowUp = model.should_followup,
                Status = model.status,
                Type = model.type,
                LastUpdated = string.Format("{0} at {1}", model.shipment_user.username, model.updatedat.ToShortDateString())
            };
            shipment.TitleName = shipment.TitleName + "(" + shipment.TitlePubDate.ToShortDateString() + ")";
            return shipment;
        }


        public void Update(Shipment model, int updatedby)
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
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
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
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


    }
}
