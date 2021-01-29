using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;


namespace REBOOK.Tests.BackEndRepos.MessageRepoTests
{
    public class SendMessages
    {
        /*
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly MessageRepo _messageRepo;

        public SendMessages()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _messageRepo = new MessageRepo(_testDbAccess);
        }


        [Fact]
        public void ShouldSendMessageTest()
        {
            Message message = new Message("text 123", 1, 2, DateTime.Now);
            

            User sender = new User("Alex", "password");
            User receiver = new User("John", "password");

            _testDbAccess.Add(sender);
            _testDbAccess.Add(receiver);
            
            _messageRepo.SendMessage(message);
            Assert.Equal(_testDbAccess.Messages.Find(1),message);
            
            _testDb.Erase();
        }
        
        [Fact]
        public void TestBReceiverIdNotFound()
        {
            Message message = new Message("text examples",1,10,DateTime.Now);

            User sender = new User("Alex", "password");
            
            _testDbAccess.Add(sender);
            
            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.SendMessage(message));

            _testDb.Erase();
        }
        
        [Fact]
        public void TestBTextEmpty()
        {
            Message message = new Message(null,1,10,DateTime.Now);

            User sender = new User("Alex", "password");
            
            _testDbAccess.Add(sender);
            
            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.SendMessage(message));

            _testDb.Erase();
        }
        
        [Fact]
        public void TestBSenderIdNotFound()
        {
            Message message = new Message("",10,1,DateTime.Now);

            User receiver = new User("Alex", "password");
            
            _testDbAccess.Add(receiver);
            
            Assert.Throws<ObjectNotFoundException>(() => _messageRepo.SendMessage(message));

            _testDb.Erase();
        }
        */
    }
}