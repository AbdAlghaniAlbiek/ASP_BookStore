# Controllers

* [Introduction](#introduction)
* [Action methods](#action-methods)
* [Custom model binder](#custom-model-binder)
  

## Introduction
* Every Asp.net web app have apis it should be placed as actions in the controllers.
* Every controller class must flaged by attribute called `[ApiController]` and it's excluding from `ControllerBase `class
```csharp
[ApiController]
public class BooksController : ControllerBase
{
}
```
* To make this controller accessible as base apis for client apps you should add specify route for it by adding `[Route]` attribute
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
}
```


## Action methods
* If the controller have only one action it will triggered automatically without specify a custom route for it
```csharp
Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    public string GetBookName()
    {
        return  "C# in nutshell";
    }
}
```
* If the controller have many of action methods we can resolve by adding `[Route("[action]")]` on every action method (note: if we didn't do that the server will have conflict with any of these action methods it will use)
```csharp
Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [Route("[action]")]
    public string GetBookName()
    {
        return  "C# in nutshell";
    }

    [Route("[action]")]
    public string GetJavaBookName()
    {
        return  "Java in nutshell";
    }
}
```
OR simply by using custom name for the route (in this example every 'action method' we can reach it by both endpoints)
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [Route("get-book")]
    [Route("getbook")] 
    public string GetBookName()
    {
         return  "C# in nutshell";
    }

    [Route("get-some-book")]
    [Route("getsomebook")]
    public string GetJavaBookName()
    {
        return  "Java in nutshell";
    }
}
```

* If we want to make post, delete, put, patch ..etc HTTP requests, we should add these attributes like `[HttpPost("")]`
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("")]
    public List<Book> GetAllBooks(){}

    [HttpPost("")]
    public void AddBook(){}

    [HttpDelete("")]
    public void DeleteBook(){}
}
```

### ActionResult with status code 
* To get result from action methods with status code and in asynchrony way you can use `IActionResult` interface
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("")]
    public Task<IActionResult> GetAllBooks(){}

    [HttpPost("")]
    public Task<IActionResult> AddBook(){}

    [HttpDelete("")]
    public Task<IActionResult> DeleteBook(){}
}
```
* Action methods that get response data with status code must return one of these methods:
1. 200: everything is okay and get data successfully => OK() || OK(object)
2. 201: everything is okay and add a new resource to the server => Created("~/api/animals", object)
3. 204: request has succeeded, but that the client doesn't need to navigate away from its current page => NoContent()
4. 301: To redirect to some another route permanently (action method) => LocalRedirectPermanent("~/api/books/some-route");
5. 302:To redirect to some another route (action method) => LocalRedirect("~/api/books/some-route");
6. 400: There is error from the client side like parameters not correct => BadRequest() || BadRequest(object)
7. 401: A user not having the permission to access resources from the server => Unauthorized() || Unauthorized(object)
8. 403: A user can't reach to specific resource cause he's from abandoned region => forbidden() || forbidden(object)
9. 404: Not getting anything from the server => NotFound() || NotFount(object)

like:
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("")]
    public Task<IActionResult> GetAllBooks(){ ... return Ok(books);}
}
```

## Getting data from Path params, Query params, Body or Header
### Path (Route) params
* To get data from the route directly you can define the same name of path parameter in the parameters of action method
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetBookById(int id){}
}
```

* If you want to make some roles to the incoming path parameters you can do it like this
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("{id:int:min(10):max(20)}")]
    public Task<IActionResult> GetBookById(int id){}

    [HttpGet("names/{name:minlength(20):maxlength(20):length(10)}")]
    public Task<IActionResult> GetBookByName(string name){}

    //Not should contains on numbers
    [HttpGet("alph-names/{name:alpha}")]
    public Task<IActionResult> GetBookByAlphaName(string name) {}

    //Specify a regex based value to path param
    [HttpGet("regex/{name:regex(a(b|c))}")]
    public Task<IActionResult> GetBookByRegexName(string name) {}
}
```
* If the path params data with name is different from the declare it in action method parameters we can use `[FromRoute(Name = "")]` attribute
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetBookById([FromRoute(Name = "id")]int bookId){}
}
``` 

