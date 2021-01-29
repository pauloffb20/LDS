
using REBOOK.Models;

namespace REBOOK.Data
{
    public interface IBooksRepo
    {
        
        Book GetBookById(int id);
        void AddBook(Book Book);
        void DeleteBook(int id);
        void EditBook(Book Book);

    }
}

