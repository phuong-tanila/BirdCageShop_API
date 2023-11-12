using BirdCageShop.Middlewares;
using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repositories;
using Repositories.Implements;
using System.Configuration;
using System.Text;
using System.Text.Json;
using static Repositories.Commons.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpContextAccessor();
var allowAllPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowAllPolicyName, builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<RoleDAO>();
builder.Services.AddScoped<CustomerDAO>();
builder.Services.AddScoped<ComponentDAO>();
builder.Services.AddScoped<VoucherDAO>();
builder.Services.AddScoped<CageDAO>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<FeedbackDAO>();
builder.Services.AddScoped<SmsDAO>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICageRepository, CageRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ISmsOtpRepository, SmsOtpRepository>();

builder.Services.AddDbContext<BirdCageShopContext>();

builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<BirdCageShopContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),

        ClockSkew = TimeSpan.Zero
    };
});

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EnableLowerCamelCase();
modelBuilder.EntitySet<Customer>("Customers");
modelBuilder.EntitySet<Role>("Roles");
modelBuilder.EntitySet<Component>("Components");
modelBuilder.EntitySet<Cage>("Cages");
modelBuilder.EntitySet<Order>("Orders");
modelBuilder.EntitySet<Voucher>("Vouchers");

modelBuilder.EntityType<Cage>();
modelBuilder.EntityType<CageComponent>();
modelBuilder.EntityType<Component>();
modelBuilder.EntityType<OrderDetail>();
modelBuilder.EntityType<Customer>();
modelBuilder.EntityType<Voucher>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddOData(
        opt =>
            opt.Select()
            .Count().Filter()
            .OrderBy().SetMaxTop(100)
            .SkipToken().Expand()
        .AddRouteComponents("odata", modelBuilder.GetEdmModel()
    )
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMappers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(allowAllPolicyName);
app.UseMiddleware<ErrorMiddleWare>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
