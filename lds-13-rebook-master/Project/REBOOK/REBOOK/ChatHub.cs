using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using REBOOK.Data;
using REBOOK.Models;
using REBOOK.Services;

namespace REBOOK
{
    [Authorize]
    public class ChatHub : Hub
    {
        private MessageRepo mRepo;
        private UserRepo uRepo;
        
        public ChatHub(RebookDbContext dbContext)
        {
            mRepo = new MessageRepo(dbContext);
            uRepo = new UserRepo(dbContext);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionDestinatario"></param>
        /// <param name="connectionRemetente"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(int destinatario, int remetente, string message, DateTime Time)
        {
            try
            {
                string cDestinatario = mRepo.GetHubConnectionByUserId(destinatario);
                string cRemetente = mRepo.GetHubConnectionByUserId(remetente);
                
                int hasChat = mRepo.GetUsersChat(remetente, destinatario);
                int chatID;
                
                if (hasChat == 0)
                { 
                    chatID = mRepo.CreateChatForUsers(remetente, destinatario);
                }
                else
                {
                    chatID = mRepo.GetUsersChat(remetente, destinatario);
                }

                string SenderName = uRepo.GetUserById(remetente).Name;
                
                if (cDestinatario.Length > 0)
                {
                    await Clients.Clients(cDestinatario, cRemetente)
                        .SendAsync("ReceiveMessage", message, SenderName);
                    
                    mRepo.SaveMessage(new Message(message, chatID, remetente, Time, SenderName));
                }
                else
                {
                    mRepo.SaveMessage(new Message(message, chatID, remetente, Time, SenderName));
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override async Task OnConnectedAsync()
        {
            try
            {
                await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

                int? userId = ClaimService.GetIdFromClaimIdentity((ClaimsIdentity)Context.User.Identity);

                if (userId != null)
                {
                    if (mRepo.GetHubConnectionByUserId((int) userId) != null)
                    {
                        mRepo.AddUserConnection((int) userId, Context.ConnectionId);
                    }
                    else
                    {
                        mRepo.UpdateUserHubConnection((int) userId, Context.ConnectionId);
                    }
                }
                else
                {
                    throw new Exception("Não permitido!");
                }

                await base.OnConnectedAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
                int? userId = ClaimService.GetIdFromClaimIdentity((ClaimsIdentity)Context.User.Identity);
                if (userId != null) mRepo.RemoveUserHubConnection(Context.ConnectionId);
                await base.OnDisconnectedAsync(ex);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}