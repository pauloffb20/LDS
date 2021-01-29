using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;


namespace REBOOK.Tests.BackEndRepos.MessageRepoTests
{
    public class ChangeStatusByMessageId
    {
        /*
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly MessageRepo _messageRepo;

        public ChangeStatusByMessageId()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _messageRepo = new MessageRepo(_testDbAccess);
        }


        [Fact]
        public void TestB16_1()
        {
            User sender = new User("Alex", "password");
            User receiver = new User("John", "password");

            _testDbAccess.Add(sender);
            _testDbAccess.Add(receiver);
   
            Message message = new Message("text 123", 1, 2, DateTime.Now);
            
            _testDbAccess.Add(message);
            
            _messageRepo.ChangeStatusByMessageId(message.Id);
            
            var message2 = _testDbAccess.Messages.Find(message.Id);

            Assert.Equal(Status.Read, message2.Read);
            
            _testDb.Erase();
        }

        [Fact]
        public void TestB16_2()
        {
            
            
            User sender = new User("Alex", "password");

            _testDbAccess.Add(sender);
            
            Message message = new Message("text 123", sender.Id, 2, DateTime.Now);
            
            _testDbAccess.Add(message);

            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.ChangeStatusByMessageId(message.Id));

            _testDb.Erase();
        }
        */
    }
}