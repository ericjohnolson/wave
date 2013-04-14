using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavePoetry.Model
{
    public class Title
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Subtitle { get; set; }
        [Required]
        public int Author { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public string Edition { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required, DisplayName("Published Date")]
        public DateTime PubDate { get; set; }
        public string Notes { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Contact> Contacts { get; set; }
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
