# EntityFrameworkCore guide
This project implement all functionalities from scaffolding, connect Ef core with sql server db and using EF methods ...etc

>Note
>This configuation is good for these projects UWP, WPF, Console ... 
>If you work on ASP.net app it's preferred to use the configuration that inside it by default

## Dependencies: 
1. Microsoft.EntityFrameworkCore
2. Microsoft.EntityFrameworkCore.SqlServer
3. Microsoft.EntityFrameworkCore.Tools
4. Microsoft.EntityFrameworkCore.Proxies

## Setup
1. Install these [Dependencies](#dependencies)
2. Add class `ApplicationDbContext` and make inherit from `DbContext` 
2. Make classes that represents as classes and therir properties will appear like columns 
```csharp
public class Author 
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    [InverseProperty(nameof(Book.Author))]
    public virtual ICollection<Book> Books { get; set; }
}

public class Book 
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    public int Price { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    [InverseProperty("Books")]
    public virtual Author Author { get; set; }

}
```
3. Make these proprties inside ApplicationDbContext
```csharp
public class ApplicationDBContext : DbContext 
{
    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }
} 
```
4. Override `OnConfiguration` virtual method of DbContext 
```csharp
optionsBuilder
    .UseLazyLoadingProxies()
    .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EfCore;Integrated Security=True",
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
```
5. Use `add-migration <migration name>` statement in `Package Manager Console` to make Initial migration cs file that descripe how schema of tables will be in db
5. Upload this migration file using `update-database` to create tables inside sql server db
> you can see the created db using ""SQL Server Object Explorer"" panel 


## EFCore command lines
1. `add-migration <migration-file-name>` for creating cs file that descripe how schema of tables will be in db
2. `update-database` will upload the configuration inside last migration file to specified db (sql server, sqlite, mysql)
3. `update-database <migration-file-name>` this statement will fire `Down` method starting from migration file that it connected with to the migration file that you specified, and then you can delete these files that it was conncected with
4. `remove-migration` remove last migration that not connected/uploaded to DBMD
5. `remove-migration <migration-file-name>` removes all migration files by firing `Down` method for each oth them until it comes to migraiont file that you specified 
6. `script-migration` get all migration files that you created
7. `get-migration` get all migration files that connected to db server


## EFCore methods
first of all you need to make object from `ApplicationDbContext`
```csharp
var _context = new ApplicationDbContext();
```
>VERY IMPORTANT NOTE:
>You can use Async programming approach with implementing "Microsoft.EntityFrameworkCore" namespace 
>And methods of EFCore will be like this: ToList() => ToListAsync()

### ToList
Give all rows 
```csharp
List<Book> books = _context.Books.ToList();
foreach (var book in books)
Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### SingleOrDefault
Give one item, and if not exist will return null
```csharp
var book = _context.Books.SingleOrDefault(b => b.Id == 1000);

Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### Find
Give one item, and if not exist will throw exception
```csharp
var book = _context.Books.Firs(b => b.Id > 100);

Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### FirstOrDefault
Give the first item
```csharp
var book = _context.Books.First(b => b.Id > 100);

Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### LastOrDefault
Give the last item, but it need order by with it
```csharp
var book = _context.Books.OrderBy(b => b.Id).LastOrDefault(b => b.Id < 100);

Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### Where
Make condition for accepting item in query
```csharp
var books = _context.Books.Where(b => b.Name.StartsWith("a")).ToList();

foreach (var book in books)
    Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### Any
Return true if one item acheive the condition
```csharp
var books = _context.Books.Any(b => b.Id > 500);

Console.WriteLine(books);
```

### All
Return true if all items of table acheive the condition
```csharp
var books = _context.Books.All(b => b.Id > 0);

Console.WriteLine(books);
```

### Append
Add an item to returned collection
```csharp
var books = _context.Books
    .Where(b => b.Id > 900).ToList()
    .Append(new book { Id = 1001, Name = "test" });
        
foreach (var book in books)
    Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### Prepend
Add an item in beggining of returned collection
```csharp
var books = _context.Books
    .Where(b => b.Id > 900).ToList()
    .Prepend(new book { Id = 1001, Name = "test" });
 
foreach (var book in books)
    Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
```

### Average
Calculate the average of returned values of query
```csharp
var avgBooksBalance = _context.Books.Where(b => b.Id > 500).Average(b => b.Balance);

Console.WriteLine(avgBooksBalance);
```

### Sum
Calculate the sum of returned values of query
```csharp
var sumBooksBalance = _context.Books.Where(b => b.Id > 500).Sum(b => b.Balance);

Console.WriteLine(sumBooksBalance);
```

### Count
Calculate the count of returned values of query
```csharp
var countIds = _context.Books.Count(b => b.Id > 700);

Console.WriteLine(countIds);
```

### LongCount
Calculate the long count of returned values of query
```csharp
var countIds = _context.Books.LongCount(b => b.Id > 700);

Console.WriteLine(countIds);
```

### Max
Return Max value of returned values of query
```csharp
var maxBook = _context.Books.Where(b => b.Id < 500).Max(b => b.Id);

Console.WriteLine(maxBook);
```

### Min
Return Min value of returned values of query
```csharp
var minBook = _context.Books.Where(b => b.Id < 500).Min(b => b.Id);

Console.WriteLine(minBook);
```

### OrderBy / OrderByDescending  &&  ThenBy / ThenByDescending
Ordering values based on some properties
```csharp
var stocks = _context.Stocks
    .Where(s => s.Id <= 50)
    .OrderBy(s => s.Balance)
    .ToList();

var stocks = _context.Stocks
    .Where(s => s.Id <= 50)
    .OrderByDescending(s => s.Balance)
    .ToList();

var stocks = _context.Stocks
    .Where(s => s.Id <= 100)
    .OrderBy(s => s.Industry)
    .ThenBy(s => s.Balance)
    .ToList();

var stocks = _context.Stocks
    .Where(s => s.Id <= 100)
    .OrderBy(s => s.Industry)
    .ThenByDescending(s => s.Balance)
    .ToList();

foreach (var stock in stocks)
    Console.WriteLine($"Industry: {stock.Industry}, Balance: {stock.Balance}");
```

### Projection data with select
Projection data and convert it from face to another face
```csharp
var stocks = _context.Stocks.Select(s => new { StockId = s.Id, StockName = s.Name });
// OR
var stocks = _context.Stocks.Select(s => new Blog { BlogId = s.Id, Url = s.Name });
//
foreach (var stock in stocks)
    Console.WriteLine($"ID: {stock.BlogId}, Name: {stock.Url}");
```

### Distinct
Return Unsimilar values
```csharp
var stocks = _context.Stocks.Select(s => new { s.Industry }).Distinct();

foreach (var stock in stocks)
    Console.WriteLine(stock.Industry);
```

### Skip  && Take
Return results from query from `skip` and with number is equal `take`
```csharp
var stocks = _context.Stocks.Skip(10).Take(10).ToList();

foreach (var stock in stocks)
    Console.WriteLine(stock.Id);
```

###  Group By
Grouping results of operations (sum, average, count) based on columns
```csharp
var industries = _context.Stocks
    .GroupBy(s => s.Industry)
    .Select(s => new { Name = s.Key, IndCount = s.Count(), IndBalance = s.Sum(s => s.Balance), IndAvg = s.Average(s => s.Balance) })
    .OrderByDescending(i => i.IndBalance)
    .Take(10)
    .ToList();

foreach (var industry in industries)
    Console.WriteLine($"{industry.Name}  -  {industry.IndCount}  -  {industry.IndBalance}");
```

###  Join
Return results from multiple tables/entities
```csharp
var books = _context.Books
    .Join(
        _context.Authors,
        book => book.AuthorId,
        author => author.Id,
        (book, author) => new
        {
            BookId = book.Id,
            BookName = book.Name,
            AuthorName = author.Name,
            AuthorNationalifyId = author.NationalityId
        }
    )
    .Join(
        _context.Nationalities,
        book => book.AuthorNationalifyId,
        nationalify => nationalify.Id,
        (book, nationality) => new
        {
            book.BookId,
            book.BookName,
            book.AuthorName,
            NationalityName = nationality.Name
        }
    )
    .ToList();

foreach (var book in books)
    Console.WriteLine($"{book.BookId} -- {book.BookName} -- {book.AuthorName} -- {book.NationalityName}");
```

### GroupJoin & SelecteMany
Using when you want data from multiple tables and you want to use left join along with query
```csharp
var books = _context.Books
    .Join(
        _context.Authors,
        book => book.AuthorId,
        author => author.Id,
        (book, author) => new
        {
            BookId = book.Id,
            BookName = book.Name,
            AuthorName = author.Name,
            AuthorNationalityId = author.NationalityId
        }
    )
    .GroupJoin(
        _context.Nationalities,
        book => book.AuthorNationalityId,
        nationality => nationality.Id,
        (book, nationality) => new
        {
            Book = book,
            Nationality = nationality
        }
    )
    .SelectMany(
        b => b.Nationality.DefaultIfEmpty(),
        (b, n) => new
        {
            b.Book,
            NationalityName = n.Name
        }
    );


foreach (var book in books)
    Console.WriteLine($"{book.Book.BookId} -- {book.Book.BookName} --  {book.Book.AuthorName} --  {book?.NationalityName} ");
```

###  Tracking && AsNoTracking
Return results from query from `skip` and with number is equal `take`
```csharp
idea(1)
For no tracking all queries that is made it here
_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


idea(2)
No tracking only this object that retrieved from this query
var book = _context.Books.AsNoTracking().SingleOrDefault(b => b.Id == 1);
book.Price = 200;


idea(3)
To get info about the modified entities
var book = _context.Books.SingleOrDefault(b => b.Id == 1);
book.Price = 99;

var trackers = _context.ChangeTracker.Entries();
foreach (var tracker in trackers)
    Console.WriteLine($"{tracker.Entity} - {tracker.State}");


use this statement for the 3 ideas

_context.SaveChanges();
```

### Eager Loading
Load data from tables that related to table that we make a query from it
```csharp
var book = _context.Books
    .Include(b => b.Author)
    .ThenInclude(a => a.Nationality)
    .SingleOrDefault(b => b.Id == 1);

Console.WriteLine($"{book.Name} -- {book.Author.Name} -- {book.Author.Nationality.Name}");
```

### Explicit Loading
Explicit from what we need loading data that related to table that we make a query from it
```csharp
idea(1)
var book = _context.Books.SingleOrDefault(b => b.Id == 1);

_context.Entry(book).Reference(b => b.Author).Load();

Console.WriteLine($"{book.Name} -- {book.Author.Name}");


idea(2)
var author = _context.Authors.SingleOrDefault(a => a.Id == 1);

_context.Entry(author)
    .Collection(a => a.Books)
    .Query()
    .Where(b => b.Name.StartsWith("o"))
    .ToList();

foreach (var book in author.Books)
    Console.WriteLine($"{book.Name}");
```

### Lazy Loading
Load data from tables when it is needed
```csharp
To use Lazy loading follow these steps:
1.Install Package "Microsoft.EntityFrameworkCore.Proxies"
2.Add UseLazyLoadingProxies() method in OnConfiguration => optionsBuilder.UseLazyLoadingProxies().UseSqlServer()
3.Add virtual keyword to every navigation property


var book = _context.Books.SingleOrDefault(b => b.Id == 1);
            
Console.WriteLine($"{book.Name} -- {book.Author.Name} -- {book.Author.Nationality.Name}");
```

### Split queries (works only with Eager loading)
split query that made with Eager loading to multiple queries
```csharp
idea(1)
Make this behavior only for this eager loading query
var book = _context.Books
    .Include(b => b.Author)
    .AsSplitQuery()
    .SingleOrDefault(b => b.Id == 1);


idea(2)
Make this behavior for all eager loading queries
    optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EfCore;Integrated Security=True",
    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));


