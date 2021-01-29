using System.Collections.Generic;
using REBOOK.Models;

namespace REBOOK.Data
{
    public interface IMessageRepo
    {
        void SaveMessage(Message message);
        public List<ChatInfo> GetChats(int remetente);
        public List<Message> GetMessages(int chatId);
        public Message GetLastMessage(int chatId);
        public void RemoveUserHubConnection(string connection);
        public void UpdateUserHubConnection(int userId, string newConnection);
        public bool AddUserConnection(int userId, string connectionId);
        public string GetHubConnectionByUserId(int userId);
        public int GetUsersChat(int remetente, int destinatário);
        public int CreateChatForUsers(int remetente, int destinatário);

    }
}