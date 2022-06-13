# Routing system

* [Getting result from the server using Run()](#getting-result-from-the-server-using-run)
* [Middlewares](#making-some-middleware-using-use-and-custom-middlewares)
* [Routing using Map](#routing-using-map)
* [Routing using Controllers](#routing-using-controllers)

## Getting result from the server using `Run()`
If you use it as alone will block all HTTP requests
``` csharp
app.Run(async context =>
{
    await context.Response.WriteAsync("Hello world \n");
});
```


## Making some middleware using `Use()` and custom middlewares

### Use()
It works like a gates for upcoming requests until they reach to the specified routes and ongoing responses until they reach to end of pipeline
``` csharp
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-1 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-1 2 \n");
});


app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-2 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-2 2 \n");
});


app.Run(async context =>
{
    await context.Response.WriteAsync("Hello world \n");
});
```

## Routing using `Map()`
It used for making a specified routes to be accessible for the client side
``` csharp
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-1 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-1 2 \n");
});


app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-2 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-2 2 \n");
});

app.Map("/some-route", CustomMiddleware);


app.Run(async context =>
{
    await context.Response.WriteAsync("Hello world \n");
});

private void CustomMiddleware(IApplicationBuilder app)
{
    app.Run(async context =>
                await context.Response.WriteAsync("Hello world \n");
           });
}
```


### Custom middlewares
1. Create folder `Middlewares` .
2. Add a new class and make implement to IMiddleware interface.
``` csharp
public class CustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Custom Middleware 1 \n");

            await next(context);

            await context.Response.WriteAsync("Custom Middleware 2 \n");
        }
    }
```
3. In startup class in `ConfigureServices` method add this line
```csharp
services.AddTransient<CustomMiddleware>();
```
4. Complete example in `Configure` method
```csharp
app.UseMiddleware<CustomMiddleware>();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-1 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-1 2 \n");
});


app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware-2 1 \n");

    await next();

    await context.Response.WriteAsync("Middleware-2 2 \n");
});

app.Map("/some-route", CustomMiddleware);


app.Run(async context =>
{
    await context.Response.WriteAsync("Hello world \n");
});

private void CustomMiddleware(IApplicationBuilder app)
{
    app.Run(async context =>
                await context.Response.WriteAsync("Hello world\n");
           });
}
```

## Routing using endpoints
We can make routes using `app.UseEndpoints` instead of using `app.Map`
```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/get-books", async context =>
    {
        await context.Response.WriteAsync("Hello world from MapGet");
    });

    endpoints.MapPost("/get-books", async context =>
    {
        await context.Response.WriteAsync("Hello world from MapPost");
    });

    endpoints.MapDelete("/get-books", async context =>
    {
        await context.Response.WriteAsync("Hello world from MapDelete");
    });
});
```

## Routing using Controllers
1. Add this line in `ConfigureServices` method
```csharp
services.AddControllers();
```
2. Add this line in `Configure` method
```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}
```
3. Make folder and named `Controllers`
4. Add the controllers inside it



