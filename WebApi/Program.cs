using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
namespace WebApi;

public class Program {
   
    static void Main(string[] args) {
       
       // WebApplication Builder Pattern
       WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
       // add http logging 
       builder.Services.AddHttpLogging(opts =>
          opts.LoggingFields = HttpLoggingFields.All);
       // add Controllers
       builder.Services.AddControllers();
      
       // Build the WebApplication
       WebApplication app = builder.Build();
       // use http logging
       app.UseHttpLogging();
       // routing
       app.MapControllers();
       // Run the WebApplication
       app.Run();

   }
}