using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;


namespace REBOOK.Tests.BackEndRepos.MessageRepoTests
{
    public class GetMessages
    {
        /*
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly MessageRepo _messageRepo;

        public GetMessages()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _messageRepo = new MessageRepo(_testDbAccess);
        }

        [Fact]
        public void ShouldWorkTest()
        {
            Message message = new Message("text 123", 1, 2, DateTime.Now);
            
            User sender = new User("Alex", "password");
            User receiver = new User("John", "password");

            _testDbAccess.Add(sender);
            _testDbAccess.Add(receiver);
            _messageRepo.SendMessage(message);

            IEnumerable<Message> message1 = _testDbAccess.Messages.ToList();
            
            
            Assert.Equal(_messageRepo.GetMessages(),message1);
            _testDb.Erase();
        }
        */
    }
}