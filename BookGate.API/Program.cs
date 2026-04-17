using BookGate.Application.Services;
using BookGate.Application.Mappings;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using BookGate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- CẤU HÌNH DATABASE ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BookGate.Infrastructure")));

// --- CẤU HÌNH AUTHENTICATION ---
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// ==========================================
// 1. THÊM DỊCH VỤ SESSION VÀO ĐÂY
// ==========================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Lưu đơn hàng VNPAY tạm trong 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// --- TIÊM DEPENDENCY INJECTION (Repository) ---
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

// --- TIÊM DEPENDENCY INJECTION (Service) ---
builder.Services.AddScoped<PublisherService>();
builder.Services.AddHttpClient<GhnService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<MemberBookService>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderStatusService>();
builder.Services.AddScoped<OrderDetailService>();

builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// ==========================================
// THỨ TỰ MIDDLEWARE (Rất quan trọng, không đổi thứ tự này)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Bắt buộc phải có dòng này trước Session

// 2. GỌI SESSION TRƯỚC KHI AUTHENTICATION
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();