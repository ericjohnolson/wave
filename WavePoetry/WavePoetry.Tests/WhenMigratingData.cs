using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WavePoetry.Model;
using WavePoetry.OldDataAccess;

namespace WavePoetry.Tests
{
    [TestClass]
    public class WhenMigratingData
    {
        [TestMethod]
        public void CanMigrateTitles()
        {
            Dictionary<int, int> contactMap = new Dictionary<int, int>();
            Dictionary<int, int> titleMap = new Dictionary<int, int>();
            WavePoetry.DataAccess.TitleData titles = new WavePoetry.DataAccess.TitleData();
            WavePoetry.DataAccess.ReviewData reviews = new WavePoetry.DataAccess.ReviewData();
            WavePoetry.DataAccess.ShipmentData shipments = new WavePoetry.DataAccess.ShipmentData();
            WavePoetry.DataAccess.ContactData contacts = new WavePoetry.DataAccess.ContactData();
            WavePoetry.DataAccess.AdminData adminData = new WavePoetry.DataAccess.AdminData();
            WavePoetry.OldDataAccess.waveoldEntities dbContext = new WavePoetry.OldDataAccess.waveoldEntities();

            ////////////
            /// TITLES
            /// //////
            var oldTitles = dbContext.titles.ToList();
            foreach (title t in oldTitles)
            {
                var savedT = titles.Search(new TitleSearch { Title = t.title1 }).FirstOrDefault();
                titleMap.Add(t.id, savedT.TitleId);
            }

            ////////////
            /// CONTACTS
            /// //////
            /// 

            var oldContacts = dbContext.contacts.ToList();
            foreach (contact c in oldContacts)
            {
                var savedContact = contacts.Search(c.city, c.fname, c.lname, c.organization).FirstOrDefault();
                if(savedContact != null)
                    contactMap.Add(c.id, savedContact.Id);
                else
                    contactMap.Add(c.id, 2);
            }

            var oldCompsSent = dbContext.contactfinals.ToList();
            List<Shipment> ships = new List<Shipment>();
            foreach (var ship in oldCompsSent)
            {
                int outValue;
                int quant = int.TryParse(ship.number, out outValue) ? outValue : 0;

                string status = "Pending";
                if (ship.contactfinal1.Trim().ToLower() == "shipped")
                    status = "Sent";

                int contactId = 2;
                int titleId = 268;
                if (contactMap.ContainsKey(ship.contact_id))
                    contactId = contactMap[ship.contact_id];
                if (titleMap.ContainsKey(ship.title_id))
                    titleId = titleMap[ship.title_id];

                Shipment shipment = new Shipment
                {
                    ContactId = contactId,
                    TitleId = titleId,
                    Status = status,
                    Quantity = quant,
                    DateSent = ship.modified_on,
                    Type = "Comp"

                };
                ships.Add(shipment);
            }

            shipments.Insert(ships, 1);
        }
    }
}
