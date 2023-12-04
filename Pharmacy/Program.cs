using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Pharmacy;
using Pharmacy.Entities;
using Pharmacy.Repositories;
using Pharmacy.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = false;
})
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Key)
        };

        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accsessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accsessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accsessToken;
                }

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
{
    p.WithOrigins(
        "http://localhost:3000"
    );
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowCredentials();

    p.SetPreflightMaxAge(TimeSpan.FromDays(1));
}
));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IJWTService, JWTService>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