### Query params
To get data from query params you can simply use `[FromQuery(Name = "")]` attribute
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("")]
    public Task<IActionResult> GetBookByName([FromQuery(Name = "name")]string bookName){}
}
``` 

### Body 
#### Raw json
To get data from body of HTTP request you need follow these steps:
1. Install `Microsoft.AspNetCore.Mvc.NewtonsoftJson` package .
2. Add this line in `ConfigureServices` method in `startup` class
```csharp
services.AddControllers().AddNewtonsoftJson();
```
3. Use `[JsonProperty(PropertyName = "")]` attribute to determine how to be serialization/deserialization process the for properties of specified object will be.
```csharp
public class Book
{
    [JsonProperty(PropertyName = "name")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }
}
```
4. Use `[FromBody]` attribute in the action method parameters
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpPost("")]
    public Task<IActionResult> AddNewBook([FromBody] Book book){}
}
```

#### Form-data
* If you want to use body form-data instead of raw-json you should define a property from the object that you want in the controller and assign `[BindProperty]` to it
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [BindProperty()]
    public Book BookObj { get; set; }

    [HttpPost("")]
    public Task<IActionResult> AddNewBook(){}
}
```

* By default we can't use body data in HTTP Get requests to apply this we can use  
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [BindProperty(SupportsGet = true)]
    public Book BookObj { get; set; }

    [HttpGet("")]
    public Task<IActionResult> AddNewBook(){}
}
```

* If you have multiple properties you can use `[BindProperties(SupportsGet = true)]` attribute in the controller scope
```csharp
[Route("api/[controller]")]
[ApiController]
[BindProperties(SupportsGet = true)]
public class BooksController : ControllerBase
{
    public Book BookObj { get; set; }
    public Book BookObj2 { get; set; }
    public Book BookObj3 { get; set; }

    [HttpGet("")]
    public Task<IActionResult> AddNewBook(){}
}
```

#### Json data in HTTP patch requests
* The main difference between put request and patch request is that put request is used to modify all record's data of a table but patch request is used to modify some data of the raw 
* We need follow these steps:
1. Install `Microsoft.AspNetCore.JsonPatch` package 
2. Install `Microsoft.AspNetCore.Mvc.NewtonsoftJson` package
3. Add this line in `ConfigureServices` in `startup` class 
```csharp
services.AddControllers().AddNewtonsoftJson();
```
4. The incoming data from the body will be like:
```csharp
[
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
            
```
5. the type of parameter that will hold the data from the body is `JsonPatchDocument`
```csharp
[HttpPatch("{id}")]
public async Task<IActionResult> UpdateBookPatch([FromBody] JsonPatchDocument updatedBook, [FromRoute(Name = "id")] int bookId)
{
    ...
    var firstValue = updateBook.Operations[0].value as string;
    var secondValue = updateBook.Operations[1].value as string;
    ...
}
```


### Header
To get data from the headers for HTTP requests you can use `[Header]` attribute in the action method parameters
```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("")]
    public Task<IActionResult> GetBookData([FromHeader(Name = "developer")] string developerName, [FromHeader(Name = "auth")] string authorization)
    {
        return Ok($"{developer} -- {authorization}");
    }
}
```

## Custom model binder
* In case if the data in route, query, body section and these data need some work to be enabled to work with, then we need to use Custom binder. Please follow these steps:
1. Add new folder `ModelBinders`
2. Create class `CustomBinder` and make it implement `IModelBinder` interface
```csharp
public class CustomBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var data = bindingContext.HttpContext.Request.Query;

        var result = data.TryGetValue("countries", out var countries);

        if (result)
        {
            var array = countries.ToString().Split("|");

            bindingContext.Result = ModelBindingResult.Success(array);
        }

        return Task.CompletedTask;
    }
}
```
3. Add `[ModelBinder(typeof(CustomBinder))]` in the controller's parameters
```csharp
[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    [HttpPost("")]
    public Task<IActionResult> GetCountryCustomBinder([ModelBinder(typeof(CustomBinder))] string[] countries)
    {
        return Ok(countries);
    }
}
```
4. let's assume that the data in query params like: `...?names:Ahmad|Mouhammad|Rami|Sami` , the result when call this api will like: `["Ahmad", "Mouhammad", "Rami", "Sami"]`.