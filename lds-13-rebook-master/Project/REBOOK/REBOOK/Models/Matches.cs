using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace REBOOK.Models
{
    public class Matches
    {
        public Matches(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;
            UserId1 = user1.Id;
            UserId2 = user2.Id;
        }
        
        public Matches(){}
        
        [ForeignKey("UserId1")]
        public User User1 { get; set; }
        public int UserId1 { get; set; }
        
        [ForeignKey("UserId2")]
        public User User2 { get; set; }
        public int UserId2 { get; set; }
        
        public Boolean Matched { get; set; } = false;
    }
}