using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Walks.Api.Data;
using Walks.Api.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inject the Database as DB dependency.
builder.Services.AddDbContext<WalksDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddDbContext<WalksAuthDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbConnection"))); 

//Inject the repo as service.
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepo, WalkRepo>();
builder.Services.AddScoped<ITokenRepository, TokenRepository > (); 
builder.Services.AddScoped<IImageRepository,  ImageRepository>();


//Inject the identity.
builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("WalksAuth")
        .AddEntityFrameworkStores<WalksAuthDbContext>()
        .AddDefaultTokenProviders();


//Configure.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
}); 


//Add authentication sceheme.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        }); 
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


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
    RequestPath = "/Images"
}); 

app.MapControllers();

app.Run();
