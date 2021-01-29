using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees;

namespace REBOOK.Models
{
    public class BooksLiked
    {
        public BooksLiked(User user, Book book)
        {
            User = user;
            Book = book;
            BookId = book.Id;
            UserId = user.Id;
        }
        
        public BooksLiked() {}
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
        
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}