idea(3)
In case we used the second way and we want a query follow QuerySingleBehavior not Splitting:
var book = _context.Books
    .Include(b => b.Author)
    .AsSingleQuery()
    .SingleOrDefault(b => b.Id == 1);


Console.WriteLine($"{book.Name} -- {book.Author.Name}");
```

### Linq queries in EF
Using Linq functionalities in EF core
```csharp
var books = (from b in _context.Books
                join a in _context.Authors
                on b.AuthorId equals a.Id
                join n in _context.Nationalities
                on a.NationalityId equals n.Id into authornationality
                from an in authornationality.DefaultIfEmpty()
                select new
                {
                    BookId = b.Id,
                    BookName = b.Name,
                    AuthorName = a.Name,
                    NationalityName = an.Name
                }).ToList();


foreach (var book in books)
    Console.WriteLine($"ID: {book.BookId} -- Name: {book.BookName} -- Author: {book.AuthorName} -- Nationality: {book.NationalityName}");
```

### FromSqlRaw
Write sql queries and pass it EF core
```csharp
idea(1)
If you want to use a normal select query
var books = _context.Books.FromSqlRaw("SELECT * FROM Books").ToList();


idea(2)
If you have a stored procedures
var idParameter = new SqlParameter("@Id", 1);
var nameParameter = new SqlParameter("@Name", "Ahmad");

