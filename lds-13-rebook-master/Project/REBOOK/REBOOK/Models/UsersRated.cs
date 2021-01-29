using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class UsersRated
    {
        public UsersRated(User rater, User rated, int rating)
        {
            Rater = rater;
            Rated = rated;
            Rating = rating;
        }
        
        public UsersRated(){}
        
        [ForeignKey("RaterId")]
        public User Rater { get; set; }
        public int RaterId { get; set; }

        [ForeignKey("RatedId")]
        public User Rated { get; set; }
        public int RatedId { get; set; }
        
        public int Rating { get; set; }
    }
}