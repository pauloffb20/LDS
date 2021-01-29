using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class User
    {
         public User(string name, string password)
        {
            Name = name;
            Password = password;
            CountRating = 0;
            SumRating = 0;
            BookList = new List<Book>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Password { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public String Email { get; set; }

        [DefaultValue(10)]
        public int Range { get; set; }
        public String Description { get; set; }
        public int Rating { get; set; }
        public int CountRating { get; set; }
        public int SumRating { get; set; }
        
        public List<Book> BookList { get; set; }
        public List<BooksLiked> BooksLiked { get; set; }
        public List <UsersRated> UsersRated { get; set; }
        public List<ChatUsers> ChatUsers { get; set; }
        public Photo Photo { get; set; }
        
        
        public void averageRating(int newRating)
        {
            CountRating++;
            SumRating += newRating;
            
            Rating = SumRating / CountRating;
        }
    }
}