var books = _context.Books.FromSqlRaw("procedure_name", idParameter, nameParameter).ToList();


foreach (var book in books)
    Console.WriteLine($"ID: {book.Id} -- Name: {book.Name}");
```

### Global Query Filtering
Make a Global quries that execute everytime when dealing with table/entity
```csharp
Add This global query filter for all queries that I have created in this app for specified entities

Important Note:

add HasQueryFilter for specified entity ex: modelBuilder.Entity<Blog>().HasQueryFilter(b => b.Posts.Count > 0)


idea(1)
var blogs = _context.Blogs.ToList();


idea(2)
To ignore the query filter that I have created
var blogs = _context.Blogs.IgnoreQueryFilters().ToList();


foreach (var blog in blogs)
    Console.WriteLine(blog.Url);
```

### Add Records
These many ways to add rows inside tables
```csharp
idea(1)
var nationality = new Nationality { Name = "Nationality1" };
_context.Nationalities.Add(nationality);



idea(2)
var nationalities = new List<Nationality>()
    {
    new Nationality{Name = "Nath 1"},
    new Nationality{Name = "Nath 2"},
    new Nationality{Name = "Nath 3"},
    new Nationality{Name = "Nath 4"},
    };
_context.Nationalities.AddRange(nationalities);



idea(3)
var author = new Author
    {
        Name = "Ahmad Madani",
        Nationality = new Nationality
        {
            Name = "Nath 5"
        }
    };
