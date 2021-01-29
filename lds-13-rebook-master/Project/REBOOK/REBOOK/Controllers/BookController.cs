using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;

namespace REBOOK.Controllers
{
    [Route("book")]
    [ApiController]
    public class BookController : Controller
    {
        
        private readonly BookRepo _bookRepo;
        private readonly PhotoRepo _photoRepo;

        public BookController()
        {
            _bookRepo = new BookRepo();
            _photoRepo = new PhotoRepo();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBooks()
        {
            return Json(_bookRepo.GetBooks());
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task< IActionResult> NewBook()
        { 
            try
            {
                var httpRequest = HttpContext.Request;
                var json = httpRequest.Form["newBook"];
                var newBook = JsonConvert.DeserializeObject<Book>(json);
                IFormFile file = httpRequest.Form.Files.First();
                _bookRepo.AddBook(newBook);
                _photoRepo.addPhoto(file, newBook);
                return Json(newBook);
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidObjectException e)
            {
                return Json(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return NotFound(e.Message);
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public JsonResult GetBook(int id)
        {
            var book = _bookRepo.GetBookById(id);
            return Json(book);
        }
        
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult DeleteBook(int id)
        {
            try {
                _bookRepo.DeleteBook(id);
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok();
        }
        
        [HttpPut]
        [AllowAnonymous]
        public IActionResult EditBook([FromBody] Book book)
        {
            try {
                _bookRepo.EditBook(book);
                return Ok();
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}