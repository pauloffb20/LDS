using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting.Internal;
using REBOOK.Models;

namespace REBOOK.Data
{
    public class PhotoRepo
    {
        private readonly RebookDbContext _db = new RebookDbContext();

        public PhotoRepo(RebookDbContext dbContext)
        {
            _db = dbContext;
        }

        public PhotoRepo()
        {
        }

        public async void addPhoto(IFormFile file, User user)
        {
            try
            {

                Photo photo = new Photo();
                using (var stream = new FileStream("./Photos/User/picture" + user.Id + ".png", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    photo.FilePath = "./Photos/User/picture" + user.Id + ".png";
                    user.Photo = photo;
                    _db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public async void addPhoto(IFormFile file, Book book)
        {
            try
            {
                Photo photo = new Photo();
                using (var stream = new FileStream("./Photos/Book/picture" + book.Id + ".png", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    photo.FilePath = "./Photos/Book/picture" + book.Id + ".png";
                    book.Photos = photo;
                    _db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
    
