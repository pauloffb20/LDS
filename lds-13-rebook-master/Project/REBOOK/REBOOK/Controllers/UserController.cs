using System;
using System.Data.Entity.Core;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using REBOOK.Services;

namespace REBOOK.Controllers
{

    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserRepo _userRepo;
        private readonly PhotoRepo _photoRepo;
        public UserController()
        {
            _userRepo = new UserRepo();
            _photoRepo = new PhotoRepo();
        }

        [HttpPost]
        [Route("/authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] User credentials)
        {
            var user = _userRepo.Login(credentials.Name, credentials.Password);
            
            if (user == null)
            {
                return NotFound(new {message = "Utilizador ou password inválidos"});
            }

            user.Token = TokenService.GenerateToken(user);
            user.Password = "";
            
            return Json(user);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> NewUser()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var json = httpRequest.Form["newUser"];
                var newUser = JsonConvert.DeserializeObject<User>(json);
                _userRepo.AddAccount(newUser);
                //If photo was uploaded add it to user
                if (httpRequest.Form.Files.Count == 1)
                {
                    _photoRepo.addPhoto(httpRequest.Form.Files.First(), newUser);
                }

                //var token = TokenService.GenerateToken(newUser);
                return Json(newUser);
            }
            catch (UserAlreadyExistsException e)
            {
                return StatusCode(409);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetUsers()
        {
            return Json(_userRepo.GetAllUsers());
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _userRepo.GetUserById(id);
                return Json(user);
            }
            catch ( ObjectNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpGet("name/{username}")]
        [AllowAnonymous]
        public IActionResult GetUserByName(string username)
        {
            try
            {
                int id = _userRepo.GetUserIdByName(username);
                return Json(id);
            }
            catch (ObjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult DeleteUser(int id)
        {
            _userRepo.DeleteUser(id);
            return Ok();
        }

        [HttpGet("books/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> BooksByUser(int id)
        {
            try
            {
                var Books = _userRepo.GetUserById(id).BookList;
                return Json(Books);
            }
            catch ( ObjectNotFoundException ex)
            {
                return NotFound();
            }
        }
        
        [HttpPut]
        [AllowAnonymous]
        public IActionResult EditUser(User user)
        {
            _userRepo.EditUser(user);
            return Ok();
        }        
        
        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User credentials)
        {
            Console.WriteLine("REACHED");

            User user = _userRepo.Login(credentials.Name, credentials.Password);
            if (user == null)
            {
                return NotFound("Wrong Credentials");
            }
            
            return Json(user);
        }

        
        [HttpGet("{id}/matches")]
        [AllowAnonymous]
        public IActionResult GetMatches(int id)
        {
            try
            {
                return Json(_userRepo.GetMatches(id).ToList());
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{userId}/setfavorites/{bookId}")]
        [AllowAnonymous]
        public IActionResult AddBookToFavorites(int userId, int bookId)
        {
            try
            {
                _userRepo.AddBookToFavorites(userId, bookId);
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{userId}/favorites/{bookId}")]
        [AllowAnonymous]
        public IActionResult RemoveBookFromFavorites(int userId, int bookId)
        {
            try
            {
                _userRepo.RemoveBookFromFavorites(userId, bookId);
                return Ok();
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{userId}/favorites")]
        [AllowAnonymous]
        public IActionResult GetAllFavorites(int userId)
        {
            try
            {
               return( Json(_userRepo.getAllFavorites(userId)));
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("rate/{raterId}/{ratedId}")]
        [AllowAnonymous]
        public IActionResult RateUser(int raterId, int ratedId, [FromBody] RatingModel ratingModel)
        {
            try
            {
                _userRepo.RateUser(ratedId, raterId, ratingModel.Rating);
                return Ok();
            }
            catch (InvalidObjectException i)
            {
                return Json(i.Message);
            }
            catch (ObjectNotFoundException o)
            {
                return Json(o.Message);
            }
        }
        
        [HttpPost("/logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}