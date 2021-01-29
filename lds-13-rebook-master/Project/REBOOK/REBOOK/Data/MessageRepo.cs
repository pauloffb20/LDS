using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using REBOOK.Exceptions;
using REBOOK.Models;

namespace REBOOK.Data
{
    public class MessageRepo : IMessageRepo
    {
        
        private readonly RebookDbContext _db = new RebookDbContext();
        
        public MessageRepo(RebookDbContext dbContext)
        {
            _db = dbContext;
        }
        
        public MessageRepo()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remetente"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ChatInfo> GetChats(int remetente)
        {
            try
            {
                List<ChatInfo> ChatsInfo = new List<ChatInfo>();
                List<ChatUsers> Chats = _db.ChatUsers.FromSqlRaw($"SELECT * FROM rebookdb.chatusers WHERE UserId={remetente}")
                    .ToList();

                foreach (ChatUsers chat in Chats)
                {
                    ChatInfo info = new ChatInfo();
                    info.ChatId = chat.ChatID;
                    
                    Message lastMessage = GetLastMessage(chat.ChatID);
                    info.LastMessage = lastMessage.Text;

                    ChatUsers destinatario = _db.ChatUsers.FromSqlRaw("SELECT * FROM rebookdb.chatusers " +
                                                                      $"WHERE ChatID={chat.ChatID} AND not( UserId = {remetente})")
                        .SingleOrDefault();
                    
                    if (destinatario == null)
                    {
                        throw new Exception("Não existe destinatário para este chat!");
                    }
                    
                    info.UserId = destinatario.UserId;

                    User userInfo = _db.Users.Where(u => u.Id == destinatario.UserId).SingleOrDefault();

                    info.UserName = userInfo.Name;
                    
                    ChatsInfo.Add(info);
                }

                return ChatsInfo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseIsEmptyException"></exception>
        public List<Message> GetMessages(int chatId)
        {
            List<Message> messages = _db.Messages.FromSqlRaw($"SELECT * FROM rebookdb.message WHERE ChatId = {chatId}")
                .ToList();
            
            if (messages.Count == 0 || messages == null)
            {
                throw new DatabaseIsEmptyException("No Messages in database");
            }

            messages.OrderByDescending(message => message.Time);
            
            return messages;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseIsEmptyException"></exception>
        public Message GetLastMessage(int chatId)
        {
            Message message = _db.Messages.FromSqlRaw($"SELECT * FROM rebookdb.message WHERE ChatId = {chatId} ORDER BY Id DESC LIMIT 1")
                .SingleOrDefault();

            if (message == null)
            {
                throw new DatabaseIsEmptyException("No Messages in database");
            }
            
            return message;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        public void SaveMessage(Message message)
        {
            try
            {
                _db.Messages.Add(message);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveUserHubConnection(string connection)
        {
            try
            {
                ChatConnections myConnection = 
                    _db.ChatConnections
                        .SingleOrDefault(ChatConnection => ChatConnection.Connection.Equals(connection));
                
                _db.ChatConnections.Remove(myConnection);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newConnection"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateUserHubConnection(int userId, string newConnection)
        {
            try
            {
                string query = $"UPDATE chatconnections SET ConnectionId={newConnection} WHERE UserId={userId}";
                _db.ChatConnections.FromSqlRaw(query);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddUserConnection(int userId, string connectionId)
        {
            try
            {
                ChatConnections connection = new ChatConnections(userId, connectionId);
                _db.ChatConnections.Add(connection);
                _db.SaveChanges();

                if (connection.Id == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetHubConnectionByUserId(int userId)
        {
            try
            {
                var connection = _db.ChatConnections
                    .FirstOrDefault(chatConnection => chatConnection.UserId == userId);

                if (connection != null)
                {
                    return connection.Connection;
                }

                return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remetente"></param>
        /// <param name="destinatário"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetUsersChat(int remetente, int destinatário)
        {
            try
            {
                var chatUsers = _db.ChatUsers
                    .FromSqlRaw("SELECT a.Id, a.ChatID, a.UserId " +
                                "FROM chatusers a " +
                                "INNER JOIN (" +
                                "SELECT ChatID," +
                                "COUNT(DISTINCT UserId) AS cnt " +
                                "FROM chatusers " +
                                $"WHERE UserId IN ({remetente}, {destinatário}) " +
                                "GROUP BY ChatId " +
                                "HAVING COUNT(DISTINCT UserId) = 2) AS b ON a.ChatId = b.ChatId " +
                                $"WHERE a.UserId IN ({remetente}, {destinatário}) LIMIT 1").SingleOrDefault();

                if (chatUsers != null)
                {
                    return chatUsers.ChatID;
                }
                
                return 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); 
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remetente"></param>
        /// <param name="destinatário"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int CreateChatForUsers(int remetente, int destinatário)
        {
            try
            {
                int chatId = CreateChatId();
                _db.ChatUsers.Add(new ChatUsers(remetente, chatId));
                _db.ChatUsers.Add(new ChatUsers(destinatário, chatId));
                _db.SaveChanges();
                return chatId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// Método que cria um chatId na BD
        /// </summary>
        /// <returns>Retorna o Id do chat</returns>
        private int CreateChatId()
        {
            try
            {
                Chat chat = new Chat();
                _db.Chat.Add(chat);
                _db.SaveChanges();
                return chat.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
