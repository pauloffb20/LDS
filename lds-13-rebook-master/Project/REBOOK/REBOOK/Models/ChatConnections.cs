using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class ChatConnections
    {
        public ChatConnections()
        {
            
        }

        public ChatConnections(int userId, string connection)
        {
            UserId = userId;
            Connection = connection;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Connection { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
    }
}