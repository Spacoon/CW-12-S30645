using Cwiczenia12.Data;
using Cwiczenia12.Services;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<TripContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}

