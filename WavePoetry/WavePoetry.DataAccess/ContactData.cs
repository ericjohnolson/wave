using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavePoetry.DataAccess
{
    public class ContactData
    {
        public IEnumerable<contact> GetAllContacts()
        {
            wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
            var contacts = from c in dbContext.contacts select c;
            return contacts;
        }
         
    }
}
