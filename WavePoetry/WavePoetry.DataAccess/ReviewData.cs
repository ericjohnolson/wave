﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WavePoetry.Model;

namespace WavePoetry.DataAccess
{
    public class ReviewData
    {
        wavepoetry2Entities1 dbContext = new wavepoetry2Entities1();
        public string GetTitleDisplayNameById(int id)
        {
                TitleDetails t2 = dbContext.titles.Where(t => t.id == id).Select(t =>
                   new TitleDetails
                   {
                       Title = t.title1,
                       PubDate = t.date_published
                   }).FirstOrDefault();
                return t2.Title + " (" + t2.PubDate.ToShortDateString() + ")";
        }

        public string GetContactDisplayNameById(int id)
        {
                ContactDetails con = dbContext.contacts.Where(t => t.id == id).Select(c2 =>
                   new ContactDetails
                   {
                       DisplayName = c2.firstname + " " + c2.lastname,
                       Organization = (c2.is_primary ? c2.organization : c2.organization_alt)
                   }).FirstOrDefault();
                return con.DisplayName + " (" + con.Organization + ")";
        }

        public Review GetById(int id)
        {
                var model = dbContext.reviews.FirstOrDefault(x => x.id == id);
                if (model == null)
                    return null;
                Review review = new Review
                {
                    Id = model.id,
                    CopyLink = model.copy_link,
                    DateReviewed = model.date_reviewed,
                    OriginalLink = model.original_link,
                    ReviewText = model.review_text,
                    Venue = model.venue,
                    ReviewerId = model.reviewedby,
                    ReviewerName = model.reviewedby > 0 ? model.review_contact.firstname + " " + model.review_contact.lastname + " (" +
                        (model.review_contact.is_primary ? model.review_contact.organization : model.review_contact.organization_alt) + ")" : string.Empty,
                    TitleId = model.title_id,
                    TitleName = model.review_title.title1,
                    TitlePubDate = model.review_title.date_published
                };
                review.TitleName = review.TitleName + " (" + review.TitlePubDate.ToShortDateString() + ")";
                return review;
        }

        public IEnumerable<ReviewDetails> Search(ReviewSearch search)
        {
                return dbContext.reviews.Where(x => x.venue.Contains(search.Venue) || (x.date_reviewed >= search.Start && x.date_reviewed <= search.End) ||
                   x.review_title.title1.Contains(search.Title) ||
                   x.review_contact.firstname.Contains(search.ReviewerFirstName) || x.review_contact.lastname.Contains(search.ReviewerLastName))
                   .Select(m => new ReviewDetails
                   {
                       Reviewer = m.reviewedby > 0 ? m.review_contact.lastname + ", " + m.review_contact.firstname : "",
                       ReviewedDate = m.date_reviewed,
                       Title = m.review_title.title1,
                       Venue = m.venue,
                       ReviewerId = m.reviewedby,
                       TitleId = m.title_id,
                       Id = m.id
                   });
        }

        public void Update(Review model, int updatedby)
        {
                var obj = dbContext.reviews.First(x => x.id == model.Id);
                {
                    obj.copy_link = model.CopyLink;
                    obj.date_reviewed = model.DateReviewed;
                    obj.original_link = model.OriginalLink;
                    obj.review_text = model.ReviewText;
                    obj.reviewedby = model.ReviewerId;
                    obj.title_id = model.TitleId;
                    obj.venue = model.Venue;
                    obj.updatedat = DateTime.Now;
                    obj.updatedby = updatedby;
                    dbContext.SaveChanges();
                }
        }

        public void Insert(Review model, int createdBy)
        {
                var newReview = new review
                    {
                        copy_link = model.CopyLink,
                        date_reviewed = model.DateReviewed,
                        original_link = model.OriginalLink,
                        review_text = model.ReviewText,
                        reviewedby = model.ReviewerId,
                        title_id = model.TitleId,
                        venue = model.Venue,
                        createdat = DateTime.Now,
                        createdby = createdBy,
                        updatedat = DateTime.Now,
                        updatedby = createdBy,
                    };

                dbContext.reviews.Add(newReview);
                dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = dbContext.reviews.FirstOrDefault(x => x.id == id);
            if (model == null)
                return;

            dbContext.reviews.Remove(model);
            dbContext.SaveChanges();
        }
    }
}
