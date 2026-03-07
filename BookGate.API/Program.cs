using BookGate.Application.Services;
using BookGate.Application.Mappings;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using BookGate.Infrastructure.Migrations;
using BookGate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BookGate.Infrastructure")));
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Đường dẫn bị đẩy về nếu chưa đăng nhập
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Giữ đăng nhập 7 ngày
    });
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<PublisherService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<MemberBookService>();
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();
app.UseAuthentication(); // Kích hoạt kiểm tra vé (Cookie)
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseStaticFiles();;
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

