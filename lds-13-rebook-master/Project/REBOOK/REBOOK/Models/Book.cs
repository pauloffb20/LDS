using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace REBOOK.Models
{
    public enum Stars
    {
        Undefined,
        One,
        Two,
        Three,
        Four,
        Five
}
    public class Book
    {
        
        public Book(string name, string author)
        {
            Name = name;
            Author = author;
            //Photos = new List<Photo>();
            State = Stars.Undefined;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Author { get; set; }
        public Stars State {get; set; }
        public String Description { get; set; }
        public Photo Photos{ get; set; }
        
        [ForeignKey("OwnerId")] 
        public User Owner { get; set; }
        public int OwnerId { get; set; }

        public List<BooksLiked> BooksLiked { get; set; }
    }
}