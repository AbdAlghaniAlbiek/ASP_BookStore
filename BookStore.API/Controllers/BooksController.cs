using AutoMapper;
using BookStore.API.Models;
using BookStore.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _booksRepository.GetAllBooksAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            var book = await _booksRepository.GetBookAsync(id);

            if (book is null)
                return NotFound();

            return Ok(book);
        }


        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book)
        {
            var bookId = await _booksRepository.AddNewBookAsync(book);

            if (bookId == 0)
                return NoContent();

            return CreatedAtAction(nameof(GetBook), new { id = bookId, controller = "books" }, bookId);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromBody] Book updatedBook, [FromRoute(Name = "id")] int bookId)
        {
            var result = await _booksRepository.UpdateBookAsync(updatedBook, bookId);

            switch (result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return NoContent();
                default:
                    return Ok(result);
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookPatch([FromBody] JsonPatchDocument updatedBook, [FromRoute(Name = "id")] int bookId)
        {
            /* schema of incoming json data from the body
             * [
                {
                    "op": "replace",
                    "path": "title",
                    "value": "Javascript"
                },
                {
                    "op": "replace",
                    "path": "description",
                    "value": "This is a description for Javascript language"
                }
               ]
             */

            var result = await _booksRepository.UpdateBookPatchAsync(updatedBook, bookId);

            switch (result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return NoContent();
                default:
                    return Ok(result);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute(Name = "id")] int bookId)
        {
            var result = await _booksRepository.DeleteBookAsync(bookId);

            switch (result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return NoContent();
                default:
                    return Ok("Item is removed successfully");
            }
        }
    }
}
