using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xamarin.Essentials;

namespace REBOOK.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly RebookDbContext _db = new RebookDbContext();
        
        public UserRepo(RebookDbContext dbContext)
        {
            _db = dbContext;
        }
        public UserRepo() {}
        
        //Adds new user instance to database
        public void AddAccount(User user)
        {

            var username = user.Name;
            
            var password = user.Password;
            const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            
            var email = user.Email;
            var emailPattern =
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("rebookg13@gmail.com", "rebookg13lds");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("rebookg13@gmail.com", "rebookg13@gmail.com");
            mail.From = new MailAddress("rebookg13@gmail.com", "reBOOK");
            mail.To.Add(new MailAddress(email.ToString(), "reBOOK"));
            mail.Subject = "Credenciais de Acesso: ";
            mail.Body = "Credenciais de Acesso: <br/> " +
                        "Username: " + username.ToString() + "<br/> " +
                        "Email:  " + email.ToString() + "<br/> " +
                        "Password: " + password.ToString() + " <br/> "; 
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                throw new ObjectNotFoundException("Email Invalid!");
            }
            finally
            {
                mail = null;
            }
            
            if (_db.Users.Any(c => c.Name.Equals(user.Name)))
            {
                throw new UserAlreadyExistsException("Username already exists");    
            }

            if (!String.IsNullOrWhiteSpace(password) && !(Regex.IsMatch(password, passwordRegex)))
            {
                throw new ObjectNotFoundException("Password Invalid!");
            }
            
            if (!String.IsNullOrWhiteSpace(email) && !(Regex.IsMatch(email, emailPattern)))
            {
                throw new ObjectNotFoundException("Email Invalid!");
            }
            
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public int GetUserIdByName(string username)
        {
            var user = _db.Users.Where(u => u.Name.ToLower() == username.ToLower()).FirstOrDefault();
            
            if (user == null) {
                throw new ObjectNotFoundException("User not found");
            }

            return user.Id;
        }
        
        
        //Returns user by Id
        public User GetUserById(int id)
        {
            var user = _db.Users.Where(u => u.Id == id)
                .Include(u =>u.BookList)
                .FirstOrDefault();
            

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found");
            }

            return user;
        }

        //Edits user in db
        public void EditUser(User user)
        {
            User current = _db.Users.Find(user.Id);
            if (current == null)
            {
                throw new ObjectNotFoundException("User not Found");
            }

            current.Email = user.Email;
            current.Name = user.Name;
            current.Password = user.Password;

            var username = user.Name;
            var password = user.Password;
            const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";

            var email = user.Email;
            var emailPattern =
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("rebookg13@gmail.com", "rebookg13lds");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("rebookg13@gmail.com", "rebookg13@gmail.com");
            mail.From = new MailAddress("rebookg13@gmail.com", "reBOOK");
            mail.To.Add(new MailAddress(email.ToString(), "reBOOK"));
            mail.Subject = "Credenciais de Acesso Alteradas: ";
            mail.Body = "Caro utilizador, as suas credenciais de acesso foram alteradas com sucesso! " + "<br/>" +
                        "" + "<br/>" + 
                        "Credenciais de Acesso: <br/> " +
                        "UserName" + username.ToString() + "<br/> " +
                        "Email:  " + email.ToString() + "<br/> " +
                        "Password: " + password.ToString() + " <br/> "; 
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                throw new ObjectNotFoundException("Email Invalid!");
            }
            finally
            {
                mail = null;
            }
            if (!String.IsNullOrWhiteSpace(password) && !(Regex.IsMatch(password, passwordRegex)))
            {
                throw new ObjectNotFoundException("Password Invalid!");
            }
            
            if (!String.IsNullOrWhiteSpace(email) && !(Regex.IsMatch(email, emailPattern)))
            {
                throw new ObjectNotFoundException("Email Invalid!");
            }
            
            _db.SaveChanges();
        }

        //Returns all users
        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        //Returns true if username-password combination is found in db
        public User Login(string username, string password)
        {
            User user = _db.Users.Where(u => u.Name.Equals(username)).FirstOrDefault();
            if (user == null) return null;
            if (user.Password.Equals(password)) {
                return user;
            }

            return null;
        }

        //Deletes user as well as all user's books and all instances of Matches
        //and RatedUsers where user is 
        public void DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                throw new ObjectNotFoundException("User not Found");
            }

            //Deletes instances of user in matches
            var matchesToDelete = _db.Matches.Where(matches => matches.UserId1 == id | matches.UserId2 == id).ToList();
            _db.Matches.RemoveRange(matchesToDelete);
            
            //Deletes instance of user in RatedUsers (where he has been rated)
            var ratingsToDelete = _db.UsersRated.Where(ratings => ratings.RatedId == id).ToList();
            _db.UsersRated.RemoveRange(ratingsToDelete);
                
            _db.Users.Remove(user);
            _db.SaveChanges();
        }
        
        public IEnumerable<User> GetMatches(int id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found!");
            }
            
            List<User> userList = new List<User>();
            var matchList = _db.Matches.Where(match => match.UserId1 == id || match.UserId2 == id).ToList();
            foreach (var match in matchList)
            {
                if (match.Matched)
                {
                    if (match.UserId1 != id)
                    {
                        userList.Add(match.User1);
                    }

                    if (match.UserId2 != id)
                    {
                        userList.Add(match.User2);
                    }
                }
            }
            return userList;
        }

        public void AddBookToFavorites(int userId, int bookId)
        {
            var book = _db.Books.Find(bookId);
            var user = _db.Users.Find(userId);
            
            if (book == null)
            {
                throw new ObjectNotFoundException("Book not found");
            }
            
            if (user == null)
            {
                throw new ObjectNotFoundException("User not found");
            }

            // If userA has liked userB book, a match object will be created with user1 = userA 
            // and user2 = userB, at this point the match object is yet to be declared as a Match.
            // We will search for match instance with user1 = userB and user2 = UserA
            // If it exists, then the match instance is declared matched(if it wasnt so already)
            // If it does not we'll create a match instance with user1 = userA and user2 = userB
            
            var match = _db.Matches.Find(book.OwnerId, userId);
            if (match == null)
            { 
                match = _db.Matches.Find(userId, book.OwnerId);
                if (match == null)
                {
                    Matches newMatch = new Matches(user, _db.Users.Find(book.OwnerId));
                    _db.Matches.Add(newMatch);
                }
                
            } else if (!match.Matched){
                match.Matched = true;
            }
            
            _db.BooksLiked.Add(new BooksLiked(user, book));
            _db.SaveChanges();
            
        }

        public void RemoveBookFromFavorites(int userId, int bookId)
        {
            var book = _db.Books.Find(bookId);
            var user = _db.Users.Find(userId);
            
            if (book == null)
            {
                throw new ObjectNotFoundException("Book not found");
            }
            
            if (user == null)
            {
                throw new ObjectNotFoundException("User not found");
            }

            var liked = _db.BooksLiked.Find(userId, bookId);
            
            if (liked == null)
            {
                throw new ObjectNotFoundException("Book not in favorites");
            }

            _db.BooksLiked.Remove(liked);
            _db.SaveChanges();

        }

        public List<Book> getAllFavorites(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User not found");
            }

            List<Book> list = _db
                .BooksLiked.Where(bl => bl.UserId == userId).Select(bl => bl.Book)
                .ToList();
            
            
            return list;
        }

        public void RateUser(int raterId, int ratedId, int rating)
        {
            var rater = _db.Users.Find(raterId);
            var rated = _db.Users.Find(ratedId);
            
            if (rater == null)
            {
                throw new ObjectNotFoundException("Rater not found");
            }
            
            if (rated == null)
            {
                throw new ObjectNotFoundException("Rated not found");
            }

            if (_db.UsersRated.Find(raterId,ratedId) != null)
            {
                throw new InvalidObjectException("User already rated");
            }
            
            if (rating < 0 || rating > 5)
            {
                throw new InvalidObjectException("Rating must be between 0 and 5");
            }

            _db.UsersRated.Add(new UsersRated(rater, rated, rating));
            rated.averageRating(rating);
            _db.SaveChanges();
        }
        
    }
}