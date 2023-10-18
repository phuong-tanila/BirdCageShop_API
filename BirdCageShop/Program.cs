using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repositories;
using Repositories.Implements;
using System.Text;
using static Repositories.Commons.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<RoleDAO>();
builder.Services.AddScoped<VoucherDAO>();
builder.Services.AddScoped<CageDAO>();


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICageRepository, CageRepository>();

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),

        ClockSkew = TimeSpan.Zero
    };
});

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Component>("Components");
modelBuilder.EntitySet<Voucher>("Vouchers");
modelBuilder.EntitySet<Cage>("Cages");


modelBuilder.EntityType<Component>();
modelBuilder.EntityType<Voucher>();
modelBuilder.EntityType<CageComponent>();


builder.Services.AddControllers()
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
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
