using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using REBOOK.Models;

namespace REBOOK.Data
{
    public class RebookDbContext : DbContext
    {
        private readonly bool _useInMemory;
        public RebookDbContext(){}
        public RebookDbContext(DbContextOptions<RebookDbContext> options) : base(options) {}
        public RebookDbContext(bool useInMemory)
        {
            _useInMemory = useInMemory;
        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<BooksLiked> BooksLiked { get; set; }
        public DbSet<Matches> Matches { get; set;  }
        public DbSet<UsersRated> UsersRated { get; set;  }
        public DbSet<ChatUsers> ChatUsers { get; set;  }
        public DbSet<Message> Message { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatConnections> ChatConnections { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.BooksLiked)
                .WithOne(bl => bl.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.UsersRated)
                .WithOne(u => u.Rated)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Owner)
                .WithMany(user => user.BookList)
                .HasForeignKey(book => book.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            modelBuilder.Entity<Book>()
                .HasMany(b => b.BooksLiked)
                .WithOne(bl => bl.Book)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<BooksLiked>()
                .HasKey(bl => new {bl.UserId, bl.BookId});
            
            modelBuilder.Entity<Matches>()
                .HasKey(m => new {m.UserId1, m.UserId2});

            modelBuilder.Entity<UsersRated>()
                .HasKey(ur => new {ur.RaterId, ur.RatedId});

            modelBuilder.Entity<ChatUsers>()
                .HasKey(chatUser => chatUser.Id);

            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id);
            
            modelBuilder.Entity<Chat>()
                .HasKey(chat => chat.Id);
            
            modelBuilder.Entity<ChatConnections>()
                .HasKey(ChatConnections => ChatConnections.Id);

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                if (_useInMemory)
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();
                    optionsBuilder.UseInMemoryDatabase("Data Source = tests.db", new InMemoryDatabaseRoot());
                    optionsBuilder.UseInternalServiceProvider(serviceProvider);
                    optionsBuilder.EnableServiceProviderCaching(false);
                }
                else 
                {
                     optionsBuilder.UseMySql("server=localhost;user=root;password=rocknroll;database=rebookdb",
                         new MySqlServerVersion(new Version(8, 0, 21)),
                         MySqlOptions =>
                             MySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
                     
                }
            }
        }
    }
}