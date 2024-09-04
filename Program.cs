using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiProject.DAL;
using WebApiProject.Data;
using WebApiProject.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCaching();

// JWT Authentication configuration
//var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
// Replace with a strong secret key
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
    options.AddPolicy("UserAndManagerPolicy", policy => policy.RequireRole("User", "Admin", "Manager"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager", "Admin"));

});

// Register services
builder.Services.AddAuthorization();

// connection service
builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register services

// Register Repositories (Data Access Layer)
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ICustomer, CustomerRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();
builder.Services.AddScoped<IOrder, OrderRepo>();
builder.Services.AddScoped<IOrderDetail, OrderDetailRepo>();
builder.Services.AddScoped<IUser, UserRepo>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkRepo>();
// Register Services (Service Layer)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
