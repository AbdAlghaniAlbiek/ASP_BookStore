using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        public readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public BooksRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<Book>> GetAllBooksAsync()
        {
            //return await _context.Books.Select(b => new Book
            //{
            //    Id = b.Id,
            //    Title = b.Title,
            //    Description = b.Description
            //}).ToListAsync();

            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<Book>>(books);
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            //return await _context.Books.Where(b => b.Id == bookId).Select(b => new Book
            //{
            //    Id = b.Id,
            //    Title = b.Title,
            //    Description = b.Description
            //}).FirstOrDefaultAsync();

            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);
            return _mapper.Map<Book>(book);

        }

        public async Task<int> AddNewBookAsync(Book newBook)
        {
            var customBook = new Book
            {
                Title = newBook.Title,
                Description = newBook.Description
            };

            try
            {
                _context.Books.Add(customBook);
                await _context.SaveChangesAsync();

                return (await _context.Books.OrderBy(b => b.Id).LastOrDefaultAsync()).Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> UpdateBookAsync(Book updatedBook, int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return -1;

            try
            {
                book.Title = updatedBook.Title;
                book.Description = updatedBook.Description;

                await _context.SaveChangesAsync();

                return book.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> UpdateBookPatchAsync(JsonPatchDocument updatedBook, int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return -1;

            try
            {
                book.Title = updatedBook.Operations[0].value.ToString();
                book.Description = updatedBook.Operations[1].value.ToString();

                await _context.SaveChangesAsync();
                return book.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
                return -1;

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}
