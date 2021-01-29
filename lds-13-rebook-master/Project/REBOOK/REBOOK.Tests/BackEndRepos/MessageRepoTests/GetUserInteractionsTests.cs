using System;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.MessageRepoTests
{
    public class GetUserInteractionsTests
    {
        /*
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly MessageRepo _messageRepo;

        public GetUserInteractionsTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _messageRepo = new MessageRepo(_testDbAccess);
        }

        [Fact]
        public void BothUsersExistTest()
        {
            User user1 = new User("Username", "password");
            User user2 = new User("Username2", "password");
            _testDbAccess.Add(user1);
            _testDbAccess.Add(user2);
            Message message1 = new Message("some text",1,2,new DateTime());
            Message message2 = new Message("some text",1,2,new DateTime());
            Message message3 = new Message("some text",2,1,new DateTime());
            Message message4 = new Message("some text",2,1,new DateTime());

            _messageRepo.SendMessage(message1);
            _messageRepo.SendMessage(message2);
            _messageRepo.SendMessage(message3);
            _messageRepo.SendMessage(message4);
            
            Assert.Equal(4, _messageRepo.GetUserInteractions(2,1).Count());
            _testDb.Erase();
        }

        [Fact]
        public void User1DoesNotExistTest()
        {
            User user = new User("Username", "password");
            _testDbAccess.Add(user);
            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.GetUserInteractions(2, 1));
            _testDb.Erase();
        }
        
        [Fact]
        public void User2DoesNotExistTest()
        {
            User user = new User("Username", "password");
            _testDbAccess.Add(user);
            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.GetUserInteractions(1, 2));
            _testDb.Erase();
        }
        */
    }
}