_context.Authors.Add(author);



idea(4)
var nationality = new Nationality
    {
        Name = "Nath 6",
        Authors = new List<Author>()
    {
        new Author{Name = "Abd Alghani"},
        new Author{Name = "Abd Alkarim"},
        new Author{Name = "Abd Allah"}
    }
    };
_context.Nationalities.Add(nationality);


Note:
If you have many of operations like: Add, update, delete you can use this statement for once time.

_context.SaveChanges();
```

### Update Records
There is many ways to update records in the tables
```csharp
idea(1)
var book = _context.Books.SingleOrDefault(b => b.Id == 1);
book.Price = 150;


idea(2)
var book = _context.Books.AsNoTracking().SingleOrDefault(b => b.Id == 1);
var updatedBook = new Book { Id = book.Id, Name = book.Name, AuthorId = book.AuthorId, Price = 200 };
_context.Update(updatedBook);


idea(3)
var book = _context.Books.AsNoTracking().SingleOrDefault(b => b.Id == 1);
var updatedbook = new Book { Id = book.Id, Name = book.Name, AuthorId = book.AuthorId, Price = 300 };
_context.Entry(book).CurrentValues.SetValues(updatedbook);


idea(4)
var books = _context.Books.AsNoTracking().Where(b => b.Price == 20).ToList();
foreach (var book in books)
{
    book.Price = 40;
}
_context.UpdateRange(books);


Note:
If you have many of operations like: Add, update, delete you can use this statement for once time.

