# Dependency injection

* [Introduction](#introduction)
* [Types of DI](#types-of-di)
* [Explain types of DI](#explain-types-of-di)
 
## Introduction
* It's process works under Adapter pattern physiologic to make classes abstract as possible to build  them up by the both developer side and tester side as they need.
* Any Asp.net core app have dependency injection feature by default by using some ready methods.
* A quick example can explain how to use this tech:
1. Make folder and name it `Repositories`
1. Add interface `IBookRepository`
```csharp
public interface IBooksRepository
{
    Task<List<Book>> GetAllBooksAsync();

    Task<int> AddNewBookAsync(Book newBook);
}
```
2. Add class `BookRepository` and make implement `IBookRepository`
```csharp
public class BooksRepository : IBooksRepository
{
    public async Task<List<Book>> GetAllBooksAsync() { ... }

    public async Task<int> AddNewBookAsync(Book newBook) { ... }
}
```
3. Make an obj from IBookRepository and put in the controller it as parameter in the constructor
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
}
```
4. The final step depending on what type of DI you want to use
```csharp
services.AddSingleton<IBookRepository, BookRepository>();
```
OR
```csharp
services.AddScoped<IBookRepository, BookRepository>();
```
OR
```csharp
services.AddTransient<IBookRepository, BookRepository>();
```

## Types of DI
There is three main ways: 
1. AddSingleton()
2. AddScoped()
3. AddTransient()

## Explain types of DI

### AddSingleton()
This method make one object from the son and assign it to all its parents in the constructor and this son obj is for all HTTP requests
ex:
```csharp
public class BooksRepository : IBooksRepository
{
    List<string> booksNames = new List<string>(){"Ahmad", "Mouhammad"};
    
    public async Task<List<Book>> GetAllBooksAsync() 
    {    
        return booksNames;
    }

    public async Task<int> AddNewBookAsync(string name) 
    {    
        booksNames.Add(name);
        return 1;
    }
}


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    private readonly IBooksRepository _booksRepository2;

    public BooksController(IBooksRepository booksRepository, IBooksRepository booksRepository2)
    {
        _booksRepository = booksRepository;
        _booksRepository2 = booksRepository2;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> BookOperations([FromRoute] string name)
    {
        await _booksRepository2.AddNewBookAsync();
        var result = await _booksRepository.GetAllBooksAsync();
        return Ok(result);
    }
}
```
* Let's assume that we have now HTTP request with name "Rami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
    "Rami"
]
```
and a second HTTP request with name "Sami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
    "Rami",
    "Sami"
]
```

### AddScoped()
This method make one object from the son and assign it to all its parents in the constructor and in every HTTP request the server will create a new son for that HTTP request
ex:
```csharp
public class BooksRepository : IBooksRepository
{
    List<string> booksNames = new List<string>(){"Ahmad", "Mouhammad"};
    
    public async Task<List<Book>> GetAllBooksAsync() 
    {    
        return booksNames;
    }

    public async Task<int> AddNewBookAsync(string name) 
    {    
        booksNames.Add(name);
        return 1;
    }
}


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    private readonly IBooksRepository _booksRepository2;

    public BooksController(IBooksRepository booksRepository, IBooksRepository booksRepository2)
    {
        _booksRepository = booksRepository;
        _booksRepository2 = booksRepository2;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> BookOperations([FromRoute] string name)
    {
        await _booksRepository2.AddNewBookAsync();
        var result = await _booksRepository.GetAllBooksAsync();
        return Ok(result);
    }
}
```
* Let's assume that we have now HTTP request with name "Rami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
    "Rami"
]
```
and a second HTTP request with name "Sami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
    "Sami",
]
```


### AddTransient()
This method make a new **objects** from the son and assign every son to different father instance in the constructor and for every HTTP request this process will repeat
ex:
```csharp
public class BooksRepository : IBooksRepository
{
    List<string> booksNames = new List<string>(){"Ahmad", "Mouhammad"};
    
    public async Task<List<Book>> GetAllBooksAsync() 
    {    
        return booksNames;
    }

    public async Task<int> AddNewBookAsync(string name) 
    {    
        booksNames.Add(name);
        return 1;
    }
}


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    private readonly IBooksRepository _booksRepository2;

    public BooksController(IBooksRepository booksRepository, IBooksRepository booksRepository2)
    {
        _booksRepository = booksRepository;
        _booksRepository2 = booksRepository2;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> BookOperations([FromRoute] string name)
    {
        await _booksRepository2.AddNewBookAsync();
        var result = await _booksRepository.GetAllBooksAsync();
        return Ok(result);
    }
}
```
* Let's assume that we have now HTTP request with name "Rami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
]
```
and a second HTTP request with name "Sami" => the result will be
```csharp
[
    "Ahmad",
    "Mouhammad",
]
```






