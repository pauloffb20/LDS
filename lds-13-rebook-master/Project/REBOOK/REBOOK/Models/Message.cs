using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace REBOOK.Models
{
    public class Message
    {
        public Message()
        {

        }

        public Message(string text, int chatId, int senderId, DateTime time, string userName)
        {
            Text = text;
            ChatId = chatId;
            SenderId = senderId;
            Time = time;
            UserName = userName;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int ChatId { get; set; }
        [Required] public String Text { get; set; }

        [Required] public DateTime Time { get; set; }

        [Required] public string UserName { get; set; }

    [ForeignKey("SenderId")]
        public User User { get; set; }
        public int SenderId { get; set; }
    }
}