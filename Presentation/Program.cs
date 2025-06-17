using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Models;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(x => 
{
    x.Password.RequiredLength = 8;
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();


app.UseCors("AllowFrontend");
app.MapOpenApi();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
