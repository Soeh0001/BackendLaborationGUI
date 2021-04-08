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
    // För att koppla till API:t ange "/Books"
    public class BooksController : ApiController
    {
        private LibraryModel db = new LibraryModel();

        // GET: /Books
        public IQueryable<Books> GetBooks()
        {
            return db.Books;
        }

        // GET: /Books/5
        [ResponseType(typeof(Books))]
        public IHttpActionResult GetBooks(int id)
        {
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        // PUT: Books/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooks(int id, Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != books.Id)
            {
                return BadRequest();
            }

            var dbBook = db.Books.Find(books.Id);
            dbBook.Title = books.Title;
            dbBook.Published = books.Published;
            dbBook.Language = books.Language;
            dbBook.Genre = books.Genre;

            db.Entry(dbBook).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
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

        // POST: /Books
        [ResponseType(typeof(Books))]
        public IHttpActionResult PostBooks(Books books)
        {
            List<Authors> bookAuthors = new List<Authors>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var bookAuthor in books.Authors)
            {
                if (bookAuthor.Id != 0)
                {
                    Authors author = db.Authors.Find(bookAuthor.Id);
                    bookAuthors.Add(author);
                }
                else
                {
                    bookAuthors.Add(bookAuthor);
                }
            }

            books.Authors = bookAuthors;
            db.Books.Add(books);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = books.Id }, books);
        }

        // DELETE: /Books/5
        [ResponseType(typeof(Books))]
        public IHttpActionResult DeleteBooks(int id)
        {
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return NotFound();
            }

            db.Books.Remove(books);
            db.SaveChanges();

            return Ok(books);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BooksExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}