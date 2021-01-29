using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REBOOK.Data;
using REBOOK.Models;
using REBOOK.Services;

namespace REBOOK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        
        private readonly MessageRepo _messageRepo;
        
        public MessageController()
        {
            _messageRepo = new MessageRepo();
        }
        
        [HttpGet("GetChats")]
        [AllowAnonymous]
        public ActionResult<List<ChatInfo>> GetChats()
        {
            try
            {
                int? loggedUser = ClaimService.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                if (loggedUser == null)
                {
                    return Unauthorized("Utilizador não autorizado ou inexistente!");
                }
                
                List<ChatInfo> chats = _messageRepo.GetChats((int) loggedUser);
                return Ok(chats);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("chatMessages/{id}")]
        [AllowAnonymous]
        public IActionResult GetMessagesByUserId(int id)
        {
            try
            {
                List<Message> mensagens = _messageRepo.GetMessages(id);
                return Ok(mensagens);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("lastChatMessage/{id}")]
        [AllowAnonymous]
        public ActionResult<Message> GetLastMessageFromChat(int id)
        {
            try
            {
                Message mensagem = _messageRepo.GetLastMessage(id);
                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}