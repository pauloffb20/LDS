
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit.Sdk;


namespace REBOOK.Data
{
    public class BookRepo : IBooksRepo
    {
        private readonly RebookDbContext _db = new RebookDbContext();
        
        public BookRepo(RebookDbContext dbContext)
        {
            _db = dbContext;
        }
        public BookRepo() {}
        public List<Book> GetBooks()
        {
            return _db.Books.ToList();
        }
        
        public Book GetBookById(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
            {
                throw new ObjectNotFoundException("Book not Found");
            }
            
            return _db.Books.Find(id);
        }

        public void AddBook(Book book)
        {
            var owner = _db.Users
                .Where(user => user.Id == book.OwnerId)
                .Include(user => user.BookList)
                .First();

            try
            {
                if (book.Name == null)
                {
                    throw new InvalidObjectException("Name of book required");
                }

                if (book.Author == null)
                {
                    throw new InvalidObjectException("Author of book required");
                }

                if (book.State == Stars.Undefined)
                {
                    throw new InvalidObjectException("State required");
                }

                //if (book.Photos.Count == 0)
                //{
                //  throw new InvalidObjectException("Photos required");
                //}
                if (owner == null)
                {
                    throw new ObjectNotFoundException("User not found");
                }

                owner.BookList.Add(book);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteBook(int id)
        {

            try
            {
                var book = _db.Books.Find(id);

                if (book == null)
                {
                    throw new ObjectNotFoundException("Book not Found");
                }

                var user = _db.Users.Find(book.OwnerId);

                if (user == null)
                {
                    _db.Books.Remove(GetBookById(id));
                    _db.SaveChanges();
                    throw new ObjectNotFoundException("User Not Found");

                }

                if (!user.BookList.Contains(book))
                {
                    _db.Books.Remove(GetBookById(id));
                    _db.SaveChanges();
                    throw new ObjectNotFoundException("Book not found in UserList");

                }

                user.BookList.Remove(book);
                _db.Books.Remove(GetBookById(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void EditBook(Book book)
        {
            Book current = _db.Books.Find(book.Id);
            if (current == null)
            {
                throw new ObjectNotFoundException("Book not Found");
            }

            current.Description = book.Description;
            current.Author = book.Author;
            current.Name = book.Name;
            current.State = book.State;
            current.Photos = book.Photos;
            _db.SaveChanges();
        }
    }
}