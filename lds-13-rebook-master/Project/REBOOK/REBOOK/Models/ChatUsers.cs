using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class ChatUsers
    {
        public ChatUsers()
        {
        }
        
        public ChatUsers(int userId, int chatId)
        {
            ChatID = chatId;
            UserId = userId;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("ChatID")]
        public Chat Chat { get; set; }
        public int ChatID { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }

    }
}