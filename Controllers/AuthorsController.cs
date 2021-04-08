using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibraryAPI;

namespace LibraryAPI.Controllers
{
    // Sofia Forsberg Ehn - Avancerad C# Laboration i GUI
    // För att koppla till API:t ange "/Authors"
    public class AuthorsController : ApiController
    {
        private LibraryModel db = new LibraryModel();
 
        // GET: /Authors
        public IQueryable<Authors> GetAuthors()
        {
            return db.Authors;
        }

        // GET: /Authors/5
        [ResponseType(typeof(Authors))]
        public IHttpActionResult GetAuthors(int id)
        {
            Authors authors = db.Authors.Find(id);
            if (authors == null)
            {
                return NotFound();
            }

            return Ok(authors);
        }

        // PUT: /Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuthors(int id, Authors authors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authors.Id)
            {
                return BadRequest();
            }

            var dbAuthor = db.Authors.Find(authors.Id);
            dbAuthor.FirstName = authors.FirstName;
            dbAuthor.LastName = authors.LastName;
            dbAuthor.Country = authors.Country;

            db.Entry(dbAuthor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: /Authors
        [ResponseType(typeof(Authors))]
        public IHttpActionResult PostAuthors(Authors authors)
        {
            List<Books> bookAuthors = new List<Books>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var bookAuthor in authors.Books)
            {
                if (bookAuthor.Id != 0)
                {
                    Books book = db.Books.Find(bookAuthor.Id);
                    bookAuthors.Add(book);
                }
                else
                {
                    bookAuthors.Add(bookAuthor);
                }
            }

            authors.Books = bookAuthors;
            db.Authors.Add(authors);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = authors.Id }, authors);
        }

        // DELETE: /Authors/5
        [ResponseType(typeof(Authors))]
        public IHttpActionResult DeleteAuthors(int id)
        {
            Authors authors = db.Authors.Find(id);
            if (authors == null)
            {
                return NotFound();
            }

            db.Authors.Remove(authors);
            db.SaveChanges();

            return Ok(authors);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorsExists(int id)
        {
            return db.Authors.Count(e => e.Id == id) > 0;
        }
    }
}