_context.SaveChanges();
```

### Remove Records
There is many ways to remove records in the tables
>Note:  
>this way is valid for non relational data, if a table have a foreign key in another table it won't work
>ex: if you Nationality have a foreign key in multiple authors and I tried to remove that record in Nationality table, it will throws an exception
```csharp
idea(1)
var book = _context.Books.Find(7);
_context.Books.Remove(book);


idea(2)
var books = _context.Books.Where(b => b.Price >= 500 && b.Price <= 1000).ToList();
_context.Books.RemoveRange(books);


_context.SaveChanges();
```

### Remove related data
These 3 states when you remove a record that has children in other tables
```csharp
idea(1) Cascade
var blog = _context.Blogs.Find(3);
_context.Blogs.Remove(blog);


idea(2) Restrict
var blog = _context.Blogs.Find(4);
var posts = _context.Posts.Where(p => p.BlogId == 4).ToList();

_context.Posts.RemoveRange(posts);
_context.Blogs.Remove(blog);


idea(3) SetNull
var blog = _context.Blogs.Find(5);
_context.Blogs.Remove(blog);


_context.SaveChanges();
```

### Transactions
Use transactions when you want to execute many queries together and if one them is failed they will fail all or they rollback to specific breakpoint
```csharp
idea(1)
It won't work and then will go to catch block, because the second operation won't succeed

var transaction = _context.Database.BeginTransaction();

try
{
    var nationanlity = new Nationality { Name = "Nath1" };
    _context.Nationalities.Add(nationanlity);
    _context.SaveChanges();

    nationanlity = new Nationality { Id = 10, Name = "Nath2" };
    _context.Nationalities.Add(nationanlity);
    _context.SaveChanges();

    transaction.Commit();

}
catch (Exception)
{
    transaction.Rollback();
}



idea(2)
    Making breakpoints to go back to it if failed in some operation


    var transaciton = _context.Database.BeginTransaction();

try
{
    var nationanlity = new Nationality { Name = "Nathooo1" };
    _context.Nationalities.Add(nationanlity);
    _context.SaveChanges();

    transaciton.CreateSavepoint("FirstSavePoint");

    nationanlity = new Nationality { Id = 10, Name = "Nathoooo2" };
    _context.Nationalities.Add(nationanlity);

    _context.SaveChanges();

    transaciton.Commit();

}
catch (Exception)
{
    transaciton.RollbackToSavepoint("FirstSavePoint");
    transaciton.Commit();
}
```

## Creating and dealing with Tables/Entities
```csharp
#region Gives some specific properties to Tables and its columns using "Fluent" way
// 
new BlogEntityTypeConfiguration().Configure(modelBuilder.Entity<Blog>());
// OR 
modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogEntityTypeConfiguration).Assembly);
//
#endregion
```

```csharp
#region Create table "Post" in db 
// 
// First way:
 public DbSet<Blog> Blogs { get; set; }
// 
// Second way:
 modelBuilder.Entity<Post>();
// 
// Third way: (using navigation property)
 Class Blog 
 {
  ...
  public List<Post> Posts { get; set; }
 }


#endregion
```

```csharp
#region Deleting this Table from db
// 
// Data annotation:
 [NotMapped] (put it on navigation property)
// 
// Fluent:
 modelBuilder.Ignore<Post>();
// 
#endregion
```

```csharp
#region this statement will make EF not listening to the changes will happen to it but the table will exist in db
// 
 modelBuilder.Entity<Blog>()
   .ToTable("Blogs", b => b.ExcludeFromMigrations());
//
#endregion
```

```csharp
#region Specify a custom name for the table
// 
// Data annotation:
 [Table("Pooost")]
//  
// Fluent:
 modelBuilder.Entity<Post>()
   .ToTable("Pooost");
//
#endregion
```

```csharp
#region Specify a custom name for the table
// 
// Data annotation:
 [Table("Pooost")]
//  
// Fluent:
 modelBuilder.Entity<Post>()
   .ToTable("Pooost");
