# EntityFramework Core 
* Implementing EntityFramework core ORM is super easy with Asp.net core

## Implementation
* We need to follow these steps:
1. Install these package
	1. `Microsoft.EntityFrameworkCore`
	2. `Microsoft.EntityFrameworkCore.SqlServer`
	3. `Microsoft.EntityFrameworkCore.Tools`
	4. `Microsoft.EntityFrameworkCore.Design`
2. Make folder `Models`
3. Inside Models folder Add class `Book`
```csharp
public class Book
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}
```
4. Make folder `Data`
5. Inside Data folder Add class `BookStoreDBContext`
```csharp
public class BookStoreDbContext : DbContext<ApplicationUser>
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {

    }

    public DbSet<Book> Books { get; set; }
}
```
6. We need to add connection string in the appsettings.json
```csharp
...
"ConnectionStrings": {
    "BookStoreDbConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStoreDB;Integrated Security=True"
  }
...
```
7. In `startup` class we need add this line
```csharp
 services.AddDbContext<BookStoreDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreDbConnectionString"))
                );
```

## Using DI approach in repositories classes
We can use DI to get DbContext obj and use EF core functionalities
```csharp
public interface IBooksRepository
{
    Task<List<Book>> GetAllBooksAsync();

    Task<int> AddNewBookAsync(Book newBook);
}


public class BooksRepository : IBooksRepository
{
    private readonly BookStoreDbContext _bookStoreDbContext
    
    public BooksRepository(BookStoreDbContext bookStoreDbContext)
    {
        _bookStoreDbContext = bookStoreDbContext;
    }

    public async Task<List<Book>> GetAllBooksAsync() 
    {
        return await _bookStoreDbContext.Books.ToListAsync();
    }

    public async Task<int> AddNewBookAsync(Book newBook) 
    {
        _bookStoreDbContext.Book.Add(newBook);
        await _bookStoreDbContext.SaveChangesAsync();

        return 1;
    }
}
```

## Using repositories methods in the controllers
Follow these steps
1. Add this line in `startup` class
```csharp
services.AddTransient<IBooksRepository, BooksRepository>();
```
2. Make this `BooksController`
```csharp
[Route("api/[controller]")]
[ApiController]
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


    [HttpPost("")]
    public async Task<IActionResult> AddNewBook([FromBody] Book book)
    {
        var result = await _booksRepository.AddNewBook(book);

        return Ok(result);
    }
}
```



