using NoLimitTexasHoldemV2.Data;            //To use HandRepository.cs
using Microsoft.AspNetCore.Mvc;             //To define API controllers and endpoints
using System.Text.Json;                     //To use JsonOptions
using System.Text.Json.Serialization;       //To use JsonSerializerOptions

public class Program
{
    public static void Main(string[] args)
    {
        //Creating an instance of WebApplicationBuilder to configure services and the app's request pipeline
        var builder = WebApplication.CreateBuilder(args);

        //Enabling use ofcontrollers for handling HTTP requests
        builder.Services.AddControllers()
            //Configuring json serialization options
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        //Add support for generating API documentation
        builder.Services.AddEndpointsApiExplorer();

        //Add Swagger support
        builder.Services.AddSwaggerGen();

        //HandRepository will ahve a Scoped lifetime (new instance created per request)
        builder.Services.AddScoped<IHandRepository, HandRepository>(provider => 
            //A new instance of HandRepository is created using the connection string from the configuration
            new HandRepository(builder.Configuration.GetConnectionString("PokerDatabaseConnection"), 2));

        //Build web app instance
        var app = builder.Build();

        //Statements that come with template. You can infer what their functionalities from the method names
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}