//
#endregion
```

```csharp
#region Specify a custom name for the table under specific scheme
//
// Data annotation:
 [Table("Pooosts", Scheme = "blogging")]
// 
// Fluent:
 modelBuilder.Entity<Post>()
    .ToTable("Pooosts", schema: "blogging");

#endregion
```

```csharp
#region For making a specific scheme is the default 
//
 modelBuilder.HasDefaultSchema("blogging");
//
#endregion
```

```csharp
#region Mapping between a stored view and entity we have created
// 
 modelBuilder.Entity<Post>()
   .ToView("Posts", schema: "blogging");
//
#endregion
```

```csharp
#region Deleting this column from db
//
// Data annotation:
 [NotMapped]
// 
// Fluent
 modelBuilder.Entity<Blog>()
    .Ignore("AddedOn");
//
#endregion
```

```csharp
#region Make this column not null (Required)
// 
// Data annotation:
 [Required()]
//  
// Fluent:
 modelBuilder.Entity<Blog>()
   .Property(b => b.Url)
   .IsRequired();
//
#endregion
```

```csharp
#region Give column specific name
// 
// Data annotation
 [Column("BlogUrl")]
// 
// Fluent
 modelBuilder.Entity<Blog>()
    .Property(b => b.Url)
    .HasColumnName("BlogUrl");
//
#endregion
```

```csharp
#region Give column a specific type value in db
//
// Data annotation:
 [Column(TypeName = "decimal(5,2)")]
// 
// Fluent:
 modelBuilder.Entity<Blog>(eb =>
 {
    eb.Property(b => b.Url).HasColumnType("varchar(300)");
    eb.Property(b => b.Rating).HasColumnType("decimal(5,2)");
 });
//
#endregion
```

```csharp
#region Specify of length of string
// 
// Data annotation:
 [MaxLength(50)]
// 
// Fluent:
 modelBuilder.Entity<Blog>()
    .Property(b => b.Url).HasMaxLength(50);
//
#endregion
```

```csharp
#region Specify of length of string
// 
// Data annotation:
 [MaxLength(50)]
// 
// Fluent:
 modelBuilder.Entity<Blog>()
    .Property(b => b.Url).HasMaxLength(50);
//
#endregion
```

```csharp
#region For Adding comment for the column of entity
//
// Data annotation;
 [Comment("For Adding comment for the column of entity")]
// 
// Fluent:
 modelBuilder.Entity<Blog>()
    .Property(b => b.Url)
    .HasComment("For Adding comment for the column of entity");
