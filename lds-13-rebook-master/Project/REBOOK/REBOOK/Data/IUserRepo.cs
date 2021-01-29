
using System.Collections.Generic;
using REBOOK.Models;

namespace REBOOK.Data
{
    public interface IUserRepo
    {
        
        void AddAccount(User user);
        User GetUserById(int id);
        void EditUser(User user);
        IEnumerable<User> GetAllUsers();
        User Login(string username, string password);
        void DeleteUser(int id);
        IEnumerable<User> GetMatches(int id);
    }
}

