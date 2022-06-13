using BookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Repositories
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetAllBooksAsync();

        Task<Book> GetBookAsync(int bookId);

        Task<int> AddNewBookAsync(Book newBook);

        Task<int> UpdateBookAsync(Book updatedbook, int bookId);

        Task<int> UpdateBookPatchAsync(JsonPatchDocument updatedBook, int bookId);

        Task<int> DeleteBookAsync(int bookId);
    }
}