//
#endregion
```

```csharp
#region Specify the PK with custom name for this table 
// 
// Data annotation:
 [Key] (doesn't support the ability to change the key name)
// 
// Fluent:
 modelBuilder.Entity<Book>()
    .HasKey(b => b.BookKey)
    .HasName("PK_BookKey");
//
#endregion
```

```csharp
#region Create composite primary key 
// 
// Fluent:
 modelBuilder.Entity<Book>()
    .HasKey(b => new { b.Name, b.Author })
    .HasName("PK_Books");
//
#endregion
```

```csharp
#region Set default values for columns
// 
// Fluent:
 modelBuilder.Entity<Blog>()
   .Property(b => b.Rating)
   .HasDefaultValue(0);
//
 modelBuilder.Entity<Blog>()
    .Property(b => b.AddedOn)
    .HasDefaultValueSql("GETDATE()");
//
#endregion
```

```csharp
#region Generate a virtual column "DisplayName" contains on the values of each FirstName & LastName
//
// Fluent:
 modelBuilder.Entity<Author>()
    .Property(a => a.DisplayName)
    .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
//
#endregion
```

```csharp
#region This Id is byte so EF doesn't recognize it as PK and For creating an Id for this table I used this way
//
// Data annotation:
 [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
// 
// Fluent:
 modelBuilder.Entity<Category>()
    .Property(c => c.Id)
    .ValueGeneratedOnAdd();
//
#endregion
```

```csharp
#region One to one relationship
// 
// Data annotation:
 [ForeignKey("BlogForiegnKey")]
//
// Fluent:
 modelBuilder.Entity<Blog>()
    .HasOne(b => b.BlogImage)
    .WithOne();
//
#endregion
```

```csharp
#region One to many relationship
//
 modelBuilder.Entity<Blog>()
    .HasMany(b => b.Posts)
    .WithOne();
//
//OR
//
//
 modelBuilder.Entity<Post>()
    .HasOne(p => p.Blog)
    .WithMany(b => b.Posts)
    .OnDelete(DeleteBehavior.Cascade)
    .HasForeignKey(p => p.BlogId)
    .HasPrincipalKey(b => b.Id);
//
#endregion
```

```csharp
#region  Many to many relationship
//
modelBuilder.Entity<Post>()
   .HasMany(p => p.Tags)
   .WithMany(t => t.Posts)
   .UsingEntity<PostTag>(
       j => j
           .HasOne(pt => pt.Tag)
           .WithMany(t => t.PostTags)
           .HasForeignKey(pt => pt.TagId),
       j => j
           .HasOne(pt => pt.Post)
           .WithMany(p => p.PostTags)
           .HasForeignKey(pt => pt.PostId),
       j =>
       {
           j.Property(pt => pt.AddedOne).HasDefaultValueSql("GETDATE()");
           j.HasKey(pt => new { pt.PostId, pt.TagId });
       }
   );
//
#endregion
```

```csharp
#region Sequence data (variable that shared between columns of many tables)
//
 modelBuilder.HasSequence<int>("OrderNumber", schema: "Testing");
//
 modelBuilder.Entity<Order>()
    .Property(o => o.OrderNo)
    .HasDefaultValueSql("NEXT VALUE FOR OrderNumber");

 modelBuilder.Entity<OrderTest>()
     .Property(o => o.OrderNo)
     .HasDefaultValueSql("NEXT VALUE FOR OrderNumber");
//
#endregion
```

```csharp
#region Data seed (data for testing)
//
 modelBuilder.Entity<Blog>(eb =>
 {
    eb.Ignore("Rating");
    eb.Ignore("AddedOn");
 });

 modelBuilder.Entity<Blog>()
    .HasData(new Blog { BlogId = 1, Url = "www.google.com" });

 modelBuilder.Entity<Post>()
    .HasData(new Post { Id = 1, Title = "Post 1", Content = "Test 1", BlogId = 1 },
             new Post { Id = 2, Title = "Post 2", Content = "Test 2", BlogId = 1 });
//
#endregion
```

```csharp
#region Scaffolding existing db in MS SQLServer
//
// Basic statement: Note(You can add multiple tags together like: ... -Table -OutputDir -context ....)
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer -Tables Blogs,Posts (This statement will init dbcontext for Blogs and Posts tables only)
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models   (Will create dbcontext and classes in Models folder)
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Data    (Wil create dbcontext only in Data folder and the classes will be creted in Main Assemply)
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer -context ApplicationDbContext (The default name of DbContext class is "_DbName_Context" in out case "EFCoreScaffoldContext" and to make a specific name for DbContext we use this statement)
// scaffold-dbcontext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreScaffold' Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations    (Will generate classes with data annotations rather than using fluent)
//
#endregion
```

```csharp
#region Global query filtering
//
 modelBuilder.Entity<Post>().HasQueryFilter(p => p.Content.Length != 0);
//
 modelBuilder.Entity<Blog>().HasQueryFilter(b => b.Posts.Count > 0);
//
#endregion
```

```csharp
#region Related data behaviors
//
// idea(1) Cascade
modelBuilder.Entity<Blog>()
    .HasMany(b => b.Posts)
    .WithOne(p => p.Blog)
    .OnDelete(DeleteBehavior.Cascade);
//
//
// idea(2) Restrict
modelBuilder.Entity<Blog>()
    .HasMany(b => b.Posts)
    .WithOne(p => p.Blog)
    .OnDelete(DeleteBehavior.Restrict);
//
//
// idea(3) SetNull
modelBuilder.Entity<Blog>()
    .HasMany(b => b.Posts)
    .WithOne(p => p.Blog)
    .OnDelete(DeleteBehavior.SetNull);
//
#endregion
```


