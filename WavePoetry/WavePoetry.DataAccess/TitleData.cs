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
            return new Title
            {
                Id = title.id,
                Name = title.title1,
                Notes = title.notes,
                Author = title.author,
                Edition = title.edition,
                ISBN = title.isbn,
                PubDate = title.date_published,
                Subtitle = title.subtitle,
                AuthorName = title.author_contact.lastname + ", " + title.author_contact.firstname
            };
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

    }
}
