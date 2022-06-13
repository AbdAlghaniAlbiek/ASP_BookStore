# Swagger
* [Description](#description)
* [Implementation](#implementation)
* [Scenarios](#scenarios)

# Description
First of all it depends OpenApi principle for creating docs for APIs so It's a great library that enables you to can make a documentation to your APIs and test them directly from SwaggerUI.

### How it works
1. Display information about the project
2. It makes groups depending on the prefix of controllers name 
for example if you have a controller called **HomeController** The group name will be **Home**.
3. Under each of group/controller will put all action methods inside it.
4. It shows the schema of DTOs (Data transfer objects) in the parameters of action methods how it will be.
5. if action methods need path params or query params or even body form-data, swagger will make fields for incoming data and you can fill these fields with data that you desire.
6. if action methods need body row-json data, swagger will make a field that you can fill it with row json that you want.

## Implementation
Visual Studio can generate all of swagger configuration for you if you check on OpenApi option when you create an ASP.net app.

Or you can do it by your self, Please follow these steps:
1. Install this package `Swashbuckle.AspNetCore`
2. Go to `Startup` class and this code in services
```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "TestApiJWT",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Email = "email@gmal.com",
                Name = "MY Name",
                Url = new Uri("https://www.website.com")
            },
            Description = "This is app for demonstration",
            License = new OpenApiLicense
            {
                Name = "My license",
                Url = new Uri("https://www.website.com/license")
            },
            TermsOfService = new Uri("https://www.website.com/terms-of-service")
        });
});
```
and add these two lines in app object
```csharp
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApiJWT v1"));
``` 
3. Go to `launchSettings.json` file inside properties folder and make sure from existence this line `"launchUrl": "swagger"`
```csharp
"profiles": {
    "IIS Express": {
    "commandName": "IISExpress",
    "launchBrowser": true,
    "launchUrl": "swagger",
    "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
    }
}
```

## Scenarios
* If you want to make your endpoints secured and need JWT token to get work, please open JWT.md file and follow its steps and make sure that JWT is working then when you open SwaggerUI you see that all action methods with `[Authorize]` attribute have Authorize button and you must fill it with valid access token.

* To make a specific action method hidden in SwaggerUI, simply put on that `[ApiExplorerSettings(IgnoreApi = true)]` action method.

* To make a specific action method under a specific group you name it by yourself, simply put this attribute `[ApiExplorerSettings(GroupName = "A group")]